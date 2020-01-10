using HexaEngine.Core.Game;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace HexaEngine.Core.Common
{
    public static class InsertCameraData
    {
        static public RawVector2 InsertRelativePosition(CameraBase camera, RawVector2 vector2)
        {
            vector2.X += camera.X;
            vector2.Y += camera.Y;
            return vector2;
        }
        static public RawVector2 InsertRelativePositionObject(Engine engine, RawVector2 vector2)
        {
            CameraBase camera = engine.Camera;
            RenderTarget target = engine.RenderTarget;
            Size2F size;
            if (engine.ObjectSystem != null)
            {
                if (engine.ObjectSystem.Player != null)
                {
                    size = engine.ObjectSystem.Player.Size;
                }
                else
                {
                    size = new Size2F(0, 0);
                }
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
