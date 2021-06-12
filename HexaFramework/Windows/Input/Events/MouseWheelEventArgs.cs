﻿using System;

namespace HexaFramework.Windows.Input.Events
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