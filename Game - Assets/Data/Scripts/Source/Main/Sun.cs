using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Modules;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Components;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Render.Interfaces;
using HexaEngine.Core.Ressources;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace Main
{
    public class Sun : BaseObject, IBaseObject, IDrawable, IPhysicsObject, IRayCasting
    {
        private bool rotateRaysMode;

        private RayCastDiscription rayCastDiscription;

        public RayCastDiscription RayCastDiscription { get => rayCastDiscription; set => rayCastDiscription = value; }

        public MouseController mouseController;

        public KeyboardController keyboardController;

        private Sun()
        {
            mouseController = new MouseController(this);
            keyboardController = new KeyboardController(this, mouseController);
            mouseController.MouseHover += MouseController_MouseHover;
            keyboardController.KeyUp += KeyboardController_KeyUp;
        }

        public Sun(Sprite sprite, RawVector3 position, PhysicsObjectDiscription physicsObjectDiscription) : this()
        {
            Sprite = sprite;
            Size = sprite.Size;
            physicsObjectDiscription.SetValues(this);
            SetPosition(position);
            MassCenter = BoundingBox.Center - Position;
            OnCollision += Sun_OnCollision;
        }

        public Sun(Sprite sprite, PhysicsObjectDiscription physicsObjectDiscription) : this()
        {
            Sprite = sprite;
            Size = sprite.Size;
            physicsObjectDiscription.SetValues(this);
            MassCenter = BoundingBox.Center - Position;
            OnCollision += Sun_OnCollision;
        }

        public Sun(Sprite sprite, PhysicsObjectDiscription physicsObjectDiscription, RayCastDiscription rayCastDiscription) : this(sprite, physicsObjectDiscription)
        {
            RayCastingModule = new RayCastingModule(this);
            this.RayCastDiscription = rayCastDiscription;
        }

        public Sun(Sprite sprite, RawVector3 position, PhysicsObjectDiscription physicsObjectDiscription, RayCastDiscription rayCastDiscription) : this(sprite, position, physicsObjectDiscription)
        {
            RayCastingModule = new RayCastingModule(this);
            this.RayCastDiscription = rayCastDiscription;
        }

        public RayCastingModule RayCastingModule { get; set; }

        private void MouseController_MouseHover(object sender, MouseUpdatePackage e)
        {
            if (mouseController.IsMouseDown)
            {
                if (rotateRaysMode)
                {
                    rayCastDiscription.StartAngle = (rayCastDiscription.StartAngle + e.MouseUpdate.Location.X) % 360;
                    rayCastDiscription.EndAngle = (rayCastDiscription.EndAngle + e.MouseUpdate.Location.X) % 360;
                }
                else
                {
                    Vector3 oldPos = Position;
                    oldPos += e.MouseUpdate.Location;
                    SetPosition(oldPos);
                }
            }
        }

        private void KeyboardController_KeyUp(object sender, KeyboardUpdatePackage e)
        {
            if (e.KeyboardUpdate.Key == Keys.R)
            {
                rotateRaysMode = !rotateRaysMode;
            }

            if (e.KeyboardUpdate.Key == Keys.L)
            {
                rayCastDiscription.RaysEnabled = !rayCastDiscription.RaysEnabled;
            }

            if (e.KeyboardUpdate.Key == Keys.M)
            {
                Destroy();
            }
        }

        private void Sun_OnCollision(object sender, HexaEngine.Core.Physics.Collision.OnCollisionEventArgs e)
        {
            if (e.Object is Planet planet)
            {
                Mass -= planet.Mass * (planet.Velocity.X + planet.Velocity.Y);
            }
        }
    }
}