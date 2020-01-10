using HexaEngine.Core.Common;
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
        public void ProcessingSpeed(BaseObject baseObject)
        {
            RawVector3 MaxSpeed = baseObject.MaxSpeed;
            RawVector3 Speed = baseObject.Speed;
            RawVector3 Acceleration = baseObject.Acceleration;
            if (Acceleration.X > 0)
            {
                if (Speed.X < MaxSpeed.X)
                {
                    if (Speed.X + Acceleration.X > MaxSpeed.X)
                    {
                        Speed.X = MaxSpeed.X;
                    }
                    else
                    {
                        Speed.X += Acceleration.X;
                    }
                }
            }
            else if(Acceleration.X < 0) {
                if (Speed.X > MaxSpeed.X * -1)
                {
                    if (Speed.X + Acceleration.X < MaxSpeed.X * -1)
                    {
                        Speed.X = MaxSpeed.X * -1;
                    }
                    else
                    {
                        Speed.X += Acceleration.X;
                    }
                }
            }
            if (Acceleration.Y > 0)
            {
                if (Speed.Y < MaxSpeed.Y)
                {
                    if (Speed.Y + Acceleration.Y > MaxSpeed.Y)
                    {
                        Speed.Y = MaxSpeed.Y;
                    }
                    else
                    {
                        Speed.Y += Acceleration.Y;
                    }
                }
            }
            else if (Acceleration.Y < 0)
            {
                if (Speed.Y > MaxSpeed.Y * -1)
                {
                    if (Speed.Y + Acceleration.Y < MaxSpeed.Y * -1)
                    {
                        Speed.Y = MaxSpeed.Y * -1;
                    }
                    else
                    {
                        Speed.Y += Acceleration.Y;
                    }
                }
            }
            if (Acceleration.Z > 0)
            {
                if (Speed.Z < MaxSpeed.Z)
                {
                    if (Speed.Z + Acceleration.Z > MaxSpeed.Z)
                    {
                        Speed.Z = MaxSpeed.Z;
                    }
                    else
                    {
                        Speed.Z += Acceleration.Z;
                    }
                }
            }
            else if (Acceleration.Z < 0)
            {
                if (Speed.Z > MaxSpeed.Z * -1)
                {
                    if (Speed.Z + Acceleration.Z < MaxSpeed.Z * -1)
                    {
                        Speed.Z = MaxSpeed.Z * -1;
                    }
                    else
                    {
                        Speed.Z += Acceleration.Z;
                    }
                }
            }

            baseObject.Speed = Speed;
        }
    }
}
