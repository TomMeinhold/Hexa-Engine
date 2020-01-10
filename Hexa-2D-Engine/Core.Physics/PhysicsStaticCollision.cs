using HexaEngine.Core.Common;
using SharpDX;
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
        public (BaseObject,BaseObject) ProcessStaticCollision(BaseObject obj, BaseObject obj1)
        {
            RawVector3 Speed_obj = obj.Speed;
            RawVector3 Position_obj = obj.Position;
            RawVector3 Position_obj1 = obj1.Position;
            RectangleF Dimentions_obj = obj.Dimentions;
            RectangleF Dimentions_obj1 = obj1.Dimentions;
            Position_obj.Y += Speed_obj.Y;
            Position_obj.X += Speed_obj.X;

            Dimentions_obj.Location = new RawVector2(Position_obj.X, Position_obj.Y);
            Dimentions_obj1.Location = new RawVector2(Position_obj1.X, Position_obj1.Y);

            RectangleF CollisonRect = RectangleF.Intersect(Dimentions_obj, Dimentions_obj1);

            bool[] Collisions = this.Collisions(Dimentions_obj1, Dimentions_obj);

            if (Collisions[0]) // Top
            {
                if (obj.Moveable)
                {
                    if (Speed_obj.Y > 0)
                    {
                        Speed_obj.Y = 0;
                        Position_obj.Y -= CollisonRect.Height;
                    }
                    obj.DirectionBlocked[0] = true;
                }
            }
            else
            {
                obj.DirectionBlocked[0] = false;
            }

            if (Collisions[1]) // Bottom
            {
                if (obj.Moveable)
                {
                    if (Speed_obj.Y < 0)
                    {
                        Speed_obj.Y = 0;
                        Position_obj.Y += CollisonRect.Height;
                    }
                    obj.DirectionBlocked[1] = true;
                }
            }
            else
            {
                obj.DirectionBlocked[1] = false;
            }

            if (Collisions[2]) // Left
            {
                if (obj.Moveable)
                {
                    if (Speed_obj.X < 0)
                    {
                        Speed_obj.X = 0;
                        Position_obj.X += CollisonRect.Width;
                    }
                    obj.DirectionBlocked[2] = true;
                }
            }
            else
            {
                obj.DirectionBlocked[2] = false;
            }

            if (Collisions[3]) // Right
            {
                if (obj.Moveable)
                {
                    if (Speed_obj.X > 0)
                    {
                        Speed_obj.X = 0;
                        Position_obj.X -= CollisonRect.Width;
                    }
                    obj.DirectionBlocked[3] = true;
                }
            }
            else
            {
                obj.DirectionBlocked[3] = false;
            }
            obj.Position = Position_obj;
            obj.Speed = Speed_obj;
            return (obj,obj1);
        }
    }
}
