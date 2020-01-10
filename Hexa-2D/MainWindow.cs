using HexaEngine.Core;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
using SharpDX.DXGI;
using SharpDX.RawInput;
using SharpDX.Windows;
using System.Threading;
using System.Windows.Forms;
using Timer = HexaEngine.Core.Common.Timer;

namespace Game
{
    public partial class MainWindow : RenderForm
    {
        readonly Engine Engine = new Engine();
        readonly Timer Timer = new Timer(500, true);

        public MainWindow()
        {

            IsFullscreen = Properties.Settings.Default.Fullscreen;
            ClientSize = new System.Drawing.Size(Properties.Settings.Default.Width, Properties.Settings.Default.Height);
            InitializeComponent();
            InitializeDirectX();
            Engine.RawInput.InitializeDirectRawInput();
            Engine.Initial(D2DRenderTarget);
            InitializeInputEventHandler();
            GameAssets.Assets.LoadAssets(Engine);
            Timer.TimerTick += TimerWorker;
            MainLoop();
        }

        private void MainLoop()
        {
            // Main loop
            RenderLoop.Run(this, () =>
            {
                if (!LockRenderTarget)
                {

                    LockModifys = true;

                    D2DRenderTarget.BeginDraw();
                    D2DRenderTarget.Clear(Color.White);
                    Engine.ObjectSystem.RenderObjects();
                    D2DRenderTarget.EndDraw();

                    SwapChain.Present(0, PresentFlags.None);



                    LockModifys = false;
                }
            });
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            Timer.Dispose();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                LockRenderTarget = true;
                Settings settings = new Settings();
                settings.ShowDialog();
                settings.Dispose();
                ClientSize = new System.Drawing.Size(Properties.Settings.Default.Width, Properties.Settings.Default.Height);
                IsFullscreen = Properties.Settings.Default.Fullscreen;
                ReloadDirectX();
                LockRenderTarget = false;
            }
            if (e.KeyCode == Keys.F4)
            {
                Engine.DebugWindow.Show();
            }
            if (e.KeyCode == Keys.F11)
            {
                if (Properties.Settings.Default.Fullscreen)
                {
                    SwapChain.SetFullscreenState(false, null);
                    Properties.Settings.Default.Fullscreen = false;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    SwapChain.SetFullscreenState(true, null);
                    Properties.Settings.Default.Fullscreen = true;
                    Properties.Settings.Default.Save();
                }

            }
        }

        bool Wp, Ap, Sp, Dp;

        bool w;

        private void TimerWorker(object sender, Timer.TimerEventArgs e)
        {
            if (w) { w = !w; }
            while (!w) { Thread.Sleep(1); }
        }

        private void KeyboardEvent(object sender, KeyboardInputEventArgs args)
        {
            if (args.Key == Keys.W && args.ScanCodeFlags == ScanCodeFlags.Break)
            {
                Wp = false;
            }
            if (args.Key == Keys.W && args.ScanCodeFlags == ScanCodeFlags.Make)
            {
                Wp = true;
            }
            if (args.Key == Keys.A && args.ScanCodeFlags == ScanCodeFlags.Break)
            {
                Ap = false;
            }
            if (args.Key == Keys.A && args.ScanCodeFlags == ScanCodeFlags.Make)
            {
                Ap = true;
            }
            if (args.Key == Keys.S && args.ScanCodeFlags == ScanCodeFlags.Break)
            {
                Sp = false;
            }
            if (args.Key == Keys.S && args.ScanCodeFlags == ScanCodeFlags.Make)
            {
                Sp = true;
            }
            if (args.Key == Keys.D && args.ScanCodeFlags == ScanCodeFlags.Break)
            {
                Dp = false;
            }
            if (args.Key == Keys.D && args.ScanCodeFlags == ScanCodeFlags.Make)
            {
                Dp = true;
            }
            if (Ap && Wp)
            {
                Engine.ObjectSystem.Player.MovementAcceleration.X = -5;
                Engine.ObjectSystem.Player.MovementAcceleration.Y = -5;
            }
            else
            {
                if (Ap) { Engine.ObjectSystem.Player.MovementAcceleration.X = -5; }
            }
            if (Wp && Dp)
            {
                Engine.ObjectSystem.Player.MovementAcceleration.Y = -5;
                Engine.ObjectSystem.Player.MovementAcceleration.X = 5;
            }
            else
            {
                if (Wp) { Engine.ObjectSystem.Player.MovementAcceleration.Y = 5; }
            }
            if (Dp && Sp)
            {
                Engine.ObjectSystem.Player.MovementAcceleration.X = 5;
                Engine.ObjectSystem.Player.MovementAcceleration.Y = 5;
            }
            else
            {
                if (Dp) { Engine.ObjectSystem.Player.MovementAcceleration.X = 5; }
            }
            if (Sp && Ap)
            {
                Engine.ObjectSystem.Player.MovementAcceleration.Y = 5;
                Engine.ObjectSystem.Player.MovementAcceleration.X = -5;
            }
            else
            {
                if (Sp) { Engine.ObjectSystem.Player.MovementAcceleration.Y = -5; }
            }
            if (args.Key == Keys.Space && args.ScanCodeFlags == ScanCodeFlags.Make)
            {
                if (!w)
                {
                    Engine.ObjectSystem.Player.MovementAcceleration.Y = 5;
                    w = true;
                }
            }
            if (args.Key == Keys.R && args.ScanCodeFlags == ScanCodeFlags.Make)
            {
                Engine.ObjectSystem.Player.Respawn();
            }
        }

        private void MouseEvent(object sender, MouseInputEventArgs args)
        {

        }
    }
}