using HexaEngine.Core;
using HexaEngine.Core.Extentions;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System.Collections.Generic;

namespace GameAssets
{
    public class Wall : BaseObject, IPhysicsObject, IDrawable
    {
        public float Mass { get; set; } = 100;

        public Vector3 Velocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 Force { get; set; }

        public BlockedDirection Sides { get; set; }

        public float ForceAbsorbtion { get; set; }

        public bool Static { get; set; }

        public Vector3 MassCenter { get; set; }

        public Vector3 RotationVelocity { get; set; }

        public Vector3 RotationAcceleration { get; set; }

        public Wall(Engine engine, Bitmap1 bitmap, RawVector3 position)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            SetPosition(position);
            MassCenter = BoundingBox.Center;
        }
    }
}