using HexaEngine.Core;
using HexaEngine.Core.Extensions;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Render.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace GameAssets.Objects.World
{
    public class WallReflective : BaseObject, IBaseObject, IDrawable, IPhysicsObject, IRayMirror
    {
        public float ReflectionStrength { get; set; } = 2000;

        public Color ReflectionColor { get; set; } = System.Drawing.Color.FromArgb(255, 255, 255, 255).Convert();

        public bool Top { get; set; }

        public bool Right { get; set; } = true;

        public bool Left { get; set; } = true;

        public bool Bottom { get; set; }

        public WallReflective(Engine engine, Bitmap1 bitmap, PhysicsObjectDiscription physicsObjectDiscription)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            physicsObjectDiscription.SetValues(this);
            MassCenter = BoundingBox.Center;
        }

        public WallReflective(Engine engine, Bitmap1 bitmap, RawVector3 position, PhysicsObjectDiscription physicsObjectDiscription)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            physicsObjectDiscription.SetValues(this);
            SetPosition(position);
            MassCenter = BoundingBox.Center;
        }
    }
}