using GameAssets.Objects.World;
using HexaEngine.Core;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Scenes;
using SharpDX;
using SharpDX.Direct2D1;

namespace GameAssets.Scenes
{
    public class Scene2 : Scene
    {
        public Scene2(Engine engine)
        {
            RayCastDiscription whiteCastDiscription = new RayCastDiscription
            {
                RayRange = 2000,
                RayDensity = 5,
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

            Bitmap1 sunBitmap = engine.RenderSystem.RessouceManager.LoadTexture(Resource.sun, true);
            Bitmap1 planetBitmap = engine.RenderSystem.RessouceManager.LoadTexture(Resource.planet);
            Add(new Sun(engine, sunBitmap, sunPhysicsObjectS, whiteCastDiscription));
            Add(new Planet(engine, planetBitmap, planetPhysicsObject));
            Add(new Planet(engine, planetBitmap, planetPhysicsObject1));
        }
    }
}