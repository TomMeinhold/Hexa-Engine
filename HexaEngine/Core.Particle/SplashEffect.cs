using HexaEngine.Core.Particle.Components;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Structs;
using System;

namespace HexaEngine.Core.Particle
{
    public class SplashEffect<T> where T : BaseParticle
    {
        private readonly int amount;

        private readonly TimeSpan livetime;

        public SplashEffect(Engine engine, IPhysicsObject physicsObject, int amount, TimeSpan livetime)
        {
            Engine = engine;
            PhysicsObject = physicsObject;
            this.amount = amount;
            this.livetime = livetime;
        }

        public Engine Engine { get; }

        public float Mass { get; set; }

        public IPhysicsObject PhysicsObject { get; }

        public void CastParticles()
        {
            for (int i = 0; i < 360;)
            {
                float x = (float)Math.Cos(Math.PI * i / 180) * 500;
                float y = (float)Math.Sin(Math.PI * i / 180) * 500;
                T particle = (T)Activator.CreateInstance(typeof(T), new object[] { Engine, livetime, new PhysicsObjectDiscription() { Mass = Mass, Velocity = new SharpDX.Vector3(x, y, 0), Position = PhysicsObject.BoundingBox.Center } });
                Engine.SceneManager.SelectedScene.Add(particle);
                i += 360 / amount;
            }
        }
    }
}