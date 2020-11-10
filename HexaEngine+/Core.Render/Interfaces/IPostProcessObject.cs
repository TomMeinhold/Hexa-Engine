using HexaEngine.Core.Objects.Components;

namespace HexaEngine.Core.Objects.Interfaces
{
    public interface IPostProcessObject
    {
        public ObjectPostProcessManager ObjectPostProcess { get; set; }
    }
}