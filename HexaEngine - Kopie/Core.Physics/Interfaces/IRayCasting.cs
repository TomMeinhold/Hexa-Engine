using HexaEngine.Core.Objects.Components;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Structs;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IRayCasting : IBaseObject
    {
        public RayCastDiscription RayCastDiscription { get; set; }

        public RayCastingModule RayCastingModule { get; }
    }
}