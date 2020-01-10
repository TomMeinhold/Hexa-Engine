using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Physics
{
    public enum PhysicsEngineState : short
    {
        Uninitialized = 0,
        Loaddata = 1,
        Waiting = 2,
        Initialized = 3,
        Failed = 4,
        Active = 5
    }

    public enum PhysicsEngineInternalState
    {
        SetStateWaiting = 0,
        SetStateActive = 1
    }
}
