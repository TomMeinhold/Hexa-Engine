using HexaFramework.Resources;
using HexaFramework.Scenes;
using HexaFramework.Windows.Native;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Vortice.Direct3D11;
using Vortice.DXGI;
using static HexaFramework.Windows.Native.Helper;

namespace HexaFramework.Windows
{
    public abstract partial class RenderWindow : NativeWindow, IDisposable
    {
        private Thread renderThread;
        private bool disposedValue;
        private bool disposing;
        private bool first;

        public bool Focus { get; set; }

        public bool Fullscreen { get; set; } = false;

        public bool Resizeable { get; set; } = true;

        public Color BackgroundClear { get; set; } = Color.White;

        public DeviceManager DeviceManager { get; private set; }

        public ResourceManager ResourceManager { get; private set; }

        public Scene Scene { get; set; }

        public Time Time { get; set; } = new Time();

        public RenderWindow(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
            Style = WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;
            StyleEx = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;
        }

        ~RenderWindow()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: false);
        }

        protected override void OnHandleCreated()
        {
            DeviceManager = new DeviceManager(this);
            ResourceManager = new ResourceManager(DeviceManager);
            Scene = new(this);
            renderThread = new Thread(TickInternal);
            renderThread.Start();
        }

        protected override IntPtr ProcessWindowMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == (uint)WindowMessage.ActivateApp)
            {
                Focus = IntPtrToInt32(wParam) != 0;
                if (Focus)
                {
                    OnActivated();
                }
                else
                {
                    OnDeactivated();
                }

                return base.ProcessWindowMessage(hWnd, msg, wParam, lParam);
            }

            Cursor?.Tick();
            return base.ProcessWindowMessage(hWnd, msg, wParam, lParam);
        }

        public DialogResult ShowMessageBox(string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            uint style = (uint)((int)buttons | (int)icon | 0 | 0);
            return (DialogResult)User32.MessageBox(Handle, title, text, style);
        }

        public event EventHandler<EventArgs> Initialized;

        public event EventHandler<EventArgs> Uninitialized;

        public bool IsInitialized { get; set; }

        protected abstract void InitializeComponent();

        protected virtual void BeginRender()
        {
            DeviceManager.ID3D11DeviceContext.ClearDepthStencilView(DeviceManager.DepthStencilView, DepthStencilClearFlags.Depth, 1, 0);
            DeviceManager.ID3D11DeviceContext.ClearRenderTargetView(DeviceManager.RenderTargetView, BackgroundClear);
            DeviceManager.ID3D11DeviceContext.RSSetViewport(0, 0, Width, Height, 0, 1);
        }

        protected virtual void Render()
        {
            Scene.Render();
        }

        protected virtual void EndRender()
        {
            DeviceManager.SwapChain.Present(0, PresentFlags.None);
        }

        internal void TickInternal()
        {
            while (!disposing)
            {
                if (!first)
                {
                    Time.Initialize();
                    Initialized?.Invoke(this, null);
                    first = true;
                    InitializeComponent();
                }

                Time.FrameUpdate();
                BeginRender();
                Render();
                EndRender();
                LimitFrameRate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                this.disposing = true;
                while (renderThread.IsAlive) Thread.Sleep(1);
                Uninitialized?.Invoke(this, null);
                DeviceManager.Dispose();
                Close();
                disposedValue = true;
            }
        }

        public bool LimitFPS;
        public int FPSTarget = 60;
        private long fpsFrameCount;
        private long fpsStartTime;

        private void LimitFrameRate()
        {
            if (LimitFPS)
            {
                int fps = FPSTarget;
                long freq = Stopwatch.Frequency;
                long frame = Stopwatch.GetTimestamp();
                while ((frame - fpsStartTime) * fps < freq * fpsFrameCount)
                {
                    int sleepTime = (int)(((fpsStartTime * fps) + (freq * fpsFrameCount) - (frame * fps)) * 1000 / (freq * fps));
                    if (sleepTime > 0) Thread.Sleep(sleepTime);
                    frame = Stopwatch.GetTimestamp();
                }
                if (++fpsFrameCount > fps)
                {
                    fpsFrameCount = 0;
                    fpsStartTime = frame;
                }
            }
        }
    }
}