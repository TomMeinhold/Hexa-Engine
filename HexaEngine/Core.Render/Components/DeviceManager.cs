// <copyright file="DeviceManager.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Render.Components
{
    using HexaEngine.Core.Windows;
    using System;
    using D2D1 = SharpDX.Direct2D1;
    using D3D = SharpDX.Direct3D;
    using D3D11 = SharpDX.Direct3D11;
    using DXGI = SharpDX.DXGI;

    public class DeviceManager : IDisposable
    {
        public DeviceManager(IRenderable renderable, D3D.FeatureLevel[] featureLevels, int buffercount, D2D1.PixelFormat pixelFormat)
        {
            Renderable = renderable ?? throw new ArgumentNullException(nameof(renderable));
            Buffercount = buffercount;
            PixelFormat = pixelFormat;

            Device = new D3D11.Device(D3D.DriverType.Hardware, D3D11.DeviceCreationFlags.BgraSupport, featureLevels);
            Device1 = Device.QueryInterfaceOrNull<D3D11.Device1>() ?? throw new NotSupportedException();
            FeatureLevel = Device.FeatureLevel;

            using var dxgi = Device1.QueryInterface<DXGI.Device2>();
            using var adapter = dxgi.Adapter;
            using var factory = adapter.GetParent<DXGI.Factory2>();
            D2DDevice = new D2D1.Device(dxgi);

            var desc1 = new DXGI.SwapChainDescription1()
            {
                SampleDescription = new DXGI.SampleDescription(1, 0),
                Width = Renderable.ClientSize.Width,
                Height = Renderable.ClientSize.Height,
                SwapEffect = DXGI.SwapEffect.Discard,
                Usage = DXGI.Usage.RenderTargetOutput,
                Flags = DXGI.SwapChainFlags.AllowModeSwitch,
                Scaling = DXGI.Scaling.Stretch,
                Format = PixelFormat.Format,
                BufferCount = Buffercount,
                Stereo = false,
            };

            var descFullSc = new DXGI.SwapChainFullScreenDescription()
            {
                RefreshRate = new DXGI.Rational(60, 1),
                Scaling = DXGI.DisplayModeScaling.Stretched,
                Windowed = true,
            };

            SwapChain = new DXGI.SwapChain1(factory, Device1, Renderable.Handle, ref desc1, descFullSc, null);
        }

        ~DeviceManager()
        {
            Dispose(false);
        }

        public bool IsDisposed { get; private set; } = false;

        public D3D11.Device Device { get; private set; }

        public D3D11.Device1 Device1 { get; private set; }

        public D2D1.Device D2DDevice { get; set; }

        public DXGI.SwapChain1 SwapChain { get; private set; }

        public int Buffercount { get; private set; }

        public D2D1.PixelFormat PixelFormat { get; private set; }

        public IRenderable Renderable { get; }

        public D3D.FeatureLevel FeatureLevel { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Device.Dispose();
                    Device1.Dispose();
                    D2DDevice.Dispose();
                    SwapChain.Dispose();
                }

                IsDisposed = true;
            }
        }
    }
}