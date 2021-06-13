using HexaFramework.Audio;
using PhysX;
using PhysX.VisualDebugger;
using System;
using Vortice.Direct2D1;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Debug;
using Vortice.DirectWrite;
using Vortice.DXGI;
using FactoryType = Vortice.DirectWrite.FactoryType;
using FillMode = Vortice.Direct3D11.FillMode;
using ResultCode = Vortice.DXGI.ResultCode;
using Usage = Vortice.DXGI.Usage;

namespace HexaFramework.Windows
{
    public class DeviceManager : System.IDisposable
    {
        private static readonly Vortice.Direct3D.FeatureLevel[] FeatureLevels =
        {
            Vortice.Direct3D.FeatureLevel.Level_12_1,
            Vortice.Direct3D.FeatureLevel.Level_12_0,
            Vortice.Direct3D.FeatureLevel.Level_11_1,
            Vortice.Direct3D.FeatureLevel.Level_11_0,
            Vortice.Direct3D.FeatureLevel.Level_10_1,
            Vortice.Direct3D.FeatureLevel.Level_10_0
        };

        private readonly Vortice.Direct3D.FeatureLevel _featureLevel;
        private readonly IDXGIFactory2 _idxgiFactory;

        public DeviceManager(RenderWindow window)
        {
            Window = window;
            SurfaceWidth = window.Width;
            SurfaceHeight = window.Height;
            AspectRatio = (float)SurfaceWidth / SurfaceHeight;
            var swapChainDescription = new SwapChainDescription1
            {
                Width = window.Width,
                Height = window.Height,
                Format = Format.B8G8R8A8_UNorm,
                BufferCount = BufferCount,
                Usage = Usage.RenderTargetOutput,
                SampleDescription = new SampleDescription(2, 0),
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.Discard,
                Flags = SwapChainFlags.AllowModeSwitch
            };

            var fullscreenDescription = new SwapChainFullscreenDescription
            {
                Windowed = true,
                RefreshRate = new Rational(0, 1),
                Scaling = ModeScaling.Unspecified,
                ScanlineOrdering = ModeScanlineOrder.Unspecified
            };

            DXGI.CreateDXGIFactory1(out _idxgiFactory);
            var adapter = GetHardwareAdapter();
            D3D11.D3D11CreateDevice(adapter, DriverType.Unknown, DeviceCreationFlags.BgraSupport, FeatureLevels, out var tempDevice, out _featureLevel, out var tempContext);
            ID3D11Device = tempDevice.QueryInterface<ID3D11Device1>();
            ID3D11DeviceContext = tempContext.QueryInterface<ID3D11DeviceContext1>();
            tempContext.Dispose();
            tempDevice.Dispose();
            SwapChain = IDXGIFactory.CreateSwapChainForHwnd(ID3D11Device, window.Handle, swapChainDescription, fullscreenDescription);
            IDXGIFactory.MakeWindowAssociation(window.Handle, WindowAssociationFlags.IgnoreAll);
            IDXGIDevice = ID3D11Device.QueryInterface<IDXGIDevice>();
            BackBuffer = SwapChain.GetBuffer<ID3D11Texture2D>(0);
            RenderTargetView = ID3D11Device.CreateRenderTargetView(BackBuffer);
            BackBuffer.Dispose();

            Texture2DDescription depthBufferDesc = new()
            {
                Width = SurfaceWidth,
                Height = SurfaceHeight,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D32_Float_S8X24_UInt,
                SampleDescription = new SampleDescription(2, 0),
                Usage = Vortice.Direct3D11.Usage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            DepthStencilBuffer = ID3D11Device.CreateTexture2D(depthBufferDesc);

            DepthStencilDescription depthStencilDesc = new()
            {
                DepthEnable = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthFunc = ComparisonFunction.LessEqual,
                StencilEnable = true,

                // Stencil operation if pixel front-facing.
                FrontFace = new DepthStencilOperationDescription()
                {
                    StencilFailOp = StencilOperation.Keep,
                    StencilDepthFailOp = StencilOperation.Incr,
                    StencilPassOp = StencilOperation.Keep,
                    StencilFunc = ComparisonFunction.Always
                },
                // Stencil operation if pixel is back-facing.
                BackFace = new DepthStencilOperationDescription()
                {
                    StencilFailOp = StencilOperation.Keep,
                    StencilDepthFailOp = StencilOperation.Incr,
                    StencilPassOp = StencilOperation.Keep,
                    StencilFunc = ComparisonFunction.Always
                }
            };

            DepthStencilState = ID3D11Device.CreateDepthStencilState(depthStencilDesc);
            ID3D11DeviceContext.OMSetDepthStencilState(DepthStencilState, 1);

            DepthStencilViewDescription depthStencilViewDesc = new()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ViewDimension = DepthStencilViewDimension.Texture2DMultisampled,
                Texture2D = new Texture2DDepthStencilView() { MipSlice = 0 }
            };

            DepthStencilView = ID3D11Device.CreateDepthStencilView(DepthStencilBuffer, depthStencilViewDesc);
            ID3D11DeviceContext.OMSetRenderTargets(RenderTargetView, DepthStencilView);

            RasterizerDescription rasterDesc = new()
            {
                AntialiasedLineEnable = true,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = .0f,
                DepthClipEnable = true,
                FillMode = FillMode.Solid,
                FrontCounterClockwise = false,
                MultisampleEnable = true,
                ScissorEnable = false,
                SlopeScaledDepthBias = .0f
            };

            RasterizerDescription rasterDesc1 = new()
            {
                AntialiasedLineEnable = true,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = .0f,
                DepthClipEnable = true,
                FillMode = FillMode.Wireframe,
                FrontCounterClockwise = false,
                MultisampleEnable = true,
                ScissorEnable = false,
                SlopeScaledDepthBias = .0f
            };

            RasterStateSolid = ID3D11Device.CreateRasterizerState(rasterDesc);
            RasterStateWireframe = ID3D11Device.CreateRasterizerState(rasterDesc1);
            ID3D11DeviceContext.RSSetState(RasterStateSolid);

            // DX 2D

            DWrite.DWriteCreateFactory(FactoryType.Shared, out _iDWriteFactory);
            DefaultTextFormat = IDWriteFactory.CreateTextFormat("Arial", FontWeight.Normal, FontStyle.Normal,
                FontStretch.Normal, 12);

            var creationOptions = new CreationProperties
            {
                DebugLevel = DebugLevel.None,
                Options = DeviceContextOptions.EnableMultithreadedOptimizations,
                ThreadingMode = ThreadingMode.MultiThreaded
            };

            _id2D1Device = D2D1.D2D1CreateDevice(IDXGIDevice, creationOptions);
            ID2D1DeviceContext = ID2D1Device.CreateDeviceContext(DeviceContextOptions.EnableMultithreadedOptimizations);
            ID2D1DeviceContext.AntialiasMode = AntialiasMode.PerPrimitive;

            Foundation = new Foundation();
            Pvd = new Pvd(Foundation);
            Physics = new Physics(Foundation, true, Pvd);
            Physics.Pvd.Connect("localhost");
        }

