using GameAssets.Objects.World;
using HexaEngine.Core;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Scenes;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace GameAssets.Scenes
{
    public class Scene1 : Scene
    {
        public Scene1(Engine engine)
        {
            Bitmap1 sunBitmap = engine.RenderSystem.RessouceManager.LoadTexture(Resource.sun, true);
            Bitmap1 mirrorBitmap = engine.RenderSystem.RessouceManager.LoadTexture(Resource.mirror);

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
            RayCastDiscription redCastDiscription = new RayCastDiscription
            {
                RayRange = 2000,
                RayDensity = 4,
                StartAngle = 135,
                EndAngle = 225,
                RayColor = new Color(255, 0, 0, 255)
            };

            RayCastDiscription greenCastDiscription = new RayCastDiscription
            {
                RayRange = 2000,
                RayDensity = 4,
                StartAngle = 135,
                EndAngle = 225,
                RayColor = new Color(0, 255, 0, 255)
            };

            RayCastDiscription blueCastDiscription = new RayCastDiscription
            {
                RayRange = 2000,
                RayDensity = 4,
                StartAngle = 135,
                EndAngle = 225,
                RayColor = new Color(0, 0, 255, 255)
            };

            Add(new Sun(engine, sunBitmap, new RawVector3() { X = 500, Y = 300, Z = 0 }, sunPhysicsObject, redCastDiscription));
            Add(new Sun(engine, sunBitmap, new RawVector3() { X = 500, Y = 900, Z = 0 }, sunPhysicsObject, blueCastDiscription));
            Add(new Sun(engine, sunBitmap, new RawVector3() { X = 500, Y = 600, Z = 0 }, sunPhysicsObject, greenCastDiscription));

            Add(new WallReflective(engine, mirrorBitmap, new RawVector3() { X = 0, Y = 0, Z = 0 }, wallPhysicsObject));
        }
    }
}