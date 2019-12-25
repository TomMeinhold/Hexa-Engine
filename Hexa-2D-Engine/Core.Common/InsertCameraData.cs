using HexaEngine.Core.Game;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Common
{
    public static class InsertCameraData
    {
        static public RawVector2 InsertRelativePosition(Camera camera, RawVector2 vector2)
        {
            vector2.X += camera.X;
            vector2.Y += camera.Y;
            return vector2;
        }
        static public RawVector2 InsertRelativePositionObject(Engine engine, RawVector2 vector2)
        {
            Camera camera = engine.Camera;
            RenderTarget target = engine.RenderTarget;
            Size2F size;
            if (engine.Objects != null)
            {
                size = engine.Objects.Player.Size;
            }
            else
            {
                size = new Size2F(0, 0);
            }
            vector2.X += camera.X + target.Size.Width / 2 - size.Width / 2;
            vector2.Y += camera.Y + target.Size.Height / 2 - size.Height / 2;
            return vector2;
        }
    }
}
