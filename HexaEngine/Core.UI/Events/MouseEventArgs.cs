using HexaEngine.Core.Input.Component;
using System;

namespace HexaEngine.Core.UI.Events
{
    public class MouseEventArgs : EventArgs
    {
        public MouseState State;
        public MouseUpdate Update;
        public MouseButtonUpdate MouseButtonUpdate;

        public MouseEventArgs(MouseState state, MouseUpdate update, MouseButtonUpdate keys)
        {
            State = state;
            Update = update;
            MouseButtonUpdate = keys;
        }
    }
}