using HexaEngine.Core;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Interfaces;
using HexaEngine.Core.Scenes;
using SharpDX.Windows;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Game
{
    public partial class MainWindow : RenderForm, IInputKeyboard
    {
        private readonly Engine Engine;

        private readonly List<Scene> scenes;

        public MainWindow()
        {
            InitializeComponent();
            Engine = new Engine(this);
            Engine.InputSystem.InputKeyboards.Add(this);
            scenes = GameAssets.Assets.GetScenes(Engine);
            Engine.SceneManager.SelectedScene = scenes[0];
            Engine.RenderSystem.MainLoop();
        }

        public void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
            if (update.Key == Keys.D1 && !update.IsPressed)
            {
                Engine.SceneManager.SelectedScene = scenes[0];
            }

            if (update.Key == Keys.D2 && !update.IsPressed)
            {
                Engine.SceneManager.SelectedScene = scenes[1];
            }

            if (update.Key == Keys.D3 && !update.IsPressed)
            {
                Engine.SceneManager.SelectedScene = scenes[2];
            }

            if (update.Key == Keys.D4 && !update.IsPressed)
            {
                Engine.SceneManager.SelectedScene = scenes[3];
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}