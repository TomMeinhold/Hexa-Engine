using HexaEngine.Core;
using HexaEngine.Core.Common;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System.Threading;

namespace GameAssets
{
    public class Wall : BaseObject
    {
        public Wall(Engine engineCore, Bitmap bitmap, RawVector3 position, RectangleF rectangle)
        {
            Engine = engineCore;
            Bitmap = bitmap;
            Size = bitmap.Size;
            Hitbox = bitmap.Size;
            Dimentions = rectangle;
            Position = position;
            Collision = true;
            Moveable = false;
            Static = true;
            Gravity = new RawVector3(0, 0, 0);
            MaxSpeed = new RawVector3(0, 0, 0);
            Thread = new Thread(Worker);
            Thread.Start();
            State = BaseObjectState.Initialized;
        }

        public Wall(Engine engineCore, Bitmap bitmap, RawVector3 position)
        {
            Engine = engineCore;
            Bitmap = bitmap;
            Size = bitmap.Size;
            Hitbox = bitmap.Size;
            Dimentions = new RectangleF() { Size = bitmap.Size };
            Position = position;
            Collision = true;
            Moveable = false;
            Static = true;
            Gravity = new RawVector3(0, 0, 0);
            MaxSpeed = new RawVector3(0, 0, 0);
            Thread = new Thread(Worker);
            Thread.Start();
            State = BaseObjectState.Initialized;
        }

        private void Worker()
        {
            while (true)
            {
                RawVector2 vector = new RawVector2()
                {
                    X = Position.X,
                    Y = Position.Y * -1
                };
                vector = InsertCameraData.InsertRelativePositionObject(Engine, vector);
                RawVector2[] raws = new RawVector2[] { vector };
                Buffer.AddBuffer(raws);
            }
        }
    }
}
