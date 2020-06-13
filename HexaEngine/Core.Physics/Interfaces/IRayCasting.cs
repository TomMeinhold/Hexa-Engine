using HexaEngine.Core.Objects.Components;
using HexaEngine.Core.Objects.Interfaces;
using SharpDX;
using System.Collections.Generic;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IRayCasting : IBaseObject
    {
        public float RayDensity { get; set; }

        public List<Ray> Rays { get; set; }

        public float RayRange { get; set; }

        public float StartAngle { get; set; }

        public float EndAngle { get; set; }

        public bool RaysEnabled { get; set; }

        Color4 GlowColor { get; set; }

        public RayCastingModule RayCastingModule { get; }
    }
}