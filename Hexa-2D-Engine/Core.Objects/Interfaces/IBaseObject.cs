using SharpDX.Mathematics.Interop;

namespace HexaEngine.Core.Objects.Interfaces
{
    public interface IBaseObject
    {
        RawVector3 Position { get; }
    }
}