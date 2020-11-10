using System;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        public event EventHandler<Engine> LoadRessources;

        public event EventHandler<Engine> StartServer;
    }
}