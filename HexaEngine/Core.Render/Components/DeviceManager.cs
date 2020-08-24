// <copyright file="DeviceManager.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Render.Components
{
    using System;
    using D2D1 = SharpDX.Direct2D1;
    using D3D = SharpDX.Direct3D;
    using D3D11 = SharpDX.Direct3D11;
    using DXGI = SharpDX.DXGI;
    using Win = SharpDX.Windows;

    public class DeviceManager : IDisposable
    {
        public DeviceManager(Win.RenderForm form, D3D.FeatureLevel[] featureLevels, int buffercount, D2D1.PixelFormat pixelFormat)
        {
            this.Form = form ?? throw new ArgumentNullException(nameof(form));
            this.Buffercount = buffercount;
            this.PixelFormat = pixelFormat;

            this.Device = new D3D11.Device(D3D.DriverType.Hardware, D3D11.DeviceCreationFlags.BgraSupport, featureLevels);
            this.Device1 = this.Device.QueryInterfaceOrNull<D3D11.Device1>() ?? throw new NotSupportedException();
            this.FeatureLevel = this.Device.FeatureLevel;

            using var dxgi = this.Device1.QueryInterface<DXGI.Device2>();
            using var adapter = dxgi.Adapter;
            using var factory = adapter.GetParent<DXGI.Factory2>();
            this.D2DDevice = new D2D1.Device(dxgi);

            var desc1 = new DXGI.SwapChainDescription1()
            {
                SampleDescription = new DXGI.SampleDescription(1, 0),
                Width = this.Form.ClientSize.Width,
                Height = this.Form.ClientSize.Height,
                SwapEffect = DXGI.SwapEffect.Discard,
                Usage = DXGI.Usage.RenderTargetOutput,
                Flags = DXGI.SwapChainFlags.AllowModeSwitch,
                Scaling = DXGI.Scaling.Stretch,
                Format = this.PixelFormat.Format,
                BufferCount = this.Buffercount,
                Stereo = false,
            };

            var descFullSc = new DXGI.SwapChainFullScreenDescription()
            {
                RefreshRate = new DXGI.Rational(60, 1),
                Scaling = DXGI.DisplayModeScaling.Stretched,
                Windowed = true,
            };

            this.SwapChain = new DXGI.SwapChain1(factory, this.Device1, this.Form.Handle, ref desc1, descFullSc, null);
        }

        ~DeviceManager()
        {
            this.Dispose(false);
        }

        public bool IsDisposed { get; private set; } = false;

        public D3D11.Device Device { get; private set; }

        public D3D11.Device1 Device1 { get; private set; }

        public D2D1.Device D2DDevice { get; set; }

        public DXGI.SwapChain1 SwapChain { get; private set; }

        public int Buffercount { get; private set; }

        public D2D1.PixelFormat PixelFormat { get; private set; }

        public Win.RenderForm Form { get; }

        public D3D.FeatureLevel FeatureLevel { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.Device.Dispose();
                    this.Device1.Dispose();
                    this.D2DDevice.Dispose();
                    this.SwapChain.Dispose();
                }

                this.IsDisposed = true;
            }
        }
    }
}