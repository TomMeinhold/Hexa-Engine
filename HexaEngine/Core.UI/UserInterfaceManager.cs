using HexaEngine.Core.Input;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.UI.Interfaces;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace HexaEngine.Core.UI
{
    public class UserInterfaceManager
    {
        public UserInterfaceManager()
        {
            InputSystem.MouseUpdate += MouseInput;
            InputSystem.KeyboardUpdate += KeyboardInput;
        }

        public IUserInterface ActiveUserInterface { get; set; }

        public Dictionary<Type, IUserInterface> Instances { get; } = new Dictionary<Type, IUserInterface>();

        public void SetUIByType(Type type, bool createNew = false)
        {
            if (!createNew && Instances.ContainsKey(type))
            {
                ActiveUserInterface = Instances[type];
            }
            else
            {
                Instances[type] = ActiveUserInterface = (IUserInterface)Activator.CreateInstance(type);
            }
        }

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