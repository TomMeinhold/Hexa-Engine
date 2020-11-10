using HexaEngine.Core.Input.Component;

namespace HexaEngine.Core.UI.Events
{
    public class KeyboardEventArgs
    {
        public KeyboardState State;
        public KeyboardUpdate Update;
        public Keys Keys;

        public KeyboardEventArgs(KeyboardState state, KeyboardUpdate update, Keys keys)
        {
            State = state;
            Update = update;
            Keys = keys;
        }
    }
}