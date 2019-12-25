using HexaEngine.Core.Common;
using HexaEngine.Core.Game;
using HexaEngine.Core.Objects;
using HexaEngine.Core.Physics;
using SharpDX;
using SharpDX.Direct2D1;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        public void Dispose()
        {
            PhysicsEngine.Dispose();
            Objects.Dispose();
            Brushpalette.Dispose();
        }

        public void Initial(RenderTarget renderTarget)
        {
            RenderTarget = renderTarget;
            Camera = new Camera(renderTarget);
            PhysicsEngine = new PhysicsEngine(this);
            Objects = new ObjectSystem(this);
            Brushpalette.BrushBlack = new SolidColorBrush(renderTarget, Color.Black);
            Brushpalette.BrushRed = new SolidColorBrush(renderTarget, Color.Red);
        }
    }
}
