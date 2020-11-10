using HexaEngine.Core;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Modules;
using HexaEngine.Core.Render;
using System.Windows.Forms;
using Keys = HexaEngine.Core.Input.Component.Keys;

namespace Game
{
    public partial class MainWindow : HexaForm
    {
        private readonly KeyboardController keyboardController;

        public MainWindow()
        {
            InitializeComponent();
            Engine.Create(this);
            keyboardController = new KeyboardController(this);
            keyboardController.KeyUp += KeyboardController_KeyUp;
            Engine.Current.Settings.AntialiasMode = true;
            Engine.Current.RenderSystem.MainLoop();
        }

        private void KeyboardController_KeyUp(object sender, KeyboardUpdatePackage e)
        {
            if (e.KeyboardUpdate.Key == Keys.F1)
            {
                Engine.Current.RenderSystem.FlipFullscreen();
            }

            if (e.KeyboardUpdate.Key == Keys.F3)
            {
                Engine.Current.Settings.DebugMode = !Engine.Current.Settings.DebugMode;
            }

            if (e.KeyboardUpdate.Key == Keys.P)
            {
                Engine.Current.PhysicsEngine.Paused = !Engine.Current.PhysicsEngine.Paused;
            }

            if (e.KeyboardUpdate.Key == Keys.O)
            {
                Engine.Current.PhysicsEngine.DoCycle = !Engine.Current.PhysicsEngine.DoCycle;
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}