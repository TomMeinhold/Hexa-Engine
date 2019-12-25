﻿using HexaEngine.Core.Objects;
using SharpDX;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine
    {
        readonly Thread MainThread;

        internal void ProcessingPhysics()
        {
            while (Engine.Objects == null) { }
            ObjectSystem objects = Engine.Objects;
            while (true)
            {
                Thread.Sleep(20);
                int i = 0;
                
                foreach (dynamic s in objects.ObjectList)
                {
                    bool[] LockXYZ = s.DirectionBlocked;

                    

                    //Gravity support only Y for now
                    if (LockXYZ[0] == false)
                    {
                        RawVector3 v1 = s.Gravity;
                        RawVector3 v2 = s.Acceleration;
                        v2.Y -= v1.Y;
                        s.Acceleration = v2;
                    }

                    //Movement
                    if (s.Acceleration.X != 0)
                    {
                        const float deacc = 0.1F;
                        if (s.Acceleration.X > 0)
                        {
                            RawVector3 v2 = s.Acceleration;
                            if (v2.X - deacc < 0)
                            {
                                v2.X = 0;
                            }
                            else
                            {
                                v2.X -= deacc;
                            }
                            s.Acceleration = v2;
                        }
                        else
                        {
                            RawVector3 v2 = s.Acceleration;
                            if (v2.X + deacc > 0)
                            {
                                v2.X = 0;
                            }
                            else
                            {
                                v2.X += deacc;
                            }
                            s.Acceleration = v2;
                        }
                    }

                    //Acceleration
                    if (LockXYZ[0] == true)
                    {
                        RawVector3 v2 = s.Acceleration;
                        if (v2.Y < 0)
                        {
                            v2.Y = 0;
                        }
                        s.Acceleration = v2;
                    }

                    //Acceleration Maximum
                    if (s.Moveable)
                    {
                        RawVector3 v2 = s.Acceleration;
                        if (v2.X > 10)
                        {
                            v2.X = 10;
                        }
                        if (v2.X < -10)
                        {
                            v2.X = -10;
                        }
                        if (v2.Y > 200)
                        {
                            v2.Y = 200;
                        }
                        if (v2.Y < -200)
                        {
                            v2.Y = -200;
                        }
                        s.Acceleration = v2;
                    }


                    // Check Collision and set new Position
                    if (s.Collision)
                    {
                        int j = 0;
                        foreach (dynamic a in objects.ObjectList)
                        {
                            if (j != i)
                            {
                                RawVector3 x2 = s.Acceleration;
                                RawVector3 v1 = s.Position;
                                v1.Y += x2.Y;
                                v1.X += x2.X;
                                RawVector3 v2 = a.Position;
                                RectangleF r1 = s.Dimentions;
                                RectangleF r2 = a.Dimentions;
                                r1.Location = new RawVector2(v1.X, v1.Y);
                                r2.Location = new RawVector2(v2.X, v2.Y);
                                RectangleF r3 = RectangleF.Intersect(r1, r2);
                                if (r3.Height != 0)
                                {
                                    if (s.Moveable)
                                    {
                                        v1.Y += r3.Height;
                                        x2.Y = 0;
                                        s.DirectionBlocked[0] = true;
                                    }
                                }
                                else
                                {
                                    s.DirectionBlocked[0] = false;
                                }
                                // Set cam to object if focus
                                if (s.CameraFocus)
                                {
                                    Vector2 v = new RawVector2((v1.X - s.Size.Width / 2) * -1, v1.Y - s.Size.Height / 2);
                                    Engine.Camera.X = v.X;
                                    Engine.Camera.Y = v.Y;
                                }
                                s.Position = v1;
                                s.Acceleration = x2;
                            }
                            j++;
                        }
                    }
                    i++;
                }
            }
        }
    }
}
