using System;
using System.Drawing;
using System.Windows.Forms;

namespace HexaEngine.Core.Windows
{
    public interface IRenderable
    {
        IntPtr Handle { get; }

        Size ClientSize { get; set; }

        int DeviceDpi { get; }
        ContextMenuStrip ContextMenuStrip { get; }
        int Height { get; }
        int Width { get; }
    }
}