using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using HexaEngine.Core.Common;
using HexaEngine.Core.Input;
using HexaEngine.Core.Physics;
using HexaEngine.Core.Plugins;
using HexaEngine.Core.Render;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Scenes;
using HexaEngine.Core.Scripts;
using HexaEngine.Core.Sounds;
using HexaEngine.Core.Timers;
using HexaEngine.Core.UI;
using SharpDX.Windows;
using System;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        public Engine(RenderForm renderForm)
        {
            Settings = new EngineSettings
            {
                Width = renderForm.Width,
                Height = renderForm.Height
            };
            BitmapConverter = new BitmapConverter(this);
            InputSystem = new InputSystem(renderForm);
            Camera = new Camera(this);
            SceneManager = new SceneManager(this);
            SoundSystem = new SoundSystem(this);
            UIManager = new UserInterfaceManager(this);
            RenderSystem = new RenderSystem(this, renderForm);
            RessouceManager = new RessouceManager(this);
            Compiler = new ScriptCompiler(this);
            PluginManager = new PluginManager(this);
            PhysicsEngine = new PhysicsEngine(this);
            LoadRessources?.Invoke(this, this);
        }

        public void Dispose()
        {
            PhysicsEngine.Dispose();
            Brushpalette.Dispose();
            TimerFramework.Dispose();
        }

        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }
    }
}