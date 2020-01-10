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
        public void ProcessingDamper(BaseObject obj)
        {
            RawVector3 Damper = obj.Damper;
            RawVector3 Speed = obj.Speed;
            RawVector3 Acceleration = obj.Acceleration;
            RawVector3 MovementAcceleration = obj.MovementAcceleration;
            if (MovementAcceleration.X == 0)
            {
                if (Acceleration.X == 0)
                {
                    if (Speed.X != 0)
                    {
                        if (Speed.X > 0)
                        {
                            if (Speed.X - Damper.X < 0)
                            {
                                Speed.X = 0;
                            }
                            else
                            {
                                Speed.X -= Damper.X;
                            }
                        }
                        else
                        {
                            if (Speed.X + Damper.X > 0)
                            {
                                Speed.X = 0;
                            }
                            else
                            {
                                Speed.X += Damper.X;
                            }
                        }
                    }
                }
            }
            obj.Damper = Damper;
            obj.Speed = Speed;
        }
    }
}
