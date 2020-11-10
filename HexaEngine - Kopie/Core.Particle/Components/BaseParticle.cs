using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Timers;
using System;

namespace HexaEngine.Core.Particle.Components
{
    public class BaseParticle : BaseObject
    {
        private readonly Timer timer;

        public BaseParticle(Sprite sprite, TimeSpan liveTime, PhysicsObjectDiscription physicsObjectDiscription)
        {
            Sprite = sprite;
            Size = sprite.Size;
            physicsObjectDiscription.SetValues(this);
            MassCenter = BoundingBox.Center - Position;
            timer = new Timer(liveTime, 1);
            timer.TimerTick += Timer_TimerTick;
            OnCollision += BaseParticle_OnCollision;
            timer.Start();
        }

        private void BaseParticle_OnCollision(object sender, Physics.Collision.OnCollisionEventArgs e)
        {
            //Destroy();
        }

        private void Timer_TimerTick(object sender, TimerTickEventArgs e)
        {
            Destroy();
        }
    }
}