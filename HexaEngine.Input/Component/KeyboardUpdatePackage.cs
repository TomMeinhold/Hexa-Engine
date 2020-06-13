using System;

namespace HexaEngine.Core.Input.Component
{
    public struct KeyboardUpdatePackage
    {
        public KeyboardUpdatePackage(KeyboardState keyboardState, KeyboardUpdate keyboardUpdate)
        {
            KeyboardState = keyboardState ?? throw new ArgumentNullException(nameof(keyboardState));
            KeyboardUpdate = keyboardUpdate;
        }

        public KeyboardState KeyboardState { get; set; }

        public KeyboardUpdate KeyboardUpdate { get; set; }
    }
}