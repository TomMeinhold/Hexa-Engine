using HexaEngine.Core.Common;
using HexaEngine.Core.Game;
using HexaEngine.Core.Objects;
using HexaEngine.Core.Physics;
using SharpDX;
using SharpDX.Direct2D1;

namespace HexaEngine.Core
{
    public class Engine
    {

        public void Dispose()
        {
            PhysicsEngine.Dispose();
            Objects.Dispose();
            Brushpalette.Dispose();
        }

        public Camera Camera { get; set; }

        public ObjectSystem Objects { get; set; }
        public RenderTarget RenderTarget { get; internal set; }

        public Brushpalette Brushpalette = new Brushpalette();

        public PhysicsEngine PhysicsEngine { get; set; }

        public void PreInitial()
        {
            PhysicsEngine = new PhysicsEngine(this);
        }

        public void Initial(RenderTarget renderTarget)
        {
            RenderTarget = renderTarget;
            Camera = new Camera(renderTarget);
            Objects = new ObjectSystem(this);
            Brushpalette.BrushBlack = new SolidColorBrush(renderTarget, Color.Black);
            Brushpalette.BrushRed = new SolidColorBrush(renderTarget, Color.Red);
        }

    }
}
