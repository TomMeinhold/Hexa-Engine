using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Render.Components;
using HexaEngine.Core.UI.Interfaces;
using SharpDX.Direct2D1;

namespace HexaEngine.Core.UI.BaseTypes
{
    public class Image : IUserInterface
    {
        public Image(Texture texture)
        {
            Texture = texture;
        }

        public Texture Texture { get; }

        public InterpolationMode Interpolation { get; set; } = InterpolationMode.HighQualityCubic;

        public void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
            return;
        }

        public void MouseInput(MouseState state, MouseUpdate update)
        {
            return;
        }

        public void Render(DeviceContext context)
        {
            context.DrawImage(Texture, Interpolation);
        }
    }
}