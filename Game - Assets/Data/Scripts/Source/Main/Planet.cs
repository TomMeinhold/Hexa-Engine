using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Modules;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Particle;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Render.Interfaces;
using HexaEngine.Core.Ressources;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace Main
{
    public class Planet : BaseObject, IBaseObject, IDrawable, IPhysicsObject
    {
        private readonly KeyboardController keyboardController;

        private readonly SplashEffect<Particle1> splashEffect;

        private Planet()
        {
            keyboardController = new KeyboardController(this);
            keyboardController.KeyUp += KeyboardController_KeyUp;
            splashEffect = new SplashEffect<Particle1>(this, 100, new System.TimeSpan(0, 0, 2));
            OnCollision += Planet_OnCollision;
            OnDestroy += Planet_OnDestroy;
        }

        public Planet(Sprite sprite, PhysicsObjectDiscription physicsObjectDiscription) : this()
        {
            Sprite = sprite;
            Size = sprite.Size;
            physicsObjectDiscription.SetValues(this);
            MassCenter = BoundingBox.Center - Position;
        }

        public Planet(Sprite sprite, RawVector3 position, PhysicsObjectDiscription physicsObjectDiscription) : this()
        {
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
    }
}