using HexaEngine.Core.Audio;
using HexaEngine.Core.Common;
using HexaEngine.Core.Input;
using HexaEngine.Core.Physics;
using HexaEngine.Core.Render;
using HexaEngine.Core.Scenes;
using HexaEngine.Core.UI;
using System;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        internal long ThreadSyncTiming;

        public Camera Camera { get; set; }

        public Brushpalette Brushpalette { get; set; } = new Brushpalette();

        public InputSystem InputSystem { get; set; }

        public EngineTransform Transform { get; set; }

        public EngineSettings Settings { get; set; }

        [Obsolete]
        public AudioSystem AudioSystem { get; set; } = new AudioSystem();

        public SceneManager SceneManager { get; set; } = new SceneManager();

        public UserInterfaceManager UIManager { get; set; }

        public PhysicsEngine PhysicsEngine { get; set; }

        public RenderSystem RenderSystem { get; set; }
    }
}