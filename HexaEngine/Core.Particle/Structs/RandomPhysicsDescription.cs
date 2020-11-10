using HexaEngine.Core.Physics.Structs;
using System;

namespace HexaEngine.Core.Particle.Structs
{
    public struct RandomPhysicsDescription
    {
        public Range3 PositionRange;
        public Range3 VelocityRange;
        public Range3 ScaleRange;
        public Range3 RotationRange;
        public Range3 RotationVelocityRange;
        public Range ForceAbsorbtionRange;
        public Range MassRange;
        public bool Colliding;

        public PhysicsObjectDiscription GetDiscription()
        {
            Random seed = new Random();
            Random random = new Random(seed.Next());
            return new PhysicsObjectDiscription
            {
                Colliding = Colliding,
                Position = PositionRange.GetVector(random),
                Velocity = VelocityRange.GetVector(random),
                Scale = ScaleRange.GetVector(random),
                Rotation = RotationRange.GetVector(random),
                RotationVelocity = RotationVelocityRange.GetVector(random),
                ForceAbsorbtion = ForceAbsorbtionRange.GetFloat(random),
                Mass = MassRange.GetFloat(random)
            };
        }
    }
}