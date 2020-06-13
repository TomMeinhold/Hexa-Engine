using SharpDX;
using System.Collections.Generic;
using System.Linq;

namespace HexaEngine.Core.Physics.Rays
{
    public class RayBuffer
    {
        private readonly List<Ray> rays;

        public RayBuffer()
        {
            rays = new List<Ray>();
        }

        public RayBuffer(List<Ray> rays)
        {
            if (rays is null)
            {
                this.rays = new List<Ray>();
            }
            else
            {
                this.rays = rays.ToList();
            }
        }

        public int Pointer { get; set; }

        public void Add(Ray ray)
        {
            if (rays.Count < Pointer + 1)
            {
                rays.Add(ray);
            }
            else
            {
                rays[Pointer] = ray;
            }

            Pointer++;
        }

        public List<Ray> GetRays()
        {
            return rays.GetRange(0, Pointer);
        }
    }
}