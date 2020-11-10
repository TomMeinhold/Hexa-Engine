using HexaEngine.Core.Sounds;
using HexaEngine.Core.Common;
using HexaEngine.Core.Input;
using HexaEngine.Core.Physics;
using HexaEngine.Core.Plugins;
using HexaEngine.Core.Render;
using HexaEngine.Core.Scenes;
using HexaEngine.Core.Scripts;
using HexaEngine.Core.UI;
using System.IO;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Network;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        internal long ThreadSyncTiming;

        public Server Server { get; set; }

        public Camera Camera { get; set; }

        public Brushpalette Brushpalette { get; set; } = new Brushpalette();

        public InputSystem InputSystem { get; set; }

        public BitmapConverter BitmapConverter { get; set; }

        public EngineSettings Settings { get; set; }

        public SoundSystem SoundSystem { get; set; }

        public SceneManager SceneManager { get; set; }

        public UserInterfaceManager UIManager { get; set; }

        public PhysicsEngine PhysicsEngine { get; set; }

        public RenderSystem RenderSystem { get; set; }

        public RessourceManager RessouceManager { get; set; }

        public ScriptCompiler Compiler { get; set; }

        public PluginManager PluginManager { get; set; }

        public bool UseScriptCache { get; set; } = false;

        public static DirectoryInfo Base { get; set; } = new DirectoryInfo("Data\\");

        public static DirectoryInfo ScriptSourcePath { get; set; } = new DirectoryInfo("Data\\Scripts\\Source");

        public static DirectoryInfo ScriptCache { get; set; } = new DirectoryInfo("Data\\Scripts\\Cache");

        public static DirectoryInfo PreCompiledScripts { get; set; } = new DirectoryInfo("Data\\Scripts");

        public static DirectoryInfo PluginsPath { get; set; } = new DirectoryInfo("Data\\Plugins");

        public static DirectoryInfo SoundsPath { get; set; } = new DirectoryInfo("Data\\Sounds");

        public static DirectoryInfo TexturePath { get; set; } = new DirectoryInfo("Data\\Textures\\");
    }
}