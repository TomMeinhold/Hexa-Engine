using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IRayLens : IPhysicsObject
    {
        float Multiplier { get; set; }
    }
}