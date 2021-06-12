using HexaFramework.Input.Events;
using HexaFramework.Windows.Native;
using System;
using System.Collections.Generic;

namespace HexaFramework.Input
{
    public class Keyboard
    {
        public Keyboard()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                var state = (KeyStates)User32.GetKeyState((int)key);
                KeyStates[key] = state;
            }
        }

        public Dictionary<Keys, KeyStates> KeyStates { get; } = new();

        public event EventHandler<KeyboardEventArgs> OnKeyDown;

        public event EventHandler<KeyboardEventArgs> OnKeyUp;

        public KeyboardEventArgs Update(Keys key, KeyStates state)
        {
            KeyStates[key] = state;
            var args = new KeyboardEventArgs(key, state);
            if (state == Input.KeyStates.Pressed | state == Input.KeyStates.Toggled)
            {
                OnKeyDown?.Invoke(this, args);
            }
            else if (state == Input.KeyStates.Released)
            {
                OnKeyUp?.Invoke(this, args);
            }
            return args;
        }

        public bool IsDown(Keys n)
        {
            return KeyStates[n] == Input.KeyStates.Pressed;
        }
    }
}