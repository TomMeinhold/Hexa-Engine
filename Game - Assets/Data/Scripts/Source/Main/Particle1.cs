using HexaEngine.Core.Particle.Components;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Ressources;
using System;

namespace Main
{
    public class Particle1 : BaseParticle
    {
        public Particle1(TimeSpan liveTime, PhysicsObjectDiscription physicsObjectDiscription) : base(ParticleSprite, liveTime, physicsObjectDiscription)
        {
        }

        public static Sprite ParticleSprite { get; set; }
    }
}