using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Objects.BaseTypes;
using System;
using System.Windows.Forms;

namespace HexaEngine.Core.Input.Modules
{
    public class KeyboardController
    {
        /// <summary>
        /// A Controller to manage keyboard input.
        /// </summary>
        /// <param name="mouseController">if parameter is null focus will be ignored.</param>
        public KeyboardController(BaseObject baseObject, MouseController mouseController = null)
        {
            BaseObject = baseObject ?? throw new ArgumentNullException(nameof(baseObject));
            MouseController = mouseController;
            baseObject.Activate += BaseObject_Activate;
            baseObject.Disable += BaseObject_Disable;
            if (baseObject.Enabled)
            {
                InputSystem.KeyboardUpdate += InputSystem_KeyboardUpdate;
            }
        }

        /// <summary>
        /// A Controller to manage keyboard input.
        /// </summary>
        /// <param name="mouseController">if parameter is null focus will be ignored.</param>
        public KeyboardController(Control control)
        {
            control.EnabledChanged += (ss, ee) => { if (control.Enabled) { InputSystem.KeyboardUpdate += InputSystem_KeyboardUpdate; } else { InputSystem.KeyboardUpdate -= InputSystem_KeyboardUpdate; } };
            if (control.Enabled)
            {
                InputSystem.KeyboardUpdate += InputSystem_KeyboardUpdate;
            }
        }

        private void BaseObject_Disable(object sender, EventArgs e)
        {
            InputSystem.KeyboardUpdate -= InputSystem_KeyboardUpdate;
        }

        private void BaseObject_Activate(object sender, EventArgs e)
        {
            InputSystem.KeyboardUpdate += InputSystem_KeyboardUpdate;
        }

        public BaseObject BaseObject { get; }

        public MouseController MouseController { get; }

        public event EventHandler<KeyboardUpdatePackage> KeyDown;

        public event EventHandler<KeyboardUpdatePackage> KeyUp;

        private void InputSystem_KeyboardUpdate(object sender, KeyboardUpdatePackage e)
        {
            if (MouseController != null)
            {
                if (MouseController.Focus)
                {
                    HandleEvent(sender, e);
                }
            }
            else
            {
                HandleEvent(sender, e);
            }
        }

        private void HandleEvent(object sender, KeyboardUpdatePackage e)
        {
            if (e.KeyboardUpdate.IsPressed)
            {
                KeyDown?.Invoke(sender, e);
            }
            else
            {
                KeyUp?.Invoke(sender, e);
            }
        }
    }
}