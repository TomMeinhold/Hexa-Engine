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
            Engine = new Engine(this);
            keyboardController = new KeyboardController(this);
            keyboardController.KeyUp += KeyboardController_KeyUp;
            Engine.RenderSystem.MainLoop();
        }

        private void KeyboardController_KeyUp(object sender, KeyboardUpdatePackage e)
        {
            if (e.KeyboardUpdate.Key == Keys.F1)
            {
                Engine.RenderSystem.FlipFullscreen();
            }

            if (e.KeyboardUpdate.Key == Keys.F3)
            {
                Engine.Settings.DebugMode = !Engine.Settings.DebugMode;
            }

            if (e.KeyboardUpdate.Key == Keys.P)
            {
                Engine.PhysicsEngine.Paused = !Engine.PhysicsEngine.Paused;
            }

            if (e.KeyboardUpdate.Key == Keys.O)
            {
                Engine.PhysicsEngine.DoCycle = !Engine.PhysicsEngine.DoCycle;
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}