using SharpDX;
using System;
using System.Collections.Generic;

namespace HexaEngine.Core.Physics.Rays
{
    public class RayBuffer
    {
        private readonly List<Ray> rays;

        public RayBuffer()
        {
            rays = new List<Ray>();
        }

        public RayBuffer(ref List<Ray> rays)
        {
            if (rays is null)
            {
                this.rays = new List<Ray>();
            }
            else
            {
                this.rays = rays;
            }
        }

        public int Pointer { get; set; }

        [Obsolete("Creates GC Pressure")]
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

        public Ray AddRay(Vector3 pos, Vector3 direct)
        {
            if (rays.Count < Pointer + 1)
            {
                Ray ray = new Ray(pos, direct);
                lock (rays)
                {
                    rays.Add(ray);
                }

                Pointer++;
                return ray;
            }
            else
            {
                Ray ray = rays[Pointer];
                ray.Position = pos;
                ray.Direction = direct;
                return ray;
            }
        }

        public List<Ray> GetRays()
        {
            return rays.GetRange(0, Pointer);
        }
    }
}