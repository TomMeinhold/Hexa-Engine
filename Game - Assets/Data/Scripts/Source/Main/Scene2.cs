using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Scenes;
using SharpDX;

namespace Main
{
    public class Scene2 : Scene
    {
        public Scene2()
        {
            RayCastDiscription whiteCastDiscription = new RayCastDiscription
            {
                RayRange = 2000,
                RayDensity = 1,
                RayColor = new Color(200, 200, 200, 100)
            };

            PhysicsObjectDiscription sunPhysicsObjectS = new PhysicsObjectDiscription
            {
                Mass = 10000,
                Position = new Vector3(900, 450, 0),
            };

            PhysicsObjectDiscription planetPhysicsObject = new PhysicsObjectDiscription
            {
                Mass = 10,
                Velocity = new Vector3(0, -250, 0),
                Position = new Vector3(600, 466, 0)
            };

            PhysicsObjectDiscription planetPhysicsObject1 = new PhysicsObjectDiscription
            {
                Mass = 10,
                Velocity = new Vector3(0, 275, 0),
                Position = new Vector3(750, 466, 0)
            };

            Add(new Sun(RessourceManager.GetSprite("sun"), sunPhysicsObjectS, whiteCastDiscription));
            Add(new Planet(RessourceManager.GetSprite("planet"), planetPhysicsObject));
            Add(new Planet(RessourceManager.GetSprite("planet"), planetPhysicsObject1));
        }
    }
}