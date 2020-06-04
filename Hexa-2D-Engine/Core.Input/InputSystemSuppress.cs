// <copyright file="InputSystemSuppress.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input
{
    using System;

    /// <summary>
    /// Input Management.
    /// </summary>
    public partial class InputSystem
    {
        private void Initial()
        {
            this.Form.Activated += this.Form_Activated;
            this.Form.Deactivate += this.Form_Deactivate;
            this.Form.ResizeBegin += this.Form_ResizeBegin;
            this.Form.ResizeEnd += this.Form_ResizeEnd;
            this.Form.MouseEnter += this.Form_MouseEnter;
            this.Form.MouseLeave += this.Form_MouseLeave;
        }

        private void Form_MouseLeave(object sender, EventArgs e)
        {
            this.Active = false;
        }

        private void Form_MouseEnter(object sender, EventArgs e)
        {
            this.Active = true;
        }

        private void Form_ResizeEnd(object sender, EventArgs e)
        {
            this.Active = true;
        }

        private void Form_ResizeBegin(object sender, EventArgs e)
        {
            this.Active = false;
        }

        private void Form_Deactivate(object sender, EventArgs e)
        {
            this.Active = false;
        }

        private void Form_Activated(object sender, EventArgs e)
        {
            this.Active = true;
        }
    }
}