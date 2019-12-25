using HexaEngine.Core.Physics;
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
    public class Player : BaseObject
    {
        public Player(Engine engineCore, Bitmap bitmap, Vector3 position, RectangleF rectangle)
        {
            Engine = engineCore;
            Bitmap = bitmap;
            Size = bitmap.Size;
            Hitbox = bitmap.Size;
            Dimentions = rectangle;
            Position = position;
            Collision = true;
            Moveable = true;
            Gravity = new RawVector3(0, 0.5F, 0);
            CameraFocus = true;
            Thread = new Thread(Worker);
            Thread.Start();
        }

        public Player(Engine engineCore, Bitmap bitmap, Vector3 position)
        {
            Engine = engineCore;
            Bitmap = bitmap;
            Size = bitmap.Size;
            Hitbox = bitmap.Size;
            Dimentions = new RectangleF() { Size = bitmap.Size };
            Position = position;
            Gravity = new RawVector3(0, 1, 0);
            Collision = true;
            Moveable = true;
            CameraFocus = true;
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
                vector = InsertCameraData.InsertRelativePositionObject(Engine, vector);
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

        public void Respawn()
        {
            Position = new RawVector3() { X = 0, Y = 0, Z = 0 };
        }
    }
}
