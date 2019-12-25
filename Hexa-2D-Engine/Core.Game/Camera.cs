using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Game
{
    public class Camera
    {
        public Camera(RenderTarget target)
        {
            RenderTarget = target;
        }

        public RenderTarget RenderTarget { get; set; }

        public float X = 0;

        public float Y = 0;

        public float Z = 0;

        public bool IsLocked = false;

        public void SetPosition(float X, float Y ,float Z)
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
