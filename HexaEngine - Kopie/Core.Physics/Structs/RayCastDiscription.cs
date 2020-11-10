using HexaEngine.Core.Physics.Interfaces;
using SharpDX;

namespace HexaEngine.Core.Physics.Structs
{
    public struct RayCastDiscription
    {
        private float? rayDensity;

        private float? rayRange;

        private float? startAngle;

        private float? endAngle;

        private bool? raysEnabled;

        private Color4? glowColor;

        public float RayDensity { get => rayDensity ?? 1; set => rayDensity = value; }

        public float RayRange { get => rayRange ?? 500; set => rayRange = value; }

        public float StartAngle { get => startAngle ?? 0; set => startAngle = value; }

        public float EndAngle { get => endAngle ?? 360; set => endAngle = value; }

        public bool RaysEnabled { get => raysEnabled ?? true; set => raysEnabled = value; }

        public Color4 RayColor { get => glowColor ?? Color.Transparent; set => glowColor = value; }

        /// <summary>
        /// Sets the values to the IRayCasting.
        /// </summary>
        /// <param name="rayCasting">IRayCasting.</param>
        public void SetValues(IRayCasting rayCasting)
        {
            var desc = rayCasting.RayCastDiscription;
            desc.RayDensity = RayDensity;
            desc.RayRange = RayRange;
            desc.StartAngle = StartAngle;
            desc.EndAngle = EndAngle;
            desc.RaysEnabled = RaysEnabled;
            desc.RayColor = RayColor;
            rayCasting.RayCastDiscription = desc;
        }
    }
}