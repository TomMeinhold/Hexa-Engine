using SharpDX.Direct2D1;
using System.Collections;

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
