using HexaEngine.Core;
using HexaEngine.Core.Extensions;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Modules;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Particle;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Render.Interfaces;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Timers;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System.Collections.Generic;

namespace Main
{
    public class Planet : BaseObject, IBaseObject, IDrawable, IPhysicsObject
    {
        private readonly List<Vector2> positions = new List<Vector2>();

        private readonly Timer timer = new Timer(new System.TimeSpan(0, 0, 0, 0, 16));

        private readonly KeyboardController keyboardController;

        private readonly SplashEffect<Particle1> splashEffect;

        private Planet(Engine engine)
        {
            keyboardController = new KeyboardController(this);
            keyboardController.KeyUp += KeyboardController_KeyUp;
            splashEffect = new SplashEffect<Particle1>(engine, this, 100, new System.TimeSpan(0, 0, 2));
            OnCollision += Planet_OnCollision;
            OnDestroy += Planet_OnDestroy;
        }

        public Planet(Engine engine, Sprite sprite, PhysicsObjectDiscription physicsObjectDiscription) : this(engine)
        {
            Engine = engine;
            Sprite = sprite;
            Size = sprite.Size;
            physicsObjectDiscription.SetValues(this);
            MassCenter = BoundingBox.Center - Position;
            timer.TimerTick += Timer_TimerTick;
            timer.Start();
        }

        public Planet(Engine engine, Sprite sprite, RawVector3 position, PhysicsObjectDiscription physicsObjectDiscription) : this(engine)
        {
            Engine = engine;
            Sprite = sprite;
            Size = sprite.Size;
            physicsObjectDiscription.SetValues(this);
            SetPosition(position);
            MassCenter = BoundingBox.Center - Position;
        }

        private void KeyboardController_KeyUp(object sender, KeyboardUpdatePackage e)
        {
            if (e.KeyboardUpdate.Key == Keys.U && e.KeyboardUpdate.IsPressed)
            {
                Static = !Static;
            }
        }

        private void Timer_TimerTick(object sender, TimerTickEventArgs e)
        {
            lock (positions)
            {
                if (positions.Count < 500)
                {
                    positions.Add(BoundingBox.Center.Downgrade());
                }
                else
                {
                    positions.RemoveAt(0);
                    positions.Add(BoundingBox.Center.Downgrade());
                }
            }
        }

        private void Planet_OnCollision(object sender, HexaEngine.Core.Physics.Collision.OnCollisionEventArgs e)
        {
            if (e.Object is Sun)
            {
                this.Destroy();
            }
        }

        private void Planet_OnDestroy(object sender, System.EventArgs e)
        {
            splashEffect.Mass = Mass / 10;
            splashEffect.CastParticles();
        }

        public override void Render(DeviceContext context)
        {
            base.Render(context);

            using var brushA = new SolidColorBrush(context, Color.Red);
            context.Transform = (Matrix3x2)Matrix.Identity;
            if (positions.Count > 0)
            {
                lock (positions)
                {
                    var temp = positions[0];
                    foreach (Vector2 vector2 in positions)
                    {
                        context.DrawLine(temp, vector2, brushA);
                        temp = vector2;
                    }
                }
            }
        }
    }
}