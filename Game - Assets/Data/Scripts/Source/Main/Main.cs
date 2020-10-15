using HexaEngine.Core;
using HexaEngine.Core.Ressources;
using System;

namespace Main
{
    public static class Program
    {
        private static Engine Engine;

        public static void Main(Engine engine)
        {
            Engine = engine;
            engine.LoadRessources += Engine_LoadRessources;
            engine.RenderSystem.EnterMainLoop += RenderSystem_EnterMainLoop;
        }

        private static void Engine_LoadRessources(object sender, Engine e)
        {
            Sprite.Load(e, TimeSpan.Zero, "sun.bmp");
            Sprite.Load(e, TimeSpan.Zero, "mirror.bmp");
            Sprite.Load(e, TimeSpan.Zero, "planet.bmp");
            Sprite.Load(e, TimeSpan.Zero, "wall.bmp");
            Sprite.Load(e, TimeSpan.Zero, "large_wall.bmp");
        }

        private static void RenderSystem_EnterMainLoop(object sender, System.EventArgs e)
        {
            Engine.UIManager.SetUIByType(typeof(Screen1));
            Engine.SceneManager.SetSceneByType(typeof(Scene1));
        }
    }
}