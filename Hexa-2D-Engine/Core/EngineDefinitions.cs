using HexaEngine.Core.Audio;
using HexaEngine.Core.Common;
using HexaEngine.Core.Debug;
using HexaEngine.Core.Game;
using HexaEngine.Core.Input;
using HexaEngine.Core.Objects;
using HexaEngine.Core.Physics;
using SharpDX.Direct2D1;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        public CameraBase Camera { get; set; }

        public ObjectSystem ObjectSystem { get; set; }

        public RenderTarget RenderTarget { get; internal set; }

        public Brushpalette Brushpalette = new Brushpalette();

        public RawInput RawInput = new RawInput();

        public AudioSystem AudioSystem = new AudioSystem();

        public PhysicsEngine PhysicsEngine { get; set; }

        public DebugWindow DebugWindow { get; set; }

        public DeviceContext DeviceContext;
    }
}
