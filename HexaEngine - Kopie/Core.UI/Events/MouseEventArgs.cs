using HexaEngine.Core.Input.Component;

namespace HexaEngine.Core.UI.Events
{
    public class MouseEventArgs
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