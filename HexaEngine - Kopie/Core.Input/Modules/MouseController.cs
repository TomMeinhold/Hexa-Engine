using HexaEngine.Core.Extensions;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using System;

namespace HexaEngine.Core.Input.Modules
{
    public class MouseController
    {
        private int buttonsDown;

        public MouseController(BaseObject baseObject)
        {
            BaseObject = baseObject ?? throw new ArgumentNullException(nameof(baseObject));
            baseObject.Activate += BaseObject_Activate;
            baseObject.Disable += BaseObject_Disable;
            if (baseObject.Enabled)
            {
                InputSystem.MouseUpdate += InputSystem_MouseUpdate;
            }
        }

        public BaseObject BaseObject { get; set; }

        public bool Focus { get; set; }

        public bool IsMouseDown { get; set; }

        public event EventHandler<MouseUpdatePackage> MouseEnter;

        public event EventHandler<MouseUpdatePackage> MouseLeave;

        public event EventHandler<MouseUpdatePackage> MouseHover;

        public event EventHandler<MouseUpdatePackage> MouseDown;

        public event EventHandler<MouseUpdatePackage> MouseUp;

        private void BaseObject_Disable(object sender, EventArgs e)
        {
            InputSystem.MouseUpdate -= InputSystem_MouseUpdate;
        }

        private void BaseObject_Activate(object sender, EventArgs e)
        {
            InputSystem.MouseUpdate += InputSystem_MouseUpdate;
        }

        private void InputSystem_MouseUpdate(object sender, MouseUpdatePackage e)
        {
            if (BaseObject.BoundingBox.ContainsVector(new Vector3(e.MouseState.LocationRaw.X, e.MouseState.LocationRaw.Y - BaseObject.BoundingBox.Height, e.MouseState.LocationRaw.Z)))
            {
                Focus = true;
                MouseEnter?.Invoke(sender, e);
            }
            else
            {
                Focus = false;
                MouseLeave?.Invoke(sender, e);
            }

            if (Focus)
            {
                if (e.MouseUpdate.MouseButton != MouseButtonUpdate.None)
                {
                    if (e.MouseUpdate.IsPressed)
                    {
                        MouseDown?.Invoke(sender, e);
                        buttonsDown++;
                    }
                    else
                    {
                        MouseUp?.Invoke(sender, e);
                        buttonsDown--;
                    }

                    IsMouseDown = Convert.ToBoolean(buttonsDown);
                }
                else
                {
                    MouseHover?.Invoke(sender, e);
                }
            }
        }
    }
}