using System;
using System.Runtime.CompilerServices;

namespace HexaEngine.Core.Windows
{
    public class RenderPlatform
    {
        public readonly Window Window;

        public RenderPlatform(Window window)
        {
            this.Window = window;
        }

        private int BeginFrame()
        {
            return 0;
        }

        public bool DrawFrame(Action<int, int> draw, [CallerMemberName] string frameName = null)
        {
            return true;
        }

        private void EndFrame(int rtvIndex)
        {
        }

        public void Dispose()
        {
        }
    }
}