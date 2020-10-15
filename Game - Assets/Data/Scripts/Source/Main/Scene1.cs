using HexaEngine.Core;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Ressources;
using HexaEngine.Core.Scenes;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace Main
{
    public class Scene1 : Scene
    {
        public Scene1(Engine engine)
        {
            PhysicsObjectDiscription sunPhysicsObject = new PhysicsObjectDiscription
            {
                Mass = 0,
                Static = true
            };

            PhysicsObjectDiscription wallPhysicsObject = new PhysicsObjectDiscription
            {
                Mass = 100,
                Static = true
            };

            // Create parameters for raytracing.
            RayCastDiscription blueCastDiscription = new RayCastDiscription
            {
                RayRange = 2000,
                RayDensity = 1,
                StartAngle = 45,
                EndAngle = 135,
                RayColor = new Color(255, 255, 255, 255)
            };

            Add(new Sun(engine, RessouceManager.GetSprite("sun"), new RawVector3() { X = 500, Y = 269, Z = 0 }, sunPhysicsObject, blueCastDiscription));
            Add(new Wall(engine, RessouceManager.GetSprite("large_wall"), new RawVector3() { X = 0, Y = 200, Z = 0 }, wallPhysicsObject));
            Add(new Wall(engine, RessouceManager.GetSprite("large_wall"), new RawVector3() { X = 0, Y = 600, Z = 0 }, wallPhysicsObject));
        }
    }
}