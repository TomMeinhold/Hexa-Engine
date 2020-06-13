// <copyright file="InputProcessing.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input
{
    using HexaEngine.Core.Input.Component;
    using HexaEngine.Core.Input.Extentions;
    using SharpDX;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Input Management.
    /// </summary>
    public partial class InputSystem
    {
        private Vector2 firstpoint;

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 tmp = new Vector2(Control.MousePosition.X, Control.MousePosition.Y);
            Vector2 res = new Vector2(this.firstpoint.X - tmp.X, (this.firstpoint.Y - tmp.Y));
            this.firstpoint = tmp;

            var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(true), false, new Vector3(res.X * -1, res.Y * -1, 0));
            this.MouseState.UpdateLocation(update);
            PointF p = Form.PointToClient(Cursor.Position);
            this.MouseState.UpdateRawLocation(new Vector3(p.X, p.Y, 0));
            if (this.Active)
            {
                MouseUpdate?.Invoke(this, new MouseUpdatePackage(this.MouseState, update));
            }
        }

        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            Vector3 tmp3 = default;
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
                MouseUpdate?.Invoke(this, new MouseUpdatePackage(this.MouseState, update));
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(), true, this.MouseState.Location);
            this.MouseState.UpdateButton(update);
            if (this.Active)
            {
                MouseUpdate?.Invoke(this, new MouseUpdatePackage(this.MouseState, update));
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(), false, this.MouseState.Location);
            this.MouseState.UpdateButton(update);
            if (this.Active)
            {
                MouseUpdate?.Invoke(this, new MouseUpdatePackage(this.MouseState, update));
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            var update = new KeyboardUpdate(true, e.KeyCode);
            this.KeyboardState.Update(update);
            if (this.Active)
            {
                KeyboardUpdate?.Invoke(this, new KeyboardUpdatePackage(this.KeyboardState, update));
            }
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            var update = new KeyboardUpdate(false, e.KeyCode);
            this.KeyboardState.Update(update);
            if (this.Active)
            {
                KeyboardUpdate?.Invoke(this, new KeyboardUpdatePackage(this.KeyboardState, update));
            }
        }
    }
}