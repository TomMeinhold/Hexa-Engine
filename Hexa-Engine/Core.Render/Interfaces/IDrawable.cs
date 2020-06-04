using HexaEngine.Core.Objects.Interfaces;
using SharpDX.Direct2D1;

namespace HexaEngine.Core.Render.Interfaces
{
    public interface IDrawable : IBaseObject
    {
        Bitmap1 Bitmap { get; set; }

        void Render(DeviceContext context);
    }
}