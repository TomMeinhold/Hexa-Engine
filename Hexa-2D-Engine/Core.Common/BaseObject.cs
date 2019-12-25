using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HexaEngine.Core.Common
{
    public class BaseObject
    {
        public Bitmap Bitmap;

        public bool Collision;

        public bool Moveable;

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

        public RawVector3 Gravity;

        public Engine Engine;

        public RawVector2Buffer Buffer = new RawVector2Buffer();

        public Thread Thread;

        public void Dispose()
        {
            Thread.Abort();
            Bitmap.Dispose();
        }
    }
}
