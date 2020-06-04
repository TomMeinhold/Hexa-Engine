using SharpDX.Direct2D1;

namespace HexaEngine.Core.Game
{
    public class CameraBase
    {
        public CameraBase(Engine engine)
        {
            Engine = engine;
        }

        public Engine Engine { get; set; }
        public float InitialZoom { get; internal set; }

        public float X = 0;

        public float Y = 0;

        public float Z = 0;

        public bool IsLocked = false;

        public void SetPosition(float X, float Y, float Z)
        {
            if (!IsLocked)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }
        }
    }
}