        private readonly ID2D1Device _id2D1Device;
        private readonly IDWriteFactory _iDWriteFactory;
        private bool disposedValue;

        public RenderWindow Window { get; }

        public AudioManager AudioManager { get; } = new();

        public Foundation Foundation { get; set; }

        public Pvd Pvd { get; set; }

        public Physics Physics { get; set; }

        public ID2D1Device ID2D1Device => _id2D1Device;

        public ID2D1DeviceContext ID2D1DeviceContext { get; private set; }

        public IDWriteFactory IDWriteFactory => _iDWriteFactory;

        public IDWriteTextFormat DefaultTextFormat { get; private set; }

        public int SurfaceWidth { get; private set; }

        public int SurfaceHeight { get; private set; }

        public IDXGIFactory2 IDXGIFactory => _idxgiFactory;

        public Vortice.Direct3D.FeatureLevel FeatureLevel => _featureLevel;

        public ID3D11DeviceContext1 ID3D11DeviceContext { get; private set; }

        public IDXGISwapChain SwapChain { get; private set; }

        public ID3D11RenderTargetView RenderTargetView { get; private set; }

        public IDXGIDevice IDXGIDevice { get; private set; }

        public ID3D11Device1 ID3D11Device { get; private set; }

        public ID3D11Texture2D BackBuffer { get; private set; }

        public ID3D11Texture2D DepthStencilBuffer { get; private set; }

        public ID3D11DepthStencilState DepthStencilState { get; }

        public ID3D11DepthStencilView DepthStencilView { get; private set; }

        public ID3D11RasterizerState RasterStateSolid { get; }

        public ID3D11RasterizerState RasterStateWireframe { get; }

