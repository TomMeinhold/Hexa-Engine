using HexaEngine.Core;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.RawInput;
using SharpDX.Windows;
using System.Windows.Forms;

namespace Game
{
    public partial class MainWindow : RenderForm
    {
        readonly Engine Engine = new Engine();

        public MainWindow()
        {

            IsFullscreen = Properties.Settings.Default.Fullscreen;
            ClientSize = new System.Drawing.Size(Properties.Settings.Default.Width, Properties.Settings.Default.Height);
            InitializeComponent();
            InitializeDirectX();
            Engine.RawInput.InitializeDirectRawInput();
            Engine.Initial(D2DRenderTarget);
            InitializeInputEventHandler();
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
                    Engine.Objects.RenderObjects();
                    D2DRenderTarget.EndDraw();
                    SwapChain.Present(0, PresentFlags.None);
                    LockModifys = false;
                }
            });
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
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
                Engine.Objects.Player.Acceleration.X -= 5;
                Engine.Objects.Player.Acceleration.Y -= 5;
            }
            else
            {
                if (Ap) { Engine.Objects.Player.Acceleration.X -= 10; }
            }
            if (Wp && Dp)
            {
                Engine.Objects.Player.Acceleration.Y -= 5;
                Engine.Objects.Player.Acceleration.X += 5;
            }
            else
            {
                if (Wp) { Engine.Objects.Player.Acceleration.Y += 10; }
            }
            if (Dp && Sp)
            {
                Engine.Objects.Player.Acceleration.X += 5;
                Engine.Objects.Player.Acceleration.Y += 5;
            }
            else
            {
                if (Dp) { Engine.Objects.Player.Acceleration.X += 10; }
            }
            if (Sp && Ap)
            {
                Engine.Objects.Player.Acceleration.Y += 5;
                Engine.Objects.Player.Acceleration.X -= 5;
            }
            else
            {
                if (Sp) { Engine.Objects.Player.Acceleration.Y -= 10; }
            }
            if (args.Key == Keys.Space && args.ScanCodeFlags == ScanCodeFlags.Make)
            {
                Engine.Objects.Player.Acceleration.Y = 15;
            }
            if (args.Key == Keys.R && args.ScanCodeFlags == ScanCodeFlags.Make)
            {
                Engine.Objects.Player.Respawn();
            }
        }

        private void MouseEvent(object sender, MouseInputEventArgs args)
        {

        }
    }
}