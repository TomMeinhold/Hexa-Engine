using HexaEngine.Core.Common;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Game
{
    public class Wall : BaseObject
    {
        public Wall(Engine engineCore, Bitmap bitmap, Vector3 position , RectangleF rectangle)
        {
            Engine = engineCore;
            Bitmap = bitmap;
            Size = bitmap.Size;
            Hitbox = bitmap.Size;
            Dimentions = rectangle;
            Position = position;
            Collision = true;
            Moveable = false;
            Gravity = new RawVector3(0, 0, 0);
            Thread = new Thread(Worker);
            Thread.Start();
        }

        public Wall(Engine engineCore, Bitmap bitmap, Vector3 position)
        {
            Engine = engineCore;
            Bitmap = bitmap;
            Size = bitmap.Size;
            Hitbox = bitmap.Size;
            Dimentions = new RectangleF() { Size = bitmap.Size };
            Position = position;
            Collision = true;
            Moveable = false;
            Gravity = new RawVector3(0, 0, 0);
            Thread = new Thread(Worker);
            Thread.Start();
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
                vector = Engine.PhysicsEngine.InsertRelativePositionObject(vector);
                RawVector2[] raws = new RawVector2[] { vector };
                Buffer.AddBuffer(raws);
            }
        }

        public void Draw()
        {
            RenderTarget target = Engine.RenderTarget;
            RawVector2[] raws = Buffer.GetBuffer();
            RectangleF rectangle = new RectangleF
            {
                Size = Bitmap.Size,
                X = raws[0].X,
                Y = raws[0].Y
            };
            target.DrawBitmap(Bitmap, rectangle, 1, BitmapInterpolationMode.Linear, Dimentions);
        }
    }
}
