using HexaFramework.Windows.Native;

namespace HexaFramework.Windows
{
    public class Cursor
    {
        private readonly NativeWindow window;
        private bool lockState;

        public Cursor(NativeWindow window)
        {
            this.window = window;
        }

        public static int Show()
        {
            return User32.ShowCursor(true);
        }

        public static int Hide()
        {
            return User32.ShowCursor(false);
        }

        public bool IsLocked { get => lockState; }

        public void Lock(bool state)
        {
            if (state)
            {
                if (window.IsActive & window.Mouse.Hover)
                {
                    lockState = true;
                }
            }
            else
            {
                lockState = false;
            }
        }

        private int tick;

        internal void Tick()
        {
            tick++;
            if (lockState & window.IsActive & window.Mouse.Hover & tick > 10)
            {
                tick = 0;
                var centerX = window.X + (window.Width / 2);
                var centerY = window.Y + (window.Height / 2);
                User32.SetCursorPos(centerX, centerY);
            }
        }
    }
}