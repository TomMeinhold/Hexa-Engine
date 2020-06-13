using HexaEngine.Core.Input;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Interfaces;
using HexaEngine.Core.UI.Interfaces;
using SharpDX.Direct2D1;
using System;

namespace HexaEngine.Core.UI
{
    public class UserInterfaceManager : IInputKeyboard, IInputMouse
    {
        public UserInterfaceManager(Engine engine)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            InputSystem.MouseUpdate += MouseInput;
            InputSystem.KeyboardUpdate += KeyboardInput;
        }

        public IUserInterface ActiveUserInterface { get; set; }

        public Engine Engine { get; }

        public void KeyboardInput(object sender, KeyboardUpdatePackage package)
        {
            if (ActiveUserInterface != null)
            {
                ActiveUserInterface.KeyboardInput(package.KeyboardState, package.KeyboardUpdate);
            }
        }

        public void MouseInput(object sender, MouseUpdatePackage package)
        {
            if (ActiveUserInterface != null)
            {
                ActiveUserInterface.MouseInput(package.MouseState, package.MouseUpdate);
            }
        }

        public void RenderUI(DeviceContext context)
        {
            if (ActiveUserInterface != null)
            {
                ActiveUserInterface.Render(context);
            }
        }
    }
}