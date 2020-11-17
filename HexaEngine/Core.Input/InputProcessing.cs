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
    using Keys = Component.Keys;

    /// <summary>
    /// Input Management.
    /// </summary>
    public partial class InputSystem
    {
        private Vector2 firstpoint;

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 tmp = new Vector2(Control.MousePosition.X, Control.MousePosition.Y);
            Vector2 res = new Vector2(firstpoint.X - tmp.X, firstpoint.Y - tmp.Y);
            firstpoint = tmp;

            var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(true), false, new Vector3(res.X * -1, res.Y * -1, 0));
            MouseState.UpdateLocation(update);
            PointF p = Control.PointToClient(Cursor.Position);
            MouseState.UpdateRawLocation(new Vector3(p.X, p.Y, 0));
            if (Active)
            {
                MouseUpdate?.Invoke(this, new MouseUpdatePackage(MouseState, update));
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
            MouseState.UpdateLocation(update);
            if (Active)
            {
                MouseUpdate?.Invoke(this, new MouseUpdatePackage(MouseState, update));
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(), true, MouseState.Location);
            MouseState.UpdateButton(update);
            if (Active)
            {
                MouseUpdate?.Invoke(this, new MouseUpdatePackage(MouseState, update));
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            var update = new MouseUpdate(e.Button.ToMouseButtonUpdate(), false, MouseState.Location);
            MouseState.UpdateButton(update);
            if (Active)
            {
                MouseUpdate?.Invoke(this, new MouseUpdatePackage(MouseState, update));
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            var update = new KeyboardUpdate(true, (Keys)(int)e.KeyCode);
            KeyboardState.Update(update);
            if (Active)
            {
                KeyboardUpdate?.Invoke(this, new KeyboardUpdatePackage(KeyboardState, update));
            }
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            var update = new KeyboardUpdate(false, (Keys)(int)e.KeyCode);
            KeyboardState.Update(update);
            if (Active)
            {
                KeyboardUpdate?.Invoke(this, new KeyboardUpdatePackage(KeyboardState, update));
            }
        }
    }
}