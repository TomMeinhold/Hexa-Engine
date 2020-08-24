using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Timers;
using SharpDX.Direct2D1;
using System;

namespace HexaEngine.Core.Particle.Components
{
    public class BaseParticle : BaseObject
    {
        private readonly Timer timer;

        public BaseParticle(Engine engine, Bitmap1 bitmap, TimeSpan liveTime, PhysicsObjectDiscription physicsObjectDiscription)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
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