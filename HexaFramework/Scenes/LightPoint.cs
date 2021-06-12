using HexaFramework.Extensions;
using System;
using System.Numerics;

namespace HexaFramework.Scenes
{
    public class LightPoint
    {
        // Properties
        public Vector4 AmbientColor { get; private set; }

        public Vector4 DiffuseColour { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public Matrix4x4 ViewMatrix { get; set; }
        public Matrix4x4 ProjectionMatrix { get; set; }

        // Methods
        public void SetAmbientColor(float red, float green, float blue, float alpha)
        {
            AmbientColor = new Vector4(red, green, blue, alpha);
        }

        public void SetDiffuseColor(float red, float green, float blue, float alpha)
        {
            DiffuseColour = new Vector4(red, green, blue, alpha);
        }

        public void GenerateViewMatrix()
        {
            // Setup the vector that points upwards.
            Vector3 upVector = Vector3.UnitY;

            // Create the view matrix from the three vectors.
            ViewMatrix = MatrixExtensions.LookAtLH(Position, LookAt, upVector);
        }

        public void GenerateProjectionMatrix()
        {
            // Setup field of view and screen aspect for a square light source.
            float fieldOfView = (float)Math.PI / 2.0f;
            float screenAspect = 1.0f;

            // Create the projection matrix for the light.
            ProjectionMatrix = MatrixExtensions.PerspectiveFovLH(fieldOfView, screenAspect, 0.0001f, 100f);
        }

        public void SetLookAt(float x, float y, float z)
        {
            LookAt = new Vector3(x, y, z);
        }
    }
}