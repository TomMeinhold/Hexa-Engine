using SharpDX.Direct3D11;

namespace HexaEngine.Core.Render.Interfaces
{
    public interface IDrawable3D
    {
        void Render3D(DeviceContext context);
    }
}