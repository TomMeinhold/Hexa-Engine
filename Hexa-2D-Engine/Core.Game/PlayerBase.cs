using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace HexaEngine.Core.Game
{
    public class PlayerBase : BaseObject
    {
        public PlayerBase(Engine engine, Bitmap1 bitmap, Vector3 position, RectangleF rectangle)
        {
            Name = "player";
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            SetPosition(position);
        }

        public PlayerBase(Engine engine, Bitmap1 bitmap, Vector3 position)
        {
            Name = "player";
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            SetPosition(position);
        }

        public void Respawn()
        {
            SetPosition(new RawVector3() { X = 0, Y = 0, Z = 0 });
        }
    }
}