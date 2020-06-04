using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Gravity;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Rays;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Physics.Components
{
    public class PhysicsSolver
    {
        private readonly PhysicsEngine engine;

        public PhysicsSolver(PhysicsEngine engine, List<IPhysicsObject> physicsObjects, IPhysicsObject target)
        {
            this.engine = engine;
            PhysicsObjects = physicsObjects;
            Target = target;
        }

        private List<IPhysicsObject> PhysicsObjects { get; }

        private IPhysicsObject Target { get; }

        public void Process()
        {
            if (Target is IRayCasting rayCasting)
            {
                RayTrace.Process(rayCasting, Target, PhysicsObjects);
            }

            if (Target.Static == false)
            {
                foreach (IPhysicsObject physicsObject in PhysicsObjects)
                {
                    if (Target == physicsObject)
                    {
                        continue;
                    }

                    Gravitation.ProcessObject(Target, physicsObject);
                    Collisions.Process(Target, physicsObject);
                }

                Acceleration.ProcessObject(Target);
                Velocity.ProcessObject(Target, engine.ThreadTiming);
            }
        }
    }
}