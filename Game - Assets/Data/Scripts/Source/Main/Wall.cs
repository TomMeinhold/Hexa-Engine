using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Render.Interfaces;
using HexaEngine.Core.Ressources;
using SharpDX;

namespace Main
{
    public class Wall : BaseObject, IBaseObject, IDrawable, IPhysicsObject
    {
        public Wall(Sprite sprite, Vector3 position, PhysicsObjectDiscription physicsObjectDiscription)
        {
            Sprite = sprite;
            Size = Sprite.Size;
            physicsObjectDiscription.SetValues(this);
            SetPosition(position);
            MassCenter = BoundingBox.Center;
        }
    }
}