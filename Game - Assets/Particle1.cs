using HexaEngine.Core;
using HexaEngine.Core.Particle.Components;
using HexaEngine.Core.Physics.Structs;
using SharpDX.Direct2D1;
using System;

namespace GameAssets
{
    public class Particle1 : BaseParticle
    {
        public Particle1(Engine engine, TimeSpan liveTime, PhysicsObjectDiscription physicsObjectDiscription) : base(engine, Texture, liveTime, physicsObjectDiscription)
        {
        }

        public static Bitmap1 Texture { get; set; }
    }
}