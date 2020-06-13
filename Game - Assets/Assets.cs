using GameAssets.Objects.World;
using HexaEngine.Core;
using HexaEngine.Core.Extentions;
using HexaEngine.Core.Scenes;
using HexaEngine.Core.UI.BaseTypes;
using SharpDX;
using SharpDX.Mathematics.Interop;
using System.Collections.Generic;
using Color = System.Drawing.Color;

namespace GameAssets
{
    public static class Assets
    {
        public static List<Scene> GetScenes(Engine engine)
        {
            EngineTransform transform = engine.Transform;
            var scenes = new List<Scene>();
            var scene1 = new Scene();

            scene1.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = 300, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 4, StartAngle = 157, EndAngle = 203, GlowColor = Color.FromArgb(255, 255, 0, 0).Convert() });
            scene1.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = 500, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 4, StartAngle = 180, EndAngle = 225, GlowColor = Color.FromArgb(255, 0, 255, 0).Convert() });
            scene1.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 600, Y = 100, Z = 0 }) { Mass = 0.1F, Static = true, RayRange = 1000, RayDensity = 4, StartAngle = 135, EndAngle = 180, GlowColor = Color.FromArgb(255, 0, 0, 255).Convert() });
            scene1.Add(new WallReflective(engine, transform.ConvertBitmap(Resource.mirror), new RawVector3() { X = 200, Y = 300, Z = 0 }) { Mass = 0.1F, Static = true });
            scene1.Add(new WallReflective(engine, transform.ConvertBitmap(Resource.mirror), new RawVector3() { X = 300, Y = 400, Z = 0 }) { Mass = 0.1F, Static = true });
            scenes.Add(scene1);

            var scene2 = new Scene();
            scene2.Add(new Sun(engine, transform.ConvertBitmap(Resource.sun, true), new RawVector3() { X = 900, Y = 300, Z = 0 }) { Mass = 10000, Static = false, RayRange = 1000, RayDensity = 5, GlowColor = Color.FromArgb(100, 200, 200, 200).Convert(), });
            scene2.Add(new Planet(engine, transform.ConvertBitmap(Resource.planet), new RawVector3() { X = 200, Y = 316, Z = 0 }) { Mass = 1, Static = false });
            scene2.Add(new WallReflective(engine, transform.ConvertBitmap(Resource.high_wall), new RawVector3() { X = 1000, Y = 0, Z = 0 }) { Mass = 0, Static = true });
            scenes.Add(scene2);

            var scene3 = new Scene();
            scene3.Add(new Planet(engine, transform.ConvertBitmap(Resource.planet), new RawVector3() { X = 900, Y = 300, Z = 0 }) { Mass = 1, Static = false, RotationVelocity = new Vector3(0, 0, 1F) });
            scenes.Add(scene3);

            var screen = new Screen();
            screen.UserInterfaces.Add(new Button(engine, new Size2F(60, 20), new Vector3(0, 50, 0)) { Content = "Scene 1", });
            screen.UserInterfaces.Add(new Button(engine, new Size2F(60, 20), new Vector3(60, 50, 0)) { Content = "Scene 2", });
            screen.UserInterfaces.Add(new Button(engine, new Size2F(60, 20), new Vector3(120, 50, 0)) { Content = "Scene 3", });

            engine.UIManager.ActiveUserInterface = screen;

            return scenes;
        }
    }
}