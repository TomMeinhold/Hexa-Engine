using HexaEngine.Core.Particle.Components;
using HexaEngine.Core.Particle.Structs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Particle
{
    public class ParticleSystem<T> : IDisposable where T : BaseParticle
    {
        public RandomPhysicsDescription Description { get; }

        private readonly int amount;
        private readonly TimeSpan spawnTime;
        private readonly TimeSpan livetime;
        private readonly List<BaseParticle> particles = new List<BaseParticle>();

        private readonly Task task;

        private bool running;
        private bool paused;

        public ParticleSystem(RandomPhysicsDescription description, int amount, TimeSpan spawnTime, TimeSpan livetime)
        {
            running = true;
            paused = true;
            Description = description;
            this.amount = amount;
            this.spawnTime = spawnTime;
            this.livetime = livetime;
            task = new Task(CastParticles);
            task.Start();
            ParticleSystems.Tasks.Add(task);
        }

        public void Start()
        {
            paused = false;
        }

        public void Pause()
        {
            paused = true;
        }

        public void CastParticles()
        {
            while (running)
            {
                while (paused && running)
                {
                    Thread.Sleep(10);
                }

                while (amount <= particles.Count && running)
                {
                    Thread.Sleep(10);
                }

                while (Engine.Current.SceneManager.SelectedScene == null && running)
                {
                    Thread.Sleep(10);
                }

                if (!running)
                {
                    return;
                }

                T particle = (T)Activator.CreateInstance(typeof(T), new object[] { livetime, Description.GetDiscription() });
                Engine.Current.SceneManager.SelectedScene.Add(particle);
                particles.Add(particle);
                particle.PreOnDestroy += (ss, ee) =>
                {
                    particles.Remove(particle);
                };
                Thread.Sleep(spawnTime);
            }
        }

        public void Dispose()
        {
            running = false;
            paused = false;
            task.Wait();
            particles.ForEach(x => x?.Destroy());
        }
    }
}