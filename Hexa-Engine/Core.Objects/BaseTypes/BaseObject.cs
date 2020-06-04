using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace HexaEngine.Core.Objects.BaseTypes
{
    public class BaseObject
    {
        public BaseObjectState State = BaseObjectState.Uninitialized;

        public string Name { get; set; }

        public Bitmap1 Bitmap { get; set; }

        public BoundingBox BoundingBox { get; set; }

        public RawVector3 Position { get; private set; }

        public Engine Engine { set; get; }

        public bool IsVisible { get; private set; } = true;

        public Size2F Size { get; set; }

        public virtual void SetPosition(Vector3 vector3)
        {
            this.Position = vector3;
            Vector3 max = new Vector3(vector3.X + Size.Width, vector3.Y + Size.Height, vector3.Z);
            BoundingBox = new BoundingBox(vector3, max);
        }

        public virtual void SetRotation(float angle)
        {
            var m = Matrix.RotationYawPitchRoll(0, angle, 0);
            Vector3 max = new Vector3(Position.X + Size.Width, Position.Y + Size.Height, Position.Z);
            this.Position = Vector3.TransformCoordinate(Position, m);
            BoundingBox = new BoundingBox(Vector3.TransformCoordinate(Position, m), Vector3.TransformCoordinate(max, m));
        }

        public void Dispose()
        {
        }

        public void DrawOnTarget(DeviceContext deviceContext)
        {
            this.Draw(deviceContext);
        }

        public virtual void Draw(DeviceContext deviceContext)
        {
            if (deviceContext != null)
            {
                deviceContext.Transform = (Matrix3x2)Matrix.Translation(Position.X, Position.Y * -1, Position.Z);
                deviceContext.DrawBitmap(Bitmap, 1, BitmapInterpolationMode.Linear);
                deviceContext.Transform = (Matrix3x2)Matrix.Identity;
            }
        }
    }
}