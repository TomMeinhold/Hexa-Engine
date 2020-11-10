using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Scenes;
using SharpDX;

namespace Main
{
    public class Scene1 : Scene
    {
        public Scene1()
        {
            RayCastDiscription whiteCastDiscription = new RayCastDiscription
            {
                RayRange = 2000,
                RayDensity = 2,
                RayColor = new Color(200, 200, 200, 100)
            };
            PhysicsObjectDiscription planetPhysicsObject = new PhysicsObjectDiscription
            {
                Mass = 100,
                Position = new Vector3(600, 400, 0),
                Colliding = true,
                Static = true
            };
            PhysicsObjectDiscription planetPhysicsObject1 = new PhysicsObjectDiscription
            {
                Mass = 100,
                Position = new Vector3(600, 600, 0),
                Colliding = true,
                Static = true
            };

            Add(new Sun(RessourceManager.GetSprite("planet"), planetPhysicsObject, whiteCastDiscription));
            Add(new Planet(RessourceManager.GetSprite("planet"), planetPhysicsObject1));
        }
    }
}