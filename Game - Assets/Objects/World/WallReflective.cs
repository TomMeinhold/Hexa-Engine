using HexaEngine.Core;
using HexaEngine.Core.Extentions;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace GameAssets
{
    public class WallReflective : BaseObject, IBaseObject, IDrawable, IPhysicsObject, IRayMirror
    {
        public float Mass { get; set; } = 100;

        public Vector3 Velocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 Force { get; set; }

        public BlockedDirection Sides { get; set; }

        public float ForceAbsorbtion { get; set; }

        public float ReflectionStrength { get; set; } = 1000;

        public Color ReflectionColor { get; set; } = System.Drawing.Color.FromArgb(255, 255, 255, 255).Convert();

        public bool Static { get; set; }

        public float Multiplier { get; set; } = 0.1F;

        public bool Top { get; set; }

        public bool Right { get; set; } = true;

        public bool Left { get; set; } = true;

        public bool Bottom { get; set; }

        public Vector3 MassCenter { get; set; }

        public Vector3 RotationVelocity { get; set; }

        public Vector3 RotationAcceleration { get; set; }

        public WallReflective(Engine engine, Bitmap1 bitmap, RawVector3 position)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            SetPosition(position);
            MassCenter = BoundingBox.Center;
        }
    }
}