using HexaEngine.Core.Ressources;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;

namespace HexaEngine.Core.Render.Components
{
    public class DriectXManager : IDisposable
    {
        public DriectXManager(DeviceManager manager)
        {
            this.DeviceManager = manager ?? throw new ArgumentNullException(nameof(manager));
            this.SwapChain = this.DeviceManager.SwapChain;
            this.RenderForm = this.DeviceManager.Form;
            this.DefaultPixelFormat = this.DeviceManager.PixelFormat;
            this.Buffercount = this.DeviceManager.SwapChain.Description.BufferCount;
            this.D2DDeviceContext = new DeviceContext(manager.D2DDevice, DeviceContextOptions.EnableMultithreadedOptimizations);
            this.D3DDeviceContext = new SharpDX.Direct3D11.DeviceContext(manager.Device);
            this.TargetBitmapProperties = new BitmapProperties1(this.DefaultPixelFormat, this.RenderForm.DeviceDpi, this.RenderForm.DeviceDpi, BitmapOptions.Target | BitmapOptions.CannotDraw);
            this.DefaultBitmapProperties = new BitmapProperties1(this.DefaultPixelFormat, this.RenderForm.DeviceDpi, this.RenderForm.DeviceDpi, BitmapOptions.Target);
            this.SwapChainBackbuffer = Surface.FromSwapChain(this.SwapChain, 0);
            this.TargetBitmap = new Bitmap1(this.D2DDeviceContext, this.SwapChainBackbuffer, this.TargetBitmapProperties);
            this.ObjectsBitmap = new Bitmap1(this.D2DDeviceContext, new SharpDX.Size2(this.RenderForm.ClientSize.Width, this.RenderForm.ClientSize.Height), this.DefaultBitmapProperties);
            this.RayBitmap = new Bitmap1(this.D2DDeviceContext, new SharpDX.Size2(this.RenderForm.ClientSize.Width, this.RenderForm.ClientSize.Height), this.DefaultBitmapProperties);
        }

        ~DriectXManager()
        {
            this.Dispose(false);
        }

        public bool IsDisposed { get; private set; } = false;

        public DeviceManager DeviceManager { get; }

        public RenderForm RenderForm { get; private set; }

        public SwapChain1 SwapChain { get; private set; }

        public int Buffercount { get; private set; } = 2;

        public Bitmap1 TargetBitmap { get; private set; }

        public Bitmap1 RayBitmap { get; private set; }

        public Bitmap1 ObjectsBitmap { get; private set; }

        public PixelFormat DefaultPixelFormat { get; private set; }

        public BitmapProperties1 DefaultBitmapProperties { get; private set; }

        public Surface SwapChainBackbuffer { get; private set; }

        public DeviceContext D2DDeviceContext { get; private set; }

        public SharpDX.Direct3D11.DeviceContext D3DDeviceContext { get; private set; }

        private BitmapProperties1 TargetBitmapProperties { get; set; }

        public void Resize(float width, float height)
        {
            this.D2DDeviceContext.Target = null;
            this.SwapChainBackbuffer?.Dispose();
            this.TargetBitmap?.Dispose();
            this.ObjectsBitmap?.Dispose();

            foreach (Bitmap1 bitmap in RessouceManager.Bitmaps)
            {
                bitmap.Dispose();
            }

            this.SwapChain.ResizeBuffers(this.Buffercount, (int)width, (int)height, this.DefaultPixelFormat.Format, SwapChainFlags.AllowModeSwitch);

            this.SwapChainBackbuffer = Surface.FromSwapChain(this.SwapChain, 0);
            this.TargetBitmap = new Bitmap1(this.D2DDeviceContext, this.SwapChainBackbuffer, this.TargetBitmapProperties);
            this.ObjectsBitmap = new Bitmap1(this.D2DDeviceContext, new SharpDX.Size2(this.RenderForm.ClientSize.Width, this.RenderForm.ClientSize.Height), this.DefaultBitmapProperties);
            this.RayBitmap = new Bitmap1(this.D2DDeviceContext, new SharpDX.Size2(this.RenderForm.ClientSize.Width, this.RenderForm.ClientSize.Height), this.DefaultBitmapProperties);
        }

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
                    this.SwapChainBackbuffer.Dispose();
                    this.TargetBitmap.Dispose();
                    this.ObjectsBitmap.Dispose();
                    this.D2DDeviceContext.Dispose();
                }

                this.IsDisposed = true;
            }
        }
    }
}