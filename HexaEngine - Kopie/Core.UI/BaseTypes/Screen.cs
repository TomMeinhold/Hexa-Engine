using HexaEngine.Core.Input.Component;
using HexaEngine.Core.UI.Interfaces;
using SharpDX.Direct2D1;
using System.Collections.Generic;

namespace HexaEngine.Core.UI.BaseTypes
{
    public class Screen : IUserInterface
    {
        public List<IUserInterface> UserInterfaces { get; } = new List<IUserInterface>();

        public void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
            foreach (IUserInterface userInterface in UserInterfaces)
            {
                userInterface.KeyboardInput(state, update);
            }
        }

        public void MouseInput(MouseState state, MouseUpdate update)
        {
            foreach (IUserInterface userInterface in UserInterfaces)
            {
                userInterface.MouseInput(state, update);
            }
        }

        public void Render(DeviceContext context)
        {
            foreach (IUserInterface userInterface in UserInterfaces)
            {
                userInterface.Render(context);
            }
        }
    }
}