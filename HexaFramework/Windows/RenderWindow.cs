using HexaFramework.NvPhysX;
using HexaFramework.Resources;
using HexaFramework.Scenes;
using HexaFramework.Windows.Native;
using PhysX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Threading;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using static HexaFramework.Windows.Native.Helper;

namespace HexaFramework.Windows
{
    public abstract partial class RenderWindow : NativeWindow, System.IDisposable
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

        public IReadOnlyList<Camera> Cameras => cameras;

        public Camera Add(Camera camera)
        {
            camera.AttachWindow(this);
            camera.Script?.Initialize();
            cameras.Add(camera);
            return camera;
        }

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
            Scene = DeviceManager.Physics.CreateScene(CreateSceneDesc(DeviceManager.Foundation));
            Scene.SetVisualizationParameter(VisualizationParameter.Scale, 2.0f);
            Scene.SetVisualizationParameter(VisualizationParameter.CollisionShapes, true);
            Scene.SetVisualizationParameter(VisualizationParameter.JointLocalFrames, true);
            Scene.SetVisualizationParameter(VisualizationParameter.JointLimits, true);
            Scene.SetVisualizationParameter(VisualizationParameter.ActorAxes, true);
            renderThread = new Thread(TickInternal);
            renderThread.Start();
        }

        protected virtual SceneDesc CreateSceneDesc(Foundation foundation)
        {
#if GPU
            var cudaContext = new CudaContextManager(foundation);
#endif

            var sceneDesc = new SceneDesc
            {
                Gravity = new Vector3(0, -9.81f, 0),

#if GPU
                CudaContextManager = cudaContext,
#endif
                Flags = SceneFlag.EnableCcd,
                FilterShader = new FilterShader()
            };

#if GPU
            sceneDesc.Flags |= SceneFlag.EnableGpuDynamics;
            sceneDesc.BroadPhaseType |= BroadPhaseType.Gpu;
#endif

            return sceneDesc;
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

            if (msg == 0x0232)
            {
                DeviceManager.Resize(Width, Height);
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

        public IDebugShader DebugShader { get; set; }

        protected abstract void InitializeComponent();

        protected virtual void BeginRender()
        {
            DeviceManager.ID3D11DeviceContext.ClearDepthStencilView(DeviceManager.DepthStencilView, DepthStencilClearFlags.Depth, 1, 0);
            DeviceManager.ID3D11DeviceContext.ClearRenderTargetView(DeviceManager.RenderTargetView, BackgroundClear);
            DeviceManager.ID3D11DeviceContext.RSSetViewport(0, 0, Width, Height, 0, 1);
        }

        protected virtual void Render()
        {
            cameras.ForEach(x => x.Script?.Update());
            cameras.ForEach(x => x?.UpdateView());
            Scene.Simulate(Time.Delta);
            Scene.FetchResults(block: true);
            var actors = Scene.GetActors(ActorTypeFlag.RigidDynamic | ActorTypeFlag.RigidStatic);
            foreach (var actor in actors)
            {
                if (actor.UserData is SceneObject sceneObject)
                {
                    sceneObject.Transform = (actor as RigidActor).GlobalPose;
                    sceneObject.Render();
                }
            }
            if (DebugShader is not null)
                DrawDebug(Scene.GetRenderBuffer());
        }

        protected virtual void EndRender()
        {
            DeviceManager.SwapChain.Present(0, PresentFlags.None);
        }

        protected virtual void DrawDebug(RenderBuffer data)
        {
            if (data.NumberOfPoints > 0)
            {
                var vertices = new VertexPositionColor[data.Points.Length];
                for (int i = 0; i < data.Points.Length; i++)
                {
                    var point = data.Points[i];

                    vertices[i * 2 + 0] = new VertexPositionColor(point.Point, Color.FromArgb(point.Color));
                }

                DebugShader.Render(vertices, PrimitiveTopology.PointList);
            }

            if (data.NumberOfLines > 0)
            {
                var vertices = new VertexPositionColor[data.Lines.Length * 2];
                for (int x = 0; x < data.Lines.Length; x++)
                {
                    DebugLine line = data.Lines[x];

                    vertices[x * 2 + 0] = new VertexPositionColor(line.Point0, Color.FromArgb(line.Color0));
                    vertices[x * 2 + 1] = new VertexPositionColor(line.Point1, Color.FromArgb(line.Color1));
                }

                DebugShader.Render(vertices, PrimitiveTopology.LineList);
            }

            if (data.NumberOfTriangles > 0)
            {
                var vertices = new VertexPositionColor[data.Triangles.Length * 3];
                for (int x = 0; x < data.Triangles.Length; x++)
                {
                    DebugTriangle triangle = data.Triangles[x];

                    vertices[x * 3 + 0] = new VertexPositionColor(triangle.Point0, Color.FromArgb(triangle.Color0));
                    vertices[x * 3 + 1] = new VertexPositionColor(triangle.Point1, Color.FromArgb(triangle.Color1));
                    vertices[x * 3 + 2] = new VertexPositionColor(triangle.Point2, Color.FromArgb(triangle.Color2));
                }

                DebugShader.Render(vertices, PrimitiveTopology.TriangleList);
            }
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
                    cameras.ForEach(x => x.Script?.Initialize());
                }

                Time.FrameUpdate();
                BeginRender();
                Render();
                EndRender();
                Mouse.ClearDelta();
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
        private readonly List<Camera> cameras = new();

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