using HexaFramework.Resources;
using HexaFramework.Resources.Buffers;
using HexaFramework.Scenes;
using HexaFramework.Windows;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Vortice;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace App.Shaders
{
    public class DefaultShader : Shader
    {
        public DefaultShader(DeviceManager manager, Camera camera)
        {
            Manager = manager;
            Camera = camera;

            var file = new FileInfo("Shaders/defaultShader.hlsl");

            CompileShader(manager, file.FullName, "RenderSceneVS", "vs_5_0", out var vBlob);
            VertexShader = Manager.ID3D11Device.CreateVertexShader(vBlob.BufferPointer, vBlob.BufferSize);

            CompileShader(manager, file.FullName, "RenderScenePS", "ps_5_0", out var pBlob);
            PixelShader = Manager.ID3D11Device.CreatePixelShader(pBlob.BufferPointer, pBlob.BufferSize);

            Compiler.GetInputSignatureBlob(vBlob.BufferPointer, vBlob.BufferSize, out var blob);
            InputLayout = Manager.ID3D11Device.CreateInputLayout(inputElements, blob);

            vBlob.Dispose();
            pBlob.Dispose();
            blob.Dispose();

            var matrixBufferDesc = new BufferDescription(Marshal.SizeOf<BufferMatrixType>(), BindFlags.ConstantBuffer, Vortice.Direct3D11.Usage.Dynamic) { CpuAccessFlags = CpuAccessFlags.Write };
            MatrixBuffer = Manager.ID3D11Device.CreateBuffer(ref BufferMatrixType, matrixBufferDesc);

            var modelBufferDesc = new BufferDescription(Marshal.SizeOf<BufferModelType>(), BindFlags.ConstantBuffer, Vortice.Direct3D11.Usage.Dynamic) { CpuAccessFlags = CpuAccessFlags.Write };
            ModelBuffer = Manager.ID3D11Device.CreateBuffer(ref BufferModelType, modelBufferDesc);

            var lightBufferDesc = new BufferDescription(Marshal.SizeOf<BufferLightType>(), BindFlags.ConstantBuffer, Vortice.Direct3D11.Usage.Dynamic) { CpuAccessFlags = CpuAccessFlags.Write };
            LightBuffer = Manager.ID3D11Device.CreateBuffer(ref BufferLightType, lightBufferDesc);

            SamplerDescription samplerDesc = new()
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MipLODBias = 0,
                MaxAnisotropy = 0,
                ComparisonFunction = ComparisonFunction.Less,
                BorderColor = (Vortice.Mathematics.Color4)Color.FromArgb(0, 0, 0, 0),  // Black Border.
                MinLOD = 0,
                MaxLOD = float.MaxValue
            };

            SamplerState = Manager.ID3D11Device.CreateSamplerState(samplerDesc);
        }

        public readonly ID3D11Buffer MatrixBuffer;
        public readonly ID3D11Buffer ModelBuffer;
        public readonly ID3D11Buffer LightBuffer;
        public readonly ID3D11VertexShader VertexShader;
        public readonly ID3D11PixelShader PixelShader;
        public readonly ID3D11InputLayout InputLayout;
        public readonly ID3D11SamplerState SamplerState;

        public readonly Camera Camera;
        public readonly DeviceManager Manager;

        public BufferMatrixType BufferMatrixType;
        public BufferModelType BufferModelType;
        public BufferLightType BufferLightType;

        public LightDirectional Light;

        public readonly InputElementDescription[] inputElements = new InputElementDescription[]
        {
            new InputElementDescription("POSITION", 0, Format.R32G32B32A32_Float, 0, 0, InputClassification.PerVertexData,0),
            new InputElementDescription("TEXCOORD", 0, Format.R32G32_Float, InputElementDescription.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElementDescription("NORMAL", 0, Format.R32G32B32_Float, InputElementDescription.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElementDescription("TANGENT", 0, Format.R32G32B32_Float, InputElementDescription.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElementDescription("BINORMAL", 0, Format.R32G32B32_Float, InputElementDescription.AppendAligned, 0, InputClassification.PerVertexData, 0)
        };

        public void SetParameters(SceneObject sceneObject)
        {
            BufferMatrixType = new() { Projection = Camera.ProjectionMatrix, View = Camera.ViewMatrix, World = Matrix4x4.Identity, CameraPosition = new Vector4(Camera.Position, 0) };
            Write(Manager, MatrixBuffer, BufferMatrixType);

            BufferModelType = new() { ModelTransform = sceneObject.Transform };
            Write(Manager, ModelBuffer, BufferModelType);

            BufferLightType = new() { Color = Light.DiffuseColour, Direction = Light.Direction, AmbientColor = Light.AmbientColor, SpecularColor = Light.SpecularColor, SpecularPower = Light.SpecularPower };
            Write(Manager, LightBuffer, BufferLightType);
        }

        public override void Render(SceneObject sceneObject)
        {
            SetParameters(sceneObject);
            Manager.ID3D11DeviceContext.VSSetConstantBuffer(0, MatrixBuffer);
            Manager.ID3D11DeviceContext.VSSetConstantBuffer(1, ModelBuffer);
            Manager.ID3D11DeviceContext.PSSetConstantBuffer(2, LightBuffer);

            Manager.ID3D11DeviceContext.IASetInputLayout(InputLayout);
            Manager.ID3D11DeviceContext.VSSetShader(VertexShader);
            Manager.ID3D11DeviceContext.PSSetShader(PixelShader);

            Manager.ID3D11DeviceContext.PSSetSampler(0, SamplerState);
            Manager.ID3D11DeviceContext.DSSetSampler(0, SamplerState);

            Manager.ID3D11DeviceContext.PSSetShaderResources(0, sceneObject.ResourceViews);
            Manager.ID3D11DeviceContext.DSSetShaderResources(0, sceneObject.ResourceViews);
            Manager.ID3D11DeviceContext.IASetVertexBuffers(0, sceneObject.Model.VertexBufferView);
            Manager.ID3D11DeviceContext.IASetIndexBuffer(sceneObject.Model.IndexBuffer, Format.R32_UInt, 0);
            Manager.ID3D11DeviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);
            Manager.ID3D11DeviceContext.DrawIndexed(sceneObject.Model.Indices.Length, 0, 0);
        }
    }
}