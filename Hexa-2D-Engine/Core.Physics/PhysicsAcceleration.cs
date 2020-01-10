using HexaEngine.Core.Common;
using SharpDX.Mathematics.Interop;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine
    {
        public void ProcessingAcceleration(BaseObject baseObject)
        {
            RawVector3 Acceleration = baseObject.Acceleration;
            RawVector3 Gravity = baseObject.Gravity;
            RawVector3 NaturalDeceleration = baseObject.NaturalDeceleration;
            RawVector3 MovementAcceleration = baseObject.MovementAcceleration;

            if (MovementAcceleration.X != 0)
            {
                if (MovementAcceleration.X > 0)
                {
                    if (MovementAcceleration.X - NaturalDeceleration.X < 0)
                    {
                        MovementAcceleration.X = 0;
                    }
                    else
                    {
                        MovementAcceleration.X -= NaturalDeceleration.X;
                    }
                }
                else
                {
                    if (MovementAcceleration.X + NaturalDeceleration.X > 0)
                    {
                        MovementAcceleration.X = 0;
                    }
                    else
                    {
                        MovementAcceleration.X += NaturalDeceleration.X;
                    }
                }
            }
            if (MovementAcceleration.Y != 0)
            {
                if (MovementAcceleration.Y > 0)
                {
                    if (MovementAcceleration.Y - NaturalDeceleration.Y < 0)
                    {
                        MovementAcceleration.Y = 0;
                    }
                    else
                    {
                        MovementAcceleration.Y -= NaturalDeceleration.Y;
                    }
                }
                else
                {
                    if (MovementAcceleration.Z + NaturalDeceleration.Z > 0)
                    {
                        MovementAcceleration.Z = 0;
                    }
                    else
                    {
                        MovementAcceleration.Z += NaturalDeceleration.Z;
                    }
                }
            }
            if (MovementAcceleration.Z != 0)
            {
                if (MovementAcceleration.Z > 0)
                {
                    if (MovementAcceleration.Z - NaturalDeceleration.Z < 0)
                    {
                        MovementAcceleration.Z = 0;
                    }
                    else
                    {
                        MovementAcceleration.Z -= NaturalDeceleration.Z;
                    }
                }
                else
                {
                    if (MovementAcceleration.Z + NaturalDeceleration.Z > 0)
                    {
                        MovementAcceleration.Z = 0;
                    }
                    else
                    {
                        MovementAcceleration.Z += NaturalDeceleration.Z;
                    }
                }
            }


            Acceleration.X = Gravity.X + MovementAcceleration.X;
            Acceleration.Y = Gravity.Y + MovementAcceleration.Y;
            Acceleration.Z = Gravity.Z + MovementAcceleration.Z;


            baseObject.MovementAcceleration = MovementAcceleration;
            baseObject.Acceleration = Acceleration;
        }
    }
}
