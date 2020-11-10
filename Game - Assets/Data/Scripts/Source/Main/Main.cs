using HexaEngine.Core;
using HexaEngine.Core.Ressources;
using System;
using System.Net;

namespace Main
{
    public static class Program
    {
        public static void Main()
        {
            Engine.Current.LoadRessources += Engine_LoadRessources;
            Engine.Current.StartServer += Engine_StartServer;
            if (Engine.Current.RenderSystem != null)
            {
                Engine.Current.RenderSystem.EnterMainLoop += RenderSystem_EnterMainLoop;
            }
        }

        private static void Engine_StartServer(object sender, Engine e)
        {
            e.Server.Start(IPAddress.Parse("127.0.0.1"), 8080);
            e.Server.ReceivedPackage += Server_ReceivedPackage;
            e.Server.ServerStarted += Server_ServerStarted;
            e.Server.ClientConnected += Server_ClientConnected;
            e.Server.ClientDisconnected += Server_ClientDisconnected;
        }

        private static void Server_ClientDisconnected(object sender, HexaEngine.Core.Network.Components.SocketHandler e)
        {
        }

        private static void Server_ClientConnected(object sender, HexaEngine.Core.Network.Components.SocketHandler e)
        {
        }

        private static void Server_ServerStarted(object sender, HexaEngine.Core.Network.Server e)
        {
            Console.WriteLine("Server started");
        }

        private static void Server_ReceivedPackage(object sender, HexaEngine.Core.Network.Structs.Package e)
        {
        }

        private static void Engine_LoadRessources(object sender, Engine e)
        {
            Sprite.Load(TimeSpan.Zero, "sun.bmp");
            Sprite.Load(TimeSpan.Zero, "mirror.bmp");
            Sprite.Load(TimeSpan.Zero, "planet.bmp");
            Sprite.Load(TimeSpan.Zero, "planet1.bmp");
            Sprite.Load(TimeSpan.Zero, "planet2.bmp");
            Sprite.Load(TimeSpan.Zero, "wall.bmp");
            Sprite.Load(TimeSpan.Zero, "high_wall.bmp");
            Sprite.Load(TimeSpan.Zero, "raindrops.bmp");
            Particle1.ParticleSprite = Sprite.Get("raindrops");
        }

        private static void RenderSystem_EnterMainLoop(object sender, EventArgs e)
        {
            Engine.Current.SceneManager.SetSceneByType(typeof(MainMenu));
        }
    }
}