// <copyright file="MouseButtonExtentions.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Extensions
{
    using System.Windows.Forms;
    using HexaEngine.Core.Input.Component;

    public static class MouseButtonExtentions
    {
        public static MouseButtonUpdate ToMouseButtonUpdate(this MouseButtons button, bool none = false)
        {
            if (none)
            {
                return MouseButtonUpdate.None;
            }
            else
            {
                return button switch
                {
                    MouseButtons.Left => MouseButtonUpdate.Left,
                    MouseButtons.Middle => MouseButtonUpdate.Middle,
                    MouseButtons.Right => MouseButtonUpdate.Right,
                    MouseButtons.XButton1 => MouseButtonUpdate.XButton1,
                    MouseButtons.XButton2 => MouseButtonUpdate.XButton2,
                    _ => MouseButtonUpdate.None,
                };
            }
        }
    }
}