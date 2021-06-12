using HexaFramework.Resources;
using HexaFramework.Resources.Buffers;
using HexaFramework.Scenes;
using HexaFramework.Windows;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace App.Shaders
{
    internal class ShaderDepth : Shader
    {
        // Properties.
        public ID3D11VertexShader VertexShader { get; set; }

        public ID3D11PixelShader PixelShader { get; set; }
        public ID3D11InputLayout InputLayout { get; set; }
        public ID3D11Buffer ConstantMatrixBuffer { get; set; }
        public ID3D11Buffer ConstantModelBuffer { get; set; }
        public DeviceManager Manager { get; }
        public Camera Camera { get; }

        // Methods.

        public ShaderDepth(DeviceManager manager, Camera camera, string vsFileName, string psFileName)
        {
            Manager = manager;
            Camera = camera;

            // Compile the vertex shader code.
            CompileShader(Manager, vsFileName, "DepthVertexShader", "vs_5_0", out var vBlob);

            CompileShader(Manager, psFileName, "DepthPixelShader", "ps_5_0", out var pBlob);

            // Compile the pixel shader code.

            // Create the vertex shader from the buffer.
            VertexShader = Manager.ID3D11Device.CreateVertexShader(vBlob.BufferPointer, vBlob.BufferSize);
            PixelShader = Manager.ID3D11Device.CreatePixelShader(pBlob.BufferPointer, pBlob.BufferSize);

            // Now setup the layout of the data that goes into the shader.
            // This setup needs to match the VertexType structure in the Model and in the shader.
            InputElementDescription[] inputElements = new InputElementDescription[]
            {
                    new InputElementDescription()
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    }
            };

            Compiler.GetInputSignatureBlob(vBlob.BufferPointer, vBlob.BufferSize, out var inputSignature);
            InputLayout = manager.ID3D11Device.CreateInputLayout(inputElements, inputSignature);

            // Release the vertex and pixel shader buffers, since they are no longer needed.
            vBlob.Dispose();
            pBlob.Dispose();

            // Setup the description of the dynamic matrix constant buffer that is in the vertex shader.
            BufferDescription matrixBufDesc = new()
            {
                Usage = Vortice.Direct3D11.Usage.Dynamic,
                SizeInBytes = Marshal.SizeOf<BufferMatrixType>(), // was Matrix
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            // Create the constant buffer pointer so we can access the vertex shader constant buffer from within this class.
            ConstantMatrixBuffer = Manager.ID3D11Device.CreateBuffer(matrixBufDesc, IntPtr.Zero);

            BufferDescription modelBufDesc = new()
            {
                Usage = Vortice.Direct3D11.Usage.Dynamic,
                SizeInBytes = Marshal.SizeOf<BufferMatrixType>(), // was Matrix
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            ConstantModelBuffer = Manager.ID3D11Device.CreateBuffer(modelBufDesc, IntPtr.Zero);
        }

        public override void Render(SceneObject sceneObject)
        {
            SetShaderParameters(sceneObject);

            Manager.ID3D11DeviceContext.IASetInputLayout(InputLayout);
            Manager.ID3D11DeviceContext.VSSetShader(VertexShader);
            Manager.ID3D11DeviceContext.PSSetShader(PixelShader);

            Manager.ID3D11DeviceContext.PSSetShaderResources(0, sceneObject.ResourceViews);
            Manager.ID3D11DeviceContext.DSSetShaderResources(0, sceneObject.ResourceViews);
            Manager.ID3D11DeviceContext.IASetVertexBuffers(0, sceneObject.Model.VertexBufferView);
            Manager.ID3D11DeviceContext.IASetIndexBuffer(sceneObject.Model.IndexBuffer, Format.R32_UInt, 0);
            Manager.ID3D11DeviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);
            Manager.ID3D11DeviceContext.DrawIndexed(sceneObject.Model.Indices.Length, 0, 0);
        }

        private void SetShaderParameters(SceneObject sceneObject)
        {
            {
                var buffer = new BufferMatrixType() { Projection = Camera.ProjectionMatrix, View = Camera.ViewMatrix, World = Matrix4x4.Identity, CameraPosition = new Vector4(Camera.Position, 0) };
                Write(buffer, Manager, ConstantMatrixBuffer);
                Manager.ID3D11DeviceContext.VSSetConstantBuffer(0, ConstantMatrixBuffer);
            }
            {
                var buffer = new BufferModelType() { ModelTransform = sceneObject.Transform };
                Write(buffer, Manager, ConstantModelBuffer);
                Manager.ID3D11DeviceContext.VSSetConstantBuffer(1, ConstantModelBuffer);
            }
        }
    }
}