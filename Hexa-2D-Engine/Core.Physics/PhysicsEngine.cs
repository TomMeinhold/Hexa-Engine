using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine
    {
        private readonly Engine Engine;

        public PhysicsEngine(Engine engine)
        {
            Engine = engine;
            MainThread = new Thread(ProcessingPhysics);
            MainThread.Start();
        }

        public void Dispose()
        {
            MainThread.Abort();
        }
    }
}
