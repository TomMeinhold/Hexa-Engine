using HexaEngine.Core.Extentions;
using HexaEngine.Core.Mathmatics;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;

namespace HexaEngine.Core.Objects.BaseTypes
{
    public class BaseObject : IBaseObject, IDrawable
    {
        public Bitmap1 Bitmap { get; set; }

        public BoundingBox BoundingBox { get; set; }

        public Vector3 Position { get; private set; }

        public Vector3 Rotation { get; private set; }

        public Vector3 Scale { get; private set; }

        public Engine Engine { set; get; }

        public Size2F Size { get; set; }

        public Matrix3x2 TranslationMatrix { get; set; } = Matrix.Translation(0, 0, 0);

        public Matrix3x2 RotationMatrix { get; set; } = Matrix.RotationYawPitchRoll(0, 0, 0);

        public Matrix3x2 ScaleMatrix { get; set; } = Matrix.Scaling(new Vector3(1, 1, 1));

        public Matrix3x2 ObjectViewMatrix { get; set; } = Matrix.Translation(0, 0, 0);

        public virtual void SetPosition(Vector3 vector)
        {
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
            context.Target = Engine.RenderSystem.RessouceManager.ObjectsBitmap;
            context.DrawBitmap(Bitmap, 1, BitmapInterpolationMode.Linear);
            context.Transform = (Matrix3x2)Matrix.Identity;
        }
    }
}