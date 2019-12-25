using SharpDX.Direct2D1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Common
{
    public class Brushpalette
    {
        public ArrayList Brushes { get; } = new ArrayList();

        public SolidColorBrush BrushBlack;

        public SolidColorBrush BrushRed;

        public void Dispose()
        {
            foreach (SolidColorBrush a in Brushes)
            {
                a.Dispose();
            }
            BrushBlack.Dispose();
            BrushRed.Dispose();
        }
    }
}
