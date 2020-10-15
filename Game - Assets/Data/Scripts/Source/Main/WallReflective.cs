using HexaEngine.Core;
using HexaEngine.Core.Extensions;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Render.Interfaces;
using HexaEngine.Core.Ressources;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace Main
{
    public class WallReflective : BaseObject, IBaseObject, IDrawable, IPhysicsObject, IRayMirror
    {
        public float ReflectionStrength { get; set; } = 2000;

        public Color ReflectionColor { get; set; } = System.Drawing.Color.FromArgb(255, 255, 255, 255).Convert();

        public bool Top { get; set; }

        public bool Right { get; set; } = true;

        public bool Left { get; set; } = true;

        public bool Bottom { get; set; }

        public WallReflective(Engine engine, Sprite sprite, PhysicsObjectDiscription physicsObjectDiscription)
        {
            Engine = engine;
            Sprite = sprite;
            Size = sprite.Size;
            physicsObjectDiscription.SetValues(this);
            MassCenter = BoundingBox.Center;
        }

        public WallReflective(Engine engine, Sprite sprite, RawVector3 position, PhysicsObjectDiscription physicsObjectDiscription)
        {
            Engine = engine;
            Sprite = sprite;
            Size = sprite.Size;
            physicsObjectDiscription.SetValues(this);
            SetPosition(position);
            MassCenter = BoundingBox.Center;
        }
    }
}