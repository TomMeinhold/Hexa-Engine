using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System.Threading;

namespace HexaEngine.Core.Common
{
    public class BaseObject
    {
        public BaseObjectState State = BaseObjectState.Uninitialized;

        public string Name;

        public Bitmap Bitmap;

        public bool Collision;

        public bool Moveable;

        public bool Static;

        public bool CameraFocus;

        /// <summary>
        /// 0: Y Down
        /// 1: Y Up
        /// 2: X Right
        /// 3: X Left
        /// </summary>
        public bool[] DirectionBlocked = new bool[4];

        public Size2F Size;

        public Size2F Hitbox;

        public RectangleF Dimentions;

        public RawVector3 Position;

        public RawVector3 Acceleration;

        public RawVector3 MovementAcceleration;

        public RawVector3 Gravity;

        public RawVector3 Damper;

        public RawVector3 NaturalDeceleration;

        public RawVector3 MaxSpeed;

        public RawVector3 Speed = new RawVector3();

        public Engine Engine
        {
            set
            {
                engine = value;
                Engine.ObjectSystem.ObjectList.Add(this);
            }
            get => engine;
        }

        private Engine engine;

        public RawVector2Buffer Buffer = new RawVector2Buffer();

        public Thread Thread;

        public void Dispose()
        {
            Thread.Abort();
            Bitmap.Dispose();
        }

        public void Draw()
        {
            if (State == BaseObjectState.Initialized)
            {
                RenderTarget target = Engine.RenderTarget;
                RawVector2[] raws = Buffer.GetBuffer();
                RectangleF rectangle = new RectangleF
                {
                    Size = Bitmap.Size,
                    X = raws[0].X,
                    Y = raws[0].Y
                };
                target.DrawBitmap(Bitmap, rectangle, 1, BitmapInterpolationMode.Linear, Dimentions);
            }
        }
    }
}