        public ID3D11Debug ID3D11Debug { get; }

        public int BufferCount { get; set; } = 1;

        public float AspectRatio { get; private set; }

        public event EventHandler OnBufferResize;

        public void Resize(int width, int height)
        {
            // Buffering values for reasons.
            SurfaceWidth = width;
            SurfaceHeight = height;
            AspectRatio = (float)SurfaceWidth / SurfaceHeight;

            // Delete all references to SwapChainBuffers.
            ID3D11DeviceContext.OMSetDepthStencilState(null);
            ID3D11DeviceContext.OMSetRenderTargets((ID3D11RenderTargetView)null, null);
            ID3D11DeviceContext.ClearState();
            ID3D11DeviceContext.Flush();
            DepthStencilView.Dispose();
            RenderTargetView.Dispose();

            // Releasing ID2D1 references.
            ID2D1DeviceContext.Target = null;

            // Resize Targets and SwapChainBuffers.
            SwapChain.ResizeTarget(new ModeDescription(SurfaceWidth, SurfaceHeight));
            SwapChain.ResizeBuffers(BufferCount, SurfaceWidth, SurfaceHeight, Format.B8G8R8A8_UNorm, SwapChainFlags.AllowModeSwitch);

            // Recreate SwapChainBuffer all references.
            BackBuffer = SwapChain.GetBuffer<ID3D11Texture2D>(0);
            RenderTargetView = ID3D11Device.CreateRenderTargetView(BackBuffer);
            BackBuffer.Dispose();

            Texture2DDescription depthBufferDesc = new()
            {
                Width = SurfaceWidth,
                Height = SurfaceHeight,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D32_Float_S8X24_UInt,
                SampleDescription = new SampleDescription(2, 0),
                Usage = Vortice.Direct3D11.Usage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            DepthStencilBuffer?.Dispose();
            DepthStencilBuffer = ID3D11Device.CreateTexture2D(depthBufferDesc);

            DepthStencilViewDescription depthStencilViewDesc = new()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ViewDimension = DepthStencilViewDimension.Texture2DMultisampled,
                Texture2D = new Texture2DDepthStencilView() { MipSlice = 0 }
            };

            ID3D11DeviceContext.OMSetDepthStencilState(DepthStencilState, 1);

            DepthStencilView = ID3D11Device.CreateDepthStencilView(DepthStencilBuffer, depthStencilViewDesc);

            ID3D11DeviceContext.OMSetRenderTargets(RenderTargetView, DepthStencilView);

            ID3D11DeviceContext.RSSetState(RasterStateSolid);

            OnBufferResize?.Invoke(this, null);
        }

        public void SwitchWireframe(bool state)
        {
            if (state)
            {
                ID3D11DeviceContext.RSSetState(RasterStateWireframe);
            }
            else
            {
                ID3D11DeviceContext.RSSetState(RasterStateSolid);
            }
        }

        private IDXGIAdapter1 GetHardwareAdapter()
        {
            IDXGIAdapter1 adapter = null;
            var factory6 = IDXGIFactory.QueryInterfaceOrNull<IDXGIFactory6>();
            if (factory6 != null)
            {
                for (var adapterIndex = 0;
                    factory6.EnumAdapterByGpuPreference(adapterIndex, GpuPreference.HighPerformance, out adapter) !=
                    ResultCode.NotFound;
                    adapterIndex++)
                {
                    var desc = adapter.Description1;

                    if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter.Dispose();
                        continue;
                    }

                    return adapter;
                }

                factory6.Dispose();
            }

            if (adapter == null)
                for (var adapterIndex = 0;
                    IDXGIFactory.EnumAdapters1(adapterIndex, out adapter) != ResultCode.NotFound;
                    adapterIndex++)
                {
                    var desc = adapter.Description1;

                    if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter.Dispose();
                        continue;
                    }

                    return adapter;
                }

            return adapter;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                ID2D1DeviceContext.Dispose();
                DefaultTextFormat.Dispose();
                IDWriteFactory.Dispose();

                RenderTargetView.Dispose();
                BackBuffer.Dispose();
                ID3D11DeviceContext.ClearState();
                ID3D11DeviceContext.Flush();
                ID3D11DeviceContext.Dispose();
                ID3D11Device.Dispose();
                SwapChain.Dispose();
                IDXGIFactory.Dispose();

                GC.Collect(2, GCCollectionMode.Forced);

                disposedValue = true;
            }
        }

        ~DeviceManager()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}