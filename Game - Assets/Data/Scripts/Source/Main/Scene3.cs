using HexaEngine.Core;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Scenes;
using SharpDX;

namespace Main
{
    public class Scene3 : Scene
    {
        public Scene3(Engine engine)
        {
            PhysicsObjectDiscription walle = new PhysicsObjectDiscription
            {
                Mass = 1000,
                Static = true
            };
            Add(new Planet(engine, RessouceManager.GetSprite("planet"), new Vector3(100, 100, 0), walle));

            PhysicsObjectDiscription planetPhysicsObject = new PhysicsObjectDiscription
            {
                Mass = 1000,
                Velocity = new Vector3(0, -1000000, 0),
            };
            Add(new Planet(engine, RessouceManager.GetSprite("wall"), new Vector3(100, 400, 0), planetPhysicsObject));
        }
    }
}