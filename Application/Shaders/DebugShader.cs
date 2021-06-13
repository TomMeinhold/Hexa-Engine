using HexaFramework.Resources;
using HexaFramework.Resources.Buffers;
using HexaFramework.Scenes;
using HexaFramework.Windows;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace App.Shaders
{
    public class DebugShader : Shader, IDebugShader
    {
        public DebugShader(DeviceManager manager, Camera camera)
        {
            Manager = manager;
            Camera = camera;

            var file = new FileInfo("Shaders/debugShader.hlsl");
            // Compile the vertex shader code.
            CompileShader(Manager, file.FullName, "RenderSceneVS", "vs_5_0", out var vBlob);

            // Compile the pixel shader code.
            CompileShader(Manager, file.FullName, "RenderScenePS", "ps_5_0", out var pBlob);

            // Create the vertex shader from the buffer.
            VertexShader = Manager.ID3D11Device.CreateVertexShader(vBlob.BufferPointer, vBlob.BufferSize);
            PixelShader = Manager.ID3D11Device.CreatePixelShader(pBlob.BufferPointer, pBlob.BufferSize);

            // Now setup the layout of the data that goes into the shader.
            // This setup needs to match the VertexType structure in the Model and in the shader.
            InputElementDescription[] inputElements = new InputElementDescription[]
            {
                new InputElementDescription() { SemanticName = "POSITION", SemanticIndex = 0, Format = Format.R32G32B32A32_Float, Slot = 0, AlignedByteOffset = 0, Classification = InputClassification.PerVertexData, InstanceDataStepRate = 0 },
                new InputElementDescription() { SemanticName = "COLOR", SemanticIndex = 0, Format = Format.R32G32B32A32_Float, Slot = 0, AlignedByteOffset = InputElementDescription.AppendAligned, Classification = InputClassification.PerVertexData, InstanceDataStepRate = 0 }
        };

            Compiler.GetInputSignatureBlob(vBlob.BufferPointer, vBlob.BufferSize, out var blob);
            InputLayout = manager.ID3D11Device.CreateInputLayout(inputElements, blob);

            // Release the vertex and pixel shader buffers, since they are no longer needed.
            vBlob.Dispose();
            pBlob.Dispose();
            blob.Dispose();

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
            ConstantMatrixBuffer = Manager.ID3D11Device.CreateBuffer(ref ViewProjectionConstantBuffer, matrixBufDesc);

            BufferDescription debugBufDesc = new()
            {
                Usage = Vortice.Direct3D11.Usage.Dynamic,
                SizeInBytes = Marshal.SizeOf<VertexPositionColor>() * 50000,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = Marshal.SizeOf<VertexPositionColor>()
            };

            DebugBuffer = Manager.ID3D11Device.CreateBuffer(debugBufDesc);

            VertexBufferView = new VertexBufferView(DebugBuffer, Marshal.SizeOf<VertexPositionColor>());
        }

        public DeviceManager Manager { get; }
        public Camera Camera { get; }
        public ID3D11VertexShader VertexShader { get; }
        public ID3D11PixelShader PixelShader { get; }
        public ID3D11InputLayout InputLayout { get; }
        public ID3D11Buffer ConstantMatrixBuffer { get; }
        public ID3D11Buffer DebugBuffer { get; }
        public VertexBufferView VertexBufferView;
        public BufferMatrixType ViewProjectionConstantBuffer;

        private void SetParameters(VertexPositionColor[] vertices)
        {
            ViewProjectionConstantBuffer = new() { Projection = Camera.ProjectionMatrix, View = Camera.ViewMatrix, World = Matrix4x4.Identity, CameraPosition = new Vector4(Camera.Position, 0) };
            Write(Manager, ConstantMatrixBuffer, ViewProjectionConstantBuffer);

            Write(Manager, DebugBuffer, vertices);
        }

        public override void Render(SceneObject sceneObject)
        {
        }

        public void Render(VertexPositionColor[] vertices, PrimitiveTopology topology)
        {
            SetParameters(vertices);

            Manager.ID3D11DeviceContext.VSSetConstantBuffer(0, ConstantMatrixBuffer);
            Manager.ID3D11DeviceContext.VSSetConstantBuffer(1, null);
            Manager.ID3D11DeviceContext.PSSetConstantBuffer(0, null);
            Manager.ID3D11DeviceContext.DSSetConstantBuffer(0, null);
            Manager.ID3D11DeviceContext.DSSetConstantBuffer(1, null);
            Manager.ID3D11DeviceContext.HSSetConstantBuffer(0, null);

            Manager.ID3D11DeviceContext.IASetInputLayout(InputLayout);
            Manager.ID3D11DeviceContext.VSSetShader(VertexShader);
            Manager.ID3D11DeviceContext.HSSetShader(null);
            Manager.ID3D11DeviceContext.DSSetShader(null);
            Manager.ID3D11DeviceContext.PSSetShader(PixelShader);

            Manager.ID3D11DeviceContext.PSSetSampler(0, null);
            Manager.ID3D11DeviceContext.DSSetSampler(0, null);

            Manager.ID3D11DeviceContext.IASetVertexBuffers(0, VertexBufferView);
            Manager.ID3D11DeviceContext.IASetIndexBuffer(null, Format.Unknown, 0);
            Manager.ID3D11DeviceContext.IASetPrimitiveTopology(topology);
            Manager.ID3D11DeviceContext.Draw(vertices.Length, 0);
        }
    }
}