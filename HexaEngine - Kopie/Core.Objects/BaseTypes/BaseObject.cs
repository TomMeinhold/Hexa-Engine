using HexaEngine.Core.Extensions;
using HexaEngine.Core.Mathmatics;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using HexaEngine.Core.Ressources;
using SharpDX;
using SharpDX.Direct2D1;
using System;

namespace HexaEngine.Core.Objects.BaseTypes
{
    public class BaseObject : IBaseObject, IDrawable, IPhysicsObject
    {
        private bool enabled;

        public Sprite Sprite { get; set; }

        public Vector3 Position { get; private set; }

        public Vector3 Rotation { get; private set; }

        public Vector3 Scale { get; private set; }

        public Size2F Size { get; set; }

        public Matrix3x2 TranslationMatrix { get; set; } = Matrix.Translation(0, 0, 0);

        public Matrix3x2 RotationMatrix { get; set; } = Matrix.RotationYawPitchRoll(0, 0, 0);

        public Matrix3x2 ScaleMatrix { get; set; } = Matrix.Scaling(new Vector3(1, 1, 1));

        public Matrix3x2 ObjectViewMatrix { get; set; } = Matrix.Translation(0, 0, 0);

        public BoundingBox BoundingBox { get; set; }

        public float Mass { get; set; }

        public Vector3 MassCenter { get; set; }

        public bool Static { get; set; }

        public Vector3 Velocity { get; set; }

        public Vector3 RotationVelocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 RotationAcceleration { get; set; }

        public Vector3 Force { get; set; }

        public float ForceAbsorbtion { get; set; }

        public BlockedDirection Sides { get; set; }

        public bool Enabled
        {
            get => enabled;
            set
            {
                if (value)
                {
                    Activate?.Invoke(this, null);
                }
                else
                {
                    Disable?.Invoke(this, null);
                }
                enabled = value;
            }
        }

        public Vector3 PositionBefore { get; set; }
        public BoundingBox BoundingBoxBefore { get; set; }

        public event EventHandler<OnCollisionEventArgs> OnCollision;

        public event EventHandler Activate;

        public event EventHandler Disable;

        public event EventHandler OnDestroy;

        public virtual void SetPosition(Vector3 vector)
        {
            BoundingBoxBefore = BoundingBox;
            PositionBefore = Position;
            Position = vector;
            Vector3 max = new Vector3(vector.X + Size.Width, vector.Y + Size.Height, vector.Z);
            BoundingBox = new BoundingBox(vector, max);
            TranslationMatrix = Matrix.Translation(Position);
            ObjectViewMatrix = ScaleMatrix * RotationMatrix * TranslationMatrix;
        }

        public virtual void SetRotation(Vector3 vector)
        {
            if (this is IPhysicsObject physicsObject)
            {
                RotationMatrix = MatrixCalculation.RotateAt(vector.Z.ToRadians(), physicsObject.MassCenter);
            }
            else
            {
                RotationMatrix = Matrix.RotationYawPitchRoll(vector.X, vector.Y, vector.Z);
            }

            Rotation = vector;
            ObjectViewMatrix = ScaleMatrix * RotationMatrix * TranslationMatrix;
        }

        public virtual void SetScale(Vector3 vector)
        {
            ScaleMatrix = Matrix.Scaling(vector);
            ObjectViewMatrix = ScaleMatrix * RotationMatrix * TranslationMatrix;
        }

        public virtual void Render(DeviceContext context)
        {
            if (this is IRayCasting rayCasting)
            {
                rayCasting.RayCastingModule.Render(context);
            }

            context.Transform = ObjectViewMatrix;
            context.Target = Engine.Current.RenderSystem.DriectXManager.ObjectsBitmap;
            context.DrawBitmap(Sprite, 1, BitmapInterpolationMode.Linear);
            context.Transform = (Matrix3x2)Matrix.Identity;
            if (Engine.Current.Settings.DebugMode)
            {
                DrawPhysicsDebugInfo(context);
            }
        }

        private void DrawPhysicsDebugInfo(DeviceContext deviceContext)
        {
            using var brushA = new SolidColorBrush(deviceContext, Color.Red);
            using var brushV = new SolidColorBrush(deviceContext, Color.LightBlue);
            using var brushF = new SolidColorBrush(deviceContext, Color.DarkGreen);
            deviceContext.Transform = (Matrix3x2)Matrix.Translation(Position);

            var ep = new Ellipse(MassCenter.Downgrade(), Mass % Size.Width * 2, Mass % Size.Width * 2);
            deviceContext.FillEllipse(ep, brushF);

            deviceContext.DrawLine(new Vector2(MassCenter.X + 10, MassCenter.Y), new Vector2(MassCenter.X - 10, MassCenter.Y), brushA);
            deviceContext.DrawLine(new Vector2(MassCenter.X, MassCenter.Y + 10), new Vector2(MassCenter.X, MassCenter.Y - 10), brushA);

            deviceContext.DrawLine(new Vector2(MassCenter.X, MassCenter.Y), new Vector2(MassCenter.X + Acceleration.X, MassCenter.Y + Acceleration.Y), brushA);
            deviceContext.DrawLine(new Vector2(MassCenter.X, MassCenter.Y), new Vector2(MassCenter.X + Velocity.X, MassCenter.Y + Velocity.Y), brushV);

            deviceContext.DrawRectangle(BoundingBox.BoundingBoxToRectNoPos(), brushA);
        }

        public void CallOnCollision(OnCollisionEventArgs onCollisionEventArgs)
        {
            OnCollision?.Invoke(this, onCollisionEventArgs);
        }

        public virtual void Destroy()
        {
            OnDestroy?.Invoke(this, null);
            Engine.Current.SceneManager.SelectedScene.Remove(this);
        }
    }
}