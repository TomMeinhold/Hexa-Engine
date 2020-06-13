using HexaEngine.Core;
using HexaEngine.Core.Input;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Interfaces;
using HexaEngine.Core.Render;
using HexaEngine.Core.Scenes;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Game
{
    public partial class MainWindow : HexaForm, IInputKeyboard
    {
        private readonly List<Scene> scenes;

        public MainWindow()
        {
            InitializeComponent();
            Engine = new Engine(this);
            InputSystem.KeyboardUpdate += KeyboardInput;
            scenes = GameAssets.Assets.GetScenes(Engine);
            Engine.SceneManager.SelectedScene = scenes[0];
            ((HexaEngine.Core.UI.BaseTypes.Button)((HexaEngine.Core.UI.BaseTypes.Screen)Engine.UIManager.ActiveUserInterface).UserInterfaces[0]).Click += MainWindow_Click0;
            ((HexaEngine.Core.UI.BaseTypes.Button)((HexaEngine.Core.UI.BaseTypes.Screen)Engine.UIManager.ActiveUserInterface).UserInterfaces[1]).Click += MainWindow_Click1;
            ((HexaEngine.Core.UI.BaseTypes.Button)((HexaEngine.Core.UI.BaseTypes.Screen)Engine.UIManager.ActiveUserInterface).UserInterfaces[2]).Click += MainWindow_Click2;
            Engine.RenderSystem.MainLoop();
        }

        private void MainWindow_Click0(object sender, System.EventArgs e)
        {
            Engine.SceneManager.SelectedScene = scenes[0];
        }

        private void MainWindow_Click1(object sender, System.EventArgs e)
        {
            Engine.SceneManager.SelectedScene = scenes[1];
        }

        private void MainWindow_Click2(object sender, System.EventArgs e)
        {
            Engine.SceneManager.SelectedScene = scenes[2];
        }

        public void KeyboardInput(object sender, KeyboardUpdatePackage package)
        {
            KeyboardUpdate update = package.KeyboardUpdate;

            if (update.Key == Keys.F11 && !update.IsPressed)
            {
                Engine.RenderSystem.FlipFullscreen();
            }

            if (update.Key == Keys.F3 && !update.IsPressed)
            {
                Engine.Settings.DebugMode = !Engine.Settings.DebugMode;
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}