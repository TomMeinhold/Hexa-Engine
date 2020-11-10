using SharpDX;

namespace HexaEngine.Core.Objects.Interfaces
{
    public interface IBaseObject
    {
        Engine Engine { get; }

        Vector3 Position { get; }

        Vector3 Rotation { get; }

        Vector3 Scale { get; }
    }
}