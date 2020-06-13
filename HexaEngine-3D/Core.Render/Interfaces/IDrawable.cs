using SharpDX.Direct2D1;

namespace HexaEngine.Core.Render.Interfaces
{
    public interface IDrawable
    {
        Bitmap1 Bitmap { get; set; }

        void Render(DeviceContext context);
    }
}