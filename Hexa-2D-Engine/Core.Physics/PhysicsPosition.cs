using HexaEngine.Core.Common;
using SharpDX.Mathematics.Interop;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine
    {
        public void ProcessingPosition(BaseObject baseObject)
        {
            RawVector3 Position = baseObject.Position;
            RawVector3 Speed = baseObject.Speed;
            Position.X += Speed.X;
            Position.Y += Speed.Y;
            Position.Z += Speed.Z;
            baseObject.Position = Position;
        }
    }
}
