using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Extentions
{
    public static class BoundBoxExtentions
    {
        public static bool ContainsVector(this BoundingBox box, Vector3 vector) => box.Minimum.X < vector.X && vector.X < box.Maximum.X && (box.Minimum.Y < (vector.Y + box.Height) && (vector.Y + box.Height) < box.Maximum.Y);

        public static RectangleF BoundingBoxToRect(this BoundingBox box) => new RectangleF(box.Minimum.X, box.Minimum.Y, box.Width, box.Height);

        public static RectangleF BoundingBoxToRectNoPos(this BoundingBox box) => new RectangleF(0, 0, box.Width, box.Height);
    }
}