using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IRayCasting
    {
        public float RayDensity { get; set; }

        public List<Ray> Rays { get; set; }

        public float RayRange { get; set; }

        public float StartAngle { get; set; }

        public float EndAngle { get; set; }
    }
}