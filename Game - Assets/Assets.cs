using GameAssets.Objects.World;
using HexaEngine.Core;
using HexaEngine.Core.Extentions;
using HexaEngine.Core.Scenes;
using SharpDX.Mathematics.Interop;
using System.Collections.Generic;
using System.Drawing;

namespace GameAssets
{
    public static class Assets
    {
        public static List<Scene> GetScenes(Engine engine)
        {
            EngineTransform transform = engine.Transform;
            var scenes = new List<Scene>();
            var scene1 = new Scene();

            scene1.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -300, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 4, StartAngle = 157, EndAngle = 203, GlowColor = Color.FromArgb(255, 255, 0, 0).Convert() });
            scene1.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -500, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 4, StartAngle = 135, EndAngle = 180, GlowColor = Color.FromArgb(255, 0, 255, 0).Convert() });
            scene1.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -100, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 4, StartAngle = 180, EndAngle = 225, GlowColor = Color.FromArgb(255, 0, 0, 255).Convert() });

            scene1.Add(new WallReflective(engine, transform.ConvertBitmap(Resource.mirror), new RawVector3() { X = 200, Y = -300, Z = 0 }) { Mass = 0.1F, Static = true });
            scene1.Add(new WallReflective(engine, transform.ConvertBitmap(Resource.mirror), new RawVector3() { X = 800, Y = -400, Z = 0 }) { Mass = 0.1F, Static = true });
            scenes.Add(scene1);

            var scene2 = new Scene();
            scene2.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 900, Y = -300, Z = 0 }) { Mass = 1, Static = true, RayRange = 1000, RayDensity = 5, GlowColor = Color.FromArgb(100, 200, 200, 200).Convert(), });
            scene2.Add(new Planet(engine, transform.ConvertBitmap(Resource.planet), new RawVector3() { X = 200, Y = -284, Z = 0 }) { Mass = 1, Static = false });
            scenes.Add(scene2);

            var scene3 = new Scene();
            scene3.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -300, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 3, StartAngle = 157, EndAngle = 203, GlowColor = Color.FromArgb(255, 255, 0, 0).Convert() });
            scene3.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -500, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 3, StartAngle = 135, EndAngle = 180, GlowColor = Color.FromArgb(255, 0, 255, 0).Convert() });
            scene3.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -100, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 3, StartAngle = 180, EndAngle = 225, GlowColor = Color.FromArgb(255, 0, 0, 255).Convert() });
            scene3.Add(new Wall(engine, transform.ConvertBitmap(Resource.player), new RawVector3() { X = 200, Y = -300, Z = 0 }) { Mass = 0.1F, Static = true });
            scenes.Add(scene3);

            var scene4 = new Scene();
            scene4.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -300, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 3, StartAngle = 157, EndAngle = 203, GlowColor = Color.FromArgb(255, 255, 0, 0).Convert() });
            scene4.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -500, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 3, StartAngle = 135, EndAngle = 180, GlowColor = Color.FromArgb(255, 0, 255, 0).Convert() });
            scene4.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = -100, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 3, StartAngle = 180, EndAngle = 225, GlowColor = Color.FromArgb(255, 0, 0, 255).Convert() });
            scene4.Add(new Wall(engine, transform.ConvertBitmap(Resource.player), new RawVector3() { X = 200, Y = -300, Z = 0 }) { Mass = 0.1F, Static = true });
            scenes.Add(scene4);

            return scenes;
        }
    }
}