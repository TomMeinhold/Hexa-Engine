using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Ressources;
using SharpDX.Direct2D1;

namespace HexaEngine.Core.Render.Interfaces
{
    public interface IDrawable : IBaseObject
    {
        Sprite Sprite { get; set; }

        void Render(DeviceContext context);
    }
}