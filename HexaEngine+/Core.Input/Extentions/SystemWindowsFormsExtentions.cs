using HexaEngine.Core.Input.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexaEngine.Core.Input.Extentions
{
    public static class SystemWindowsFormsExtentions
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