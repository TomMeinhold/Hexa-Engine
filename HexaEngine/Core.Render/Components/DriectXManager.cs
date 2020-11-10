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
            DeviceManager = manager ?? throw new ArgumentNullException(nameof(manager));
            SwapChain = DeviceManager.SwapChain;
            RenderForm = DeviceManager.Form;
            DefaultPixelFormat = DeviceManager.PixelFormat;
            Buffercount = DeviceManager.SwapChain.Description.BufferCount;
            D2DDeviceContext = new DeviceContext(manager.D2DDevice, DeviceContextOptions.EnableMultithreadedOptimizations);
            D3DDeviceContext = new SharpDX.Direct3D11.DeviceContext(manager.Device);
            TargetBitmapProperties = new BitmapProperties1(DefaultPixelFormat, RenderForm.DeviceDpi, RenderForm.DeviceDpi, BitmapOptions.Target | BitmapOptions.CannotDraw);
            DefaultBitmapProperties = new BitmapProperties1(DefaultPixelFormat, RenderForm.DeviceDpi, RenderForm.DeviceDpi, BitmapOptions.Target);
            SwapChainBackbuffer = Surface.FromSwapChain(SwapChain, 0);
            TargetBitmap = new Bitmap1(D2DDeviceContext, SwapChainBackbuffer, TargetBitmapProperties);
            ObjectsBitmap = new Bitmap1(D2DDeviceContext, new SharpDX.Size2(RenderForm.ClientSize.Width, RenderForm.ClientSize.Height), DefaultBitmapProperties);
            RayBitmap = new Bitmap1(D2DDeviceContext, new SharpDX.Size2(RenderForm.ClientSize.Width, RenderForm.ClientSize.Height), DefaultBitmapProperties);
            ShadowMaskBitmap = new Bitmap1(D2DDeviceContext, new SharpDX.Size2(RenderForm.ClientSize.Width, RenderForm.ClientSize.Height), DefaultBitmapProperties);
        }

        ~DriectXManager()
        {
            Dispose(false);
        }

        public bool IsDisposed { get; private set; } = false;

        public DeviceManager DeviceManager { get; }

        public RenderForm RenderForm { get; private set; }

        public SwapChain1 SwapChain { get; private set; }

        public int Buffercount { get; private set; } = 2;

        public Bitmap1 TargetBitmap { get; private set; }

        public Bitmap1 RayBitmap { get; private set; }

        public Bitmap1 ShadowMaskBitmap { get; private set; }

        public Bitmap1 ObjectsBitmap { get; private set; }

        public PixelFormat DefaultPixelFormat { get; private set; }

        public BitmapProperties1 DefaultBitmapProperties { get; private set; }

        public Surface SwapChainBackbuffer { get; private set; }

        public DeviceContext D2DDeviceContext { get; private set; }

        public SharpDX.Direct3D11.DeviceContext D3DDeviceContext { get; private set; }

        private BitmapProperties1 TargetBitmapProperties { get; set; }

        public void Resize(float width, float height)
        {
            D2DDeviceContext.Target = null;
            SwapChainBackbuffer?.Dispose();
            TargetBitmap?.Dispose();
            ObjectsBitmap?.Dispose();

            foreach (Bitmap1 bitmap in RessourceManager.Bitmaps)
            {
                bitmap.Dispose();
            }

            SwapChain.ResizeBuffers(Buffercount, (int)width, (int)height, DefaultPixelFormat.Format, SwapChainFlags.AllowModeSwitch);

            SwapChainBackbuffer = Surface.FromSwapChain(SwapChain, 0);
            TargetBitmap = new Bitmap1(D2DDeviceContext, SwapChainBackbuffer, TargetBitmapProperties);
            ObjectsBitmap = new Bitmap1(D2DDeviceContext, new SharpDX.Size2(RenderForm.ClientSize.Width, RenderForm.ClientSize.Height), DefaultBitmapProperties);
            RayBitmap = new Bitmap1(D2DDeviceContext, new SharpDX.Size2(RenderForm.ClientSize.Width, RenderForm.ClientSize.Height), DefaultBitmapProperties);
            ShadowMaskBitmap = new Bitmap1(D2DDeviceContext, new SharpDX.Size2(RenderForm.ClientSize.Width, RenderForm.ClientSize.Height), DefaultBitmapProperties);
        }

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
                    SwapChainBackbuffer.Dispose();
                    TargetBitmap.Dispose();
                    ObjectsBitmap.Dispose();
                    D2DDeviceContext.Dispose();
                }

                IsDisposed = true;
            }
        }
    }
}