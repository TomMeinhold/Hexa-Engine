using HexaEngine.Core.Common;
using HexaEngine.Core.Input;
using HexaEngine.Core.Network;
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
            Instances[AppDomain.CurrentDomain.Id] = this;
            Settings = new EngineSettings
            {
                Width = renderForm.Width,
                Height = renderForm.Height
            };
            BitmapConverter = new BitmapConverter();
            InputSystem = new InputSystem(renderForm);
            Camera = new Camera();
            SceneManager = new SceneManager();
            SoundSystem = new SoundSystem();
            UIManager = new UserInterfaceManager();
            RenderSystem = new RenderSystem(renderForm);
            RessouceManager = new RessourceManager();
            Compiler = new ScriptCompiler();
            PluginManager = new PluginManager();
            PhysicsEngine = new PhysicsEngine();
            Server = new Server();
            LoadRessources?.Invoke(this, this);
        }

        public Engine()
        {
            Instances[AppDomain.CurrentDomain.Id] = this;
            SceneManager = new SceneManager();
            Compiler = new ScriptCompiler();
            PluginManager = new PluginManager();
            PhysicsEngine = new PhysicsEngine();
            Server = new Server();
            StartServer?.Invoke(this, this);
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