// <copyright file="InputProcessing.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using HexaEngine.Core.Extentions;
    using HexaEngine.Core.Input.Component;
    using HexaEngine.Core.Input.Interfaces;
    using SharpDX.Mathematics.Interop;

    /// <summary>
    /// Input Management.
    /// </summary>
    public partial class InputSystem
    {
        private RawVector2 firstpoint;

        public List<IInputKeyboard> InputKeyboards { get; } = new List<IInputKeyboard>();

        public List<IInputMouse> InputMice { get; } = new List<IInputMouse>();

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            lock (this.InputMice)
            {
                RawVector2 tmp = new RawVector2(Control.MousePosition.X, Control.MousePosition.Y);
                RawVector2 res = new RawVector2(this.firstpoint.X - tmp.X, (this.firstpoint.Y - tmp.Y) * -1);
                this.firstpoint = tmp;

                var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(true), false, res.Invert().UpgradeToVector3());
                this.MouseState.UpdateLocation(update);
                PointF p = Form.PointToClient(Cursor.Position);
                this.MouseState.UpdateRawLocation(new RawVector2(p.X, p.Y * -1).UpgradeToVector3());
                if (this.Active)
                {
                    foreach (IInputMouse plugin in this.InputMice)
                    {
                        plugin.MouseInput(this.MouseState, update);
                    }
                }
            }
        }

        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            lock (this.InputMice)
            {
                RawVector3 tmp3 = default;
                switch (e.Delta)
                {
                    case 120:
                        tmp3.Z = 1;
                        break;

                    case -120:
                        tmp3.Z = -1;
                        break;
                }

                var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(true), false, tmp3);
                this.MouseState.UpdateLocation(update);
                if (this.Active)
                {
                    foreach (IInputMouse plugin in this.InputMice)
                    {
                        plugin.MouseInput(this.MouseState, update);
                    }
                }
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            lock (this.InputMice)
            {
                var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(), true, this.MouseState.Location);
                this.MouseState.UpdateButton(update);
                if (this.Active)
                {
                    foreach (IInputMouse plugin in this.InputMice)
                    {
                        plugin.MouseInput(this.MouseState, update);
                    }
                }
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            lock (this.InputMice)
            {
                var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(), false, this.MouseState.Location);
                this.MouseState.UpdateButton(update);
                if (this.Active)
                {
                    foreach (IInputMouse plugin in this.InputMice)
                    {
                        plugin.MouseInput(this.MouseState, update);
                    }
                }
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            lock (this.InputKeyboards)
            {
                var update = new KeyboardUpdate(true, e.KeyCode);
                this.KeyboardState.Update(update);
                if (this.Active)
                {
                    foreach (IInputKeyboard plugin in this.InputKeyboards)
                    {
                        plugin.KeyboardInput(this.KeyboardState, update);
                    }
                }
            }
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            lock (this.InputKeyboards)
            {
                var update = new KeyboardUpdate(false, e.KeyCode);
                this.KeyboardState.Update(update);
                if (this.Active)
                {
                    foreach (IInputKeyboard plugin in this.InputKeyboards)
                    {
                        plugin.KeyboardInput(this.KeyboardState, update);
                    }
                }
            }
        }
    }
}