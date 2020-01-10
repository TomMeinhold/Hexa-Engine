using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Objects
{
    public enum ObjectSystemState : short
    {
        Uninitialized = 0,
        Loaddata = 1,
        Waiting = 2,
        Initialized = 3,
        Failed = 4,
        Active = 5
    }
}
