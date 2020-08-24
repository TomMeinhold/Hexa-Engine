using GameAssets.Objects.World;
using HexaEngine.Core;
using HexaEngine.Core.Physics.Structs;
using HexaEngine.Core.Scenes;
using SharpDX;
using SharpDX.Direct2D1;

namespace GameAssets.Scenes
{
    public class Scene3 : Scene
    {
        public Scene3(Engine engine)
        {
            // Load bitmaps
            Bitmap1 planetBitmap = engine.RenderSystem.RessouceManager.LoadTexture(Resource.wall);

            PhysicsObjectDiscription walle = new PhysicsObjectDiscription
            {
                Mass = 1000,
                Static = true
            };
            Add(new Planet(engine, planetBitmap, new Vector3(100, 100, 0), walle));

            PhysicsObjectDiscription planetPhysicsObject = new PhysicsObjectDiscription
            {
                Mass = 1000,
                Velocity = new Vector3(0, -1000000, 0),
            };
            Add(new Planet(engine, planetBitmap, new Vector3(100, 400, 0), planetPhysicsObject));
        }
    }
}