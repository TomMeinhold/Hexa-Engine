using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Debug
{
    public class DebugWindow
    {
        Thread DebugWindowThread;
        private Engine engine;

        public DebugWindow(Engine engine)
        {
            this.engine = engine;
        }

        public void Show()
        {
            DebugWindowThread = new Thread(Window);
            DebugWindowThread.Start();
        }

        private void Window()
        {
            Debug debug = new Debug(engine);
            debug.ShowDialog();
            debug.Dispose();
        }
    }
}
