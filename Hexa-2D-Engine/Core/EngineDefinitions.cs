using HexaEngine.Core.Common;
using HexaEngine.Core.Game;
using HexaEngine.Core.Input;
using HexaEngine.Core.Objects;
using HexaEngine.Core.Physics;
using SharpDX.Direct2D1;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        public Camera Camera { get; set; }

        public ObjectSystem Objects { get; set; }

        public RenderTarget RenderTarget { get; internal set; }

        public Brushpalette Brushpalette = new Brushpalette();

        public RawInput RawInput = new RawInput();

        public PhysicsEngine PhysicsEngine { get; set; }
    }
}
