using HexaFramework.Windows;
using HexaFramework.Models.ObjLoader.Loader.Loaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Direct3D11;

namespace HexaFramework.Resources
{
    public class Model : Resource
    {
        public Vertex[] Vertices { get; private set; }

        public int[] Indices { get; private set; }

        public VertexBufferView VertexBufferView { get; private set; }

        public ID3D11Buffer VertexBuffer { get; private set; }

        public ID3D11Buffer IndexBuffer { get; private set; }

        private static Vertex[] LoadModel(string modelFormatFilename)
        {
            try
            {
                List<string> lines = File.ReadLines(modelFormatFilename).ToList();
                var vertexCountString = lines[0].Split(new char[] { ':' })[1].Trim();
                int VertexCount = int.Parse(vertexCountString);
                Vertex[] ModelObject = new Vertex[VertexCount];

                for (var i = 4; i < lines.Count && i < 4 + VertexCount; i++)
                {
                    var modelArray = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    ModelObject[i - 4] = new Vertex()
                    {
                        Position = new Vector4(float.Parse(modelArray[0], NumberStyles.Float), float.Parse(modelArray[1], NumberStyles.Float), float.Parse(modelArray[2], NumberStyles.Float), 1),
                        Texture = new Vector2(float.Parse(modelArray[3], NumberStyles.Float), float.Parse(modelArray[4], NumberStyles.Float)),
                        Normal = new Vector3(float.Parse(modelArray[5], NumberStyles.Float), float.Parse(modelArray[6], NumberStyles.Float), float.Parse(modelArray[7], NumberStyles.Float)),
                    };
                }

                return ModelObject;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public void Load(DeviceManager manager, params Vertex[] vertices)
        {
            Vertices = vertices;
            Indices = new int[Vertices.Length];
            for (var i = 0; i < Vertices.Length; i++)
            {
                Indices[i] = i;
            }
            CalculateModelVectors();
            IndexBuffer = manager.ID3D11Device.CreateBuffer(Indices, new BufferDescription(Marshal.SizeOf<int>() * Indices.Length, BindFlags.IndexBuffer));
            VertexBuffer = manager.ID3D11Device.CreateBuffer(Vertices, new BufferDescription(Marshal.SizeOf<Vertex>() * Vertices.Length, BindFlags.VertexBuffer));
            VertexBufferView = new VertexBufferView(VertexBuffer, Marshal.SizeOf<Vertex>());
        }

        public void Load(DeviceManager manager, string path)
        {
            Vertices = LoadModel(new FileInfo(path).FullName);
            Indices = new int[Vertices.Length];
            for (var i = 0; i < Vertices.Length; i++)
            {
                Indices[i] = i;
            }
            CalculateModelVectors();
            IndexBuffer = manager.ID3D11Device.CreateBuffer(Indices, new BufferDescription(Marshal.SizeOf<int>() * Indices.Length, BindFlags.IndexBuffer));
            VertexBuffer = manager.ID3D11Device.CreateBuffer(Vertices, new BufferDescription(Marshal.SizeOf<Vertex>() * Vertices.Length, BindFlags.VertexBuffer));
            VertexBufferView = new VertexBufferView(VertexBuffer, Marshal.SizeOf<Vertex>());
        }

        public void LoadObj(DeviceManager manager, string path)
        {
            var file = new FileInfo(path);
            var before = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(file.DirectoryName);

            using var fs = file.OpenRead();
            ObjLoaderFactory factory = new();
            var loader = factory.Create();
            var result = loader.Load(fs);
            List<Vertex> vertices = new();
            for (int i = 0; i < result.Groups.Count; i++)
            {
                for (int j = 0; j < result.Groups[i].Faces.Count; j++)
                {
                    for (int jj = 0; jj < result.Groups[i].Faces[j].Count; jj++)
                    {
                        var vertexIndex = result.Groups[i].Faces[j][jj].VertexIndex - 1;
                        var textureIndex = result.Groups[i].Faces[j][jj].TextureIndex - 1;
                        var normalIndex = result.Groups[i].Faces[j][jj].NormalIndex - 1;
                        vertices.Add(new Vertex(result.Vertices[vertexIndex], result.Textures[textureIndex], result.Normals[normalIndex]));
                    }
                }
            }
            Directory.SetCurrentDirectory(before);
            Vertices = vertices.ToArray();
            Indices = new int[Vertices.Length];
            for (var i = 0; i < Vertices.Length; i++)
            {
                Indices[i] = i;
            }
            CalculateModelVectors();
            IndexBuffer = manager.ID3D11Device.CreateBuffer(Indices, new BufferDescription(Marshal.SizeOf<int>() * Indices.Length, BindFlags.IndexBuffer));
            VertexBuffer = manager.ID3D11Device.CreateBuffer(Vertices, new BufferDescription(Marshal.SizeOf<Vertex>() * Vertices.Length, BindFlags.VertexBuffer));
            VertexBufferView = new VertexBufferView(VertexBuffer, Marshal.SizeOf<Vertex>());
        }

        public void CalculateModelVectors()
        {
            // Calculate the number of faces in the model.
            int faceCount = Vertices.Length / 3;

            // Initialize the index to the model data.
            int index = 0;

            Vertex vertex1, vertex2, vertex3;

            // Go through all the faces and calculate the tangent, binormal, and normal vectors.
            for (int i = 0; i < faceCount; i++)
            {
                // Get the three vertices for the face from the model.
                vertex1 = Vertices[index];
                index++;

                // Second Vertrx
                vertex2 = Vertices[index];
                index++;

                // Third Vertex
                vertex3 = Vertices[index];
                index++;

                // Calculate the tangent and binormal of that face.
                CalculateTangentBinormal(vertex1, vertex2, vertex3, out Vector3 tangent, out Vector3 binormal);

                // Calculate the new normal using the tangent and binormal.
                CalculateNormal(tangent, binormal, out Vector3 normal);

                // Store the normal, tangent, and binormal for this face back in the model structure.
                Vertices[index - 1].Normal = normal;
                Vertices[index - 1].Tangent = tangent;
                Vertices[index - 1].Binormal = binormal;

                // Second Vertex
                Vertices[index - 2].Normal = normal;
                Vertices[index - 2].Tangent = tangent;
                Vertices[index - 2].Binormal = binormal;

                // Third Vertex
                Vertices[index - 3].Normal = normal;
                Vertices[index - 3].Tangent = tangent;
                Vertices[index - 3].Binormal = binormal;
            }
        }

        private static void CalculateTangentBinormal(Vertex vertex1, Vertex vertex2, Vertex vertex3, out Vector3 tangent, out Vector3 binormal)
        {
            // Calculate the two vectors for the this face.
            float[] vector1 = new[] { vertex2.Position.X - vertex1.Position.X, vertex2.Position.Y - vertex1.Position.Y, vertex2.Position.Z - vertex1.Position.Z };
            float[] vector2 = new[] { vertex3.Position.X - vertex1.Position.X, vertex3.Position.Y - vertex1.Position.Y, vertex3.Position.Z - vertex1.Position.Z };

            // Calculate the tu and tv texture space vectors.
            float[] tuVector = new[] { vertex2.Texture.X - vertex1.Texture.X, vertex3.Texture.X - vertex1.Texture.X };
            float[] tvVector = new[] { vertex2.Texture.Y - vertex1.Texture.Y, vertex3.Texture.Y - vertex1.Texture.Y };

            // Calculate the denominator of the tangent / binormal equation.
            float den = 1.0f / ((tuVector[0] * tvVector[1]) - (tuVector[1] * tvVector[0]));

            // Calculate the cross products and multiply by the coefficient to get the tangent and binormal.
            tangent.X = ((tvVector[1] * vector1[0]) - (tvVector[0] * vector2[0])) * den;
            tangent.Y = ((tvVector[1] * vector1[1]) - (tvVector[0] * vector2[1])) * den;
            tangent.Z = ((tvVector[1] * vector1[2]) - (tvVector[0] * vector2[2])) * den;

            binormal.X = ((tuVector[0] * vector2[0]) - (tuVector[1] * vector1[0])) * den;
            binormal.Y = ((tuVector[0] * vector2[1]) - (tuVector[1] * vector1[1])) * den;
            binormal.Z = ((tuVector[0] * vector2[2]) - (tuVector[1] * vector1[2])) * den;

            // Calculate the length of this Tengent normal.
            float length = (float)Math.Sqrt((tangent.X * tangent.X) + (tangent.Y * tangent.Y) + (tangent.Z * tangent.Z));

            // Normalize the normal and the store it.
            tangent.X /= length;
            tangent.Y /= length;
            tangent.Z /= length;

            // Calculate the length of this Bi-Tangent normal.
            length = (float)Math.Sqrt((binormal.X * binormal.X) + (binormal.Y * binormal.Y) + (binormal.Z * binormal.Z));

            // Normalize the Bi-Tangent normal and the store it.
            binormal.X /= length;
            binormal.Y /= length;
            binormal.Z /= length;
        }

        private static void CalculateNormal(Vector3 tangent, Vector3 binormal, out Vector3 normal)
        {
            // Calculate the cross product of the tangent and binormal which will give the normal vector.
            normal.X = (tangent.Y * binormal.Z) - (tangent.Z * binormal.Y);
            normal.Y = (tangent.Z * binormal.X) - (tangent.X * binormal.Z);
            normal.Z = (tangent.X * binormal.Y) - (tangent.Y * binormal.X);

            // Calculate the length of the normal.
            var length = (float)Math.Sqrt((normal.X * normal.X) + (normal.Y * normal.Y) + (normal.Z * normal.Z));

            // Normalize the normal.
            normal.X /= length;
            normal.Y /= length;
            normal.Z /= length;
        }
    }
}