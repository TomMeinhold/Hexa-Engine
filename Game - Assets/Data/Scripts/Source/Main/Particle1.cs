using HexaEngine.Core;
using HexaEngine.Core.Particle.Components;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Ressources;
using System;

namespace Main
{
    public class Particle1 : BaseParticle
    {
        public Particle1(Engine engine, TimeSpan liveTime, PhysicsObjectDiscription physicsObjectDiscription) : base(engine, sprite, liveTime, physicsObjectDiscription)
        {
        }

        public static Sprite sprite { get; set; }
    }
}