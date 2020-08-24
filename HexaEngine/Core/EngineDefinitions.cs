using HexaEngine.Core.Common;
using HexaEngine.Core.Input;
using HexaEngine.Core.Physics;
using HexaEngine.Core.Render;
using HexaEngine.Core.Scenes;
using HexaEngine.Core.UI;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        internal long ThreadSyncTiming;

        public Camera Camera { get; set; }

        public Brushpalette Brushpalette { get; set; } = new Brushpalette();

        public InputSystem InputSystem { get; set; }

        public BitmapConverter BitmapConverter { get; set; }

        public EngineSettings Settings { get; set; }

        public SceneManager SceneManager { get; set; }

        public UserInterfaceManager UIManager { get; set; }

        public PhysicsEngine PhysicsEngine { get; set; }

        public RenderSystem RenderSystem { get; set; }
    }
}