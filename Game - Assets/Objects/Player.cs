using HexaEngine.Core;
using HexaEngine.Core.Game;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace GameAssets
{
    public class Player : PlayerBase
    {
        public Player(Engine engineCore, Bitmap bitmap, Vector3 position) : base(engineCore, bitmap, position)
        {
            Damper = new RawVector3(0.5F, 0F, 0F);
            NaturalDeceleration = new RawVector3(2F, 1F, 1F);
            MaxSpeed = new RawVector3(10,10,0);
            State = HexaEngine.Core.Common.BaseObjectState.Initialized;
        }
    }
}
