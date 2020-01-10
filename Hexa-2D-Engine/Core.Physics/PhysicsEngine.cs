using System.Threading;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine
    {
        private readonly Engine Engine;

        public float Max_Speed_X;

        public float Max_Speed_Y;

        public PhysicsEngineState State;

        private PhysicsEngineInternalState InternalState;

        public PhysicsEngine(Engine engine)
        {
            State = PhysicsEngineState.Initialized;
            Engine = engine;
            MainThread = new Thread(ProcessingPhysics);
            MainThread.Start();
        }

        public void SetState(PhysicsEngineState state)
        {
            switch (state)
            {
                case PhysicsEngineState.Waiting:
                    InternalState = PhysicsEngineInternalState.SetStateWaiting;
                    break;
                case PhysicsEngineState.Active:
                    InternalState = PhysicsEngineInternalState.SetStateActive;
                    break;
            }
        }

        public void Dispose()
        {
            MainThread.Abort();
        }
    }
}
