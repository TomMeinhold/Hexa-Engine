using HexaFramework.Resources;
using HexaFramework.Resources.Buffers;
using HexaFramework.Scenes;
using HexaFramework.Windows;
using HexaFramework.Windows.Native;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Vortice;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace App.Shaders
{
    public class ShaderTessellation : Shader
    {
        public ShaderTessellation(DeviceManager manager, Scene scene, Camera camera,
            string vertexShaderPath,
            string hullShaderPath,
            string domainShaderPath,
            string pixelShaderPath,
            string vertexEntry,
            string hullEntry,
            string domainEntry,
            string pixelEntry,
            string vertexVersion,
            string hullVersion,
            string domainVersion,
            string pixelVersion
            )
        {
            Manager = manager;
            Scene = scene;
            Camera = camera;
            var fef = new BufferDescription(Marshal.SizeOf<BufferMatrixType>(), BindFlags.ConstantBuffer, Vortice.Direct3D11.Usage.Dynamic) { CpuAccessFlags = CpuAccessFlags.Write };
            MatrixBuffer = Manager.ID3D11Device.CreateBuffer(ref ViewProjectionConstantBuffer, fef);

            var feff = new BufferDescription(Marshal.SizeOf<BufferModelType>(), BindFlags.ConstantBuffer, Vortice.Direct3D11.Usage.Dynamic) { CpuAccessFlags = CpuAccessFlags.Write };
            ModelBuffer = Manager.ID3D11Device.CreateBuffer(ref ModelConstantBuffer, feff);

            var fefff = new BufferDescription(Marshal.SizeOf<BufferLightType>(), BindFlags.ConstantBuffer, Vortice.Direct3D11.Usage.Dynamic) { CpuAccessFlags = CpuAccessFlags.Write };
            LightBuffer = Manager.ID3D11Device.CreateBuffer(ref LightConstantBuffer, fefff);

            var feffff = new BufferDescription(Marshal.SizeOf<BufferTessellationType>(), BindFlags.ConstantBuffer, Vortice.Direct3D11.Usage.Dynamic) { CpuAccessFlags = CpuAccessFlags.Write };
            TessellationBuffer = Manager.ID3D11Device.CreateBuffer(ref TessellationBufferType, feffff);

            CompileShader(manager, vertexShaderPath, vertexEntry, vertexVersion, out var vblob);
            if (vblob is not null)
                VertexShader = Manager.ID3D11Device.CreateVertexShader(vblob.BufferPointer, vblob.BufferSize);

            CompileShader(manager, hullShaderPath, hullEntry, hullVersion, out var hblob);
            if (hblob is not null)
                HullShader = Manager.ID3D11Device.CreateHullShader(hblob.BufferPointer, hblob.BufferSize);

            CompileShader(manager, domainShaderPath, domainEntry, domainVersion, out var dblob);
            if (dblob is not null)
                DomainShader = Manager.ID3D11Device.CreateDomainShader(dblob.BufferPointer, dblob.BufferSize);

            CompileShader(manager, pixelShaderPath, pixelEntry, pixelVersion, out var pblob);
            if (pblob is not null)
                PixelShader = Manager.ID3D11Device.CreatePixelShader(pblob.GetBytes());

            Compiler.GetInputSignatureBlob(vblob.BufferPointer, vblob.BufferSize, out InputSignature);
            InputLayout = Manager.ID3D11Device.CreateInputLayout(inputElements, InputSignature);

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
        public readonly ID3D11Buffer TessellationBuffer;
        public readonly ID3D11VertexShader VertexShader;
        public readonly ID3D11HullShader HullShader;
        public readonly ID3D11DomainShader DomainShader;
        public readonly ID3D11PixelShader PixelShader;
        public readonly Blob InputSignature;
        public readonly ID3D11InputLayout InputLayout;
        public readonly ID3D11SamplerState SamplerState;

        public readonly Camera Camera;
        public readonly DeviceManager Manager;

        public BufferMatrixType ViewProjectionConstantBuffer;
        public BufferModelType ModelConstantBuffer;
        public BufferLightType LightConstantBuffer;
        public BufferTessellationType TessellationBufferType;

        public LightDirectional Light;
        public int TessellationAmount = 1;

        public readonly InputElementDescription[] inputElements = new InputElementDescription[]
        {
            new InputElementDescription("POSITION", 0, Format.R32G32B32A32_Float, 0, 0, InputClassification.PerVertexData,0),
            new InputElementDescription("TEXCOORD", 0, Format.R32G32_Float, InputElementDescription.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElementDescription("NORMAL", 0, Format.R32G32B32_Float, InputElementDescription.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElementDescription("TANGENT", 0, Format.R32G32B32_Float, InputElementDescription.AppendAligned, 0, InputClassification.PerVertexData, 0),
            new InputElementDescription("BINORMAL", 0, Format.R32G32B32_Float, InputElementDescription.AppendAligned, 0, InputClassification.PerVertexData, 0)
        };

        public Scene Scene { get; }

        public void SetParameters(SceneObject sceneObject)
        {
            {
                var mapped = Manager.ID3D11DeviceContext.Map(MatrixBuffer, MapMode.WriteDiscard);
                ViewProjectionConstantBuffer = new() { Projection = Camera.ProjectionMatrix, View = Camera.ViewMatrix, World = Scene.WorldMatrix, CameraPosition = new Vector4(Camera.Position, 0) };
                DataStream stream = new(mapped.DataPointer, mapped.DepthPitch * mapped.RowPitch, false, true);
                stream.Write(ViewProjectionConstantBuffer);
                stream.Close();
                Manager.ID3D11DeviceContext.Unmap(MatrixBuffer);
            }
            {
                var mapped = Manager.ID3D11DeviceContext.Map(ModelBuffer, MapMode.WriteDiscard);
                ModelConstantBuffer = new() { ModelTransform = sceneObject.Transform };
                DataStream stream = new(mapped.DataPointer, mapped.DepthPitch * mapped.RowPitch, false, true);
                stream.Write(ModelConstantBuffer);
                stream.Close();
                Manager.ID3D11DeviceContext.Unmap(ModelBuffer);
            }
            {
                var mapped = Manager.ID3D11DeviceContext.Map(LightBuffer, MapMode.WriteDiscard);
                LightConstantBuffer = new() { Color = Light.DiffuseColour, Direction = Light.Direction, AmbientColor = Light.AmbientColor, SpecularColor = Light.SpecularColor, SpecularPower = Light.SpecularPower };
                DataStream stream = new(mapped.DataPointer, mapped.DepthPitch * mapped.RowPitch, false, true);
                stream.Write(LightConstantBuffer);
                stream.Close();
                Manager.ID3D11DeviceContext.Unmap(LightBuffer);
            }
            {
                var mapped = Manager.ID3D11DeviceContext.Map(TessellationBuffer, MapMode.WriteDiscard);
                TessellationBufferType = new() { TessellationAmount = TessellationAmount, Padding = Vector3.Zero };
                DataStream stream = new(mapped.DataPointer, mapped.DepthPitch * mapped.RowPitch, false, true);
                stream.Write(TessellationBufferType);
                stream.Close();
                Manager.ID3D11DeviceContext.Unmap(TessellationBuffer);
            }
        }

        public override void Render(SceneObject sceneObject)
        {
            SetParameters(sceneObject);
            Manager.ID3D11DeviceContext.VSSetConstantBuffer(0, MatrixBuffer);
            Manager.ID3D11DeviceContext.VSSetConstantBuffer(1, ModelBuffer);
            Manager.ID3D11DeviceContext.PSSetConstantBuffer(0, LightBuffer);
            Manager.ID3D11DeviceContext.DSSetConstantBuffer(0, MatrixBuffer);
            Manager.ID3D11DeviceContext.DSSetConstantBuffer(1, ModelBuffer);
            Manager.ID3D11DeviceContext.HSSetConstantBuffer(0, TessellationBuffer);

            Manager.ID3D11DeviceContext.IASetInputLayout(InputLayout);
            Manager.ID3D11DeviceContext.VSSetShader(VertexShader);
            Manager.ID3D11DeviceContext.HSSetShader(HullShader);
            Manager.ID3D11DeviceContext.DSSetShader(DomainShader);
            Manager.ID3D11DeviceContext.PSSetShader(PixelShader);

            Manager.ID3D11DeviceContext.PSSetSampler(0, SamplerState);
            Manager.ID3D11DeviceContext.DSSetSampler(0, SamplerState);

            Manager.ID3D11DeviceContext.PSSetShaderResources(0, sceneObject.ResourceViews);
            Manager.ID3D11DeviceContext.DSSetShaderResources(0, sceneObject.ResourceViews);
            Manager.ID3D11DeviceContext.IASetVertexBuffers(0, sceneObject.Model.VertexBufferView);
            Manager.ID3D11DeviceContext.IASetIndexBuffer(sceneObject.Model.IndexBuffer, Format.R32_UInt, 0);
            Manager.ID3D11DeviceContext.IASetPrimitiveTopology(PrimitiveTopology.PatchListWith3ControlPoints);
            Manager.ID3D11DeviceContext.DrawIndexed(sceneObject.Model.Indices.Length, 0, 0);
        }
    }
}