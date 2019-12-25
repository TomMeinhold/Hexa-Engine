using HexaEngine.Core.Game;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine
    {
        public RawVector2 InsertRelativePosition(RawVector2 vector2)
        {
            Camera camera = Engine.Camera;
            vector2.X += camera.X;
            vector2.Y += camera.Y;
            return vector2;
        }
        public RawVector2 InsertRelativePositionObject(RawVector2 vector2)
        {
            Camera camera = Engine.Camera;
            vector2.X += camera.X + Engine.RenderTarget.Size.Width / 2;
            vector2.Y += camera.Y + Engine.RenderTarget.Size.Height / 2;
            return vector2;
        }
    }
}
