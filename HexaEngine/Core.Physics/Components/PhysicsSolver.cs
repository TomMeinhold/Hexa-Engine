using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Gravity;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Rays;
using System.Collections.Generic;

namespace HexaEngine.Core.Physics.Components
{
    public class PhysicsSolver
    {
        private readonly PhysicsEngine physicsEngine;

        public PhysicsSolver(PhysicsEngine physicsEngine, List<IPhysicsObject> physicsObjects, IPhysicsObject target)
        {
            this.physicsEngine = physicsEngine;
            PhysicsObjects = physicsObjects;
            Target = target;
        }

        private List<IPhysicsObject> PhysicsObjects { get; }

        private IPhysicsObject Target { get; }

        public void Process()
        {
            if (Target.Static == false)
            {
                Acceleration.ProcessObject(Target);

                Velocity.ProcessObject(Target, physicsEngine);

                foreach (IPhysicsObject physicsObject in PhysicsObjects)
                {
                    if (Target == physicsObject)
                    {
                        continue;
                    }
                    if (physicsObject.Mass != 0 && Target.Mass != 0)
                    {
                        Gravitation.Process(Target, physicsObject, physicsEngine);
                    }

                    Collisions.Process(Target, physicsObject);
                }
            }

            if (Target is IRayCasting rayCasting)
            {
                RayTrace.Process(rayCasting, Target, PhysicsObjects);
            }
        }
    }
}