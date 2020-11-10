using HexaEngine.Core.Input.Component;
using SharpDX.Direct2D1;

namespace HexaEngine.Core.UI.Interfaces
{
    public interface IUserInterface
    {
        void KeyboardInput(KeyboardState state, KeyboardUpdate update);

        void MouseInput(MouseState state, MouseUpdate update);

        void Render(DeviceContext context);
    }
}