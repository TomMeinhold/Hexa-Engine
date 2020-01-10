using HexaEngine.Core.Common;
using HexaEngine.Core.Objects;
using SharpDX;
using SharpDX.Mathematics.Interop;
using System.Threading;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine
    {
        readonly Thread MainThread;

        internal void ProcessingPhysics()
        {
            State = PhysicsEngineState.Waiting;
            while (Engine.ObjectSystem == null) {  }
            ObjectSystem objects = Engine.ObjectSystem;
            while (objects.State != ObjectSystemState.Active) {  }
            State = PhysicsEngineState.Active;
            InternalState = PhysicsEngineInternalState.SetStateActive;
            while (true)
            {
                if (InternalState == PhysicsEngineInternalState.SetStateWaiting)
                {
                    State = PhysicsEngineState.Waiting;
                    while (InternalState != PhysicsEngineInternalState.SetStateActive) { Thread.Sleep(1); }
                    State = PhysicsEngineState.Active;
                }
                Thread.Sleep(20);
                int i = 0;

                foreach (BaseObject obj in objects.ObjectList)
                {
                    if (obj.State == BaseObjectState.Uninitialized)
                    {
                        i++;
                        continue;
                    }
                    bool[] LockXYZ = obj.DirectionBlocked;
                    ProcessingDamper(obj);
                    ProcessingAcceleration(obj);
                    ProcessingSpeed(obj);
                    
                    if (!obj.Static)
                    {
                        if (obj.Collision)
                        {
                            int j = 0;
                            foreach (BaseObject obj1 in objects.ObjectList)
                            {
                                if (j != i)
                                {
                                    if (obj1.Collision)
                                    {
                                        if (!obj.Static && obj1.Static)
                                        {
                                            ProcessStaticCollision(obj, obj1);
                                        }
                                    }
                                }
                                j++;
                            }
                        }
                    }
                    i++;

                    ProcessingPosition(obj);

                    if (obj.CameraFocus)
                    {
                        Vector2 v = new RawVector2((obj.Position.X - obj.Size.Width / 2) * -1, obj.Position.Y - obj.Size.Height / 2);
                        Engine.Camera.X = v.X;
                        Engine.Camera.Y = v.Y;
                    }
                }
            }
        }
    }
}
