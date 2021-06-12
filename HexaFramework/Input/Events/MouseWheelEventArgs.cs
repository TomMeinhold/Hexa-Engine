using System;

namespace HexaFramework.Input.Events
{
    public class MouseWheelEventArgs : EventArgs
    {
        public MouseWheelEventArgs(int delta)
        {
            Delta = delta;
        }

        public int Delta { get; }
        public bool Handled { get; set; }
    }
}