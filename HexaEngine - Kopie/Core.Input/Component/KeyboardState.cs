// <copyright file="KeyboardState.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input.Component
{
    using System.Collections.Generic;

    public class KeyboardState
    {
        private Dictionary<Keys, bool> Keys { get; set; } = new Dictionary<Keys, bool>();

        public bool KeyIsPressed(Keys keys)
        {
            if (Keys.ContainsKey(keys))
            {
                return Keys[keys];
            }
            else
            {
                Keys[keys] = false;
                return Keys[keys];
            }
        }

        public bool KeyIsReleased(Keys keys)
        {
            if (Keys.ContainsKey(keys))
            {
                return !Keys[keys];
            }
            else
            {
                Keys[keys] = false;
                return !Keys[keys];
            }
        }

        public void Update(KeyboardUpdate update)
        {
            Keys[update.Key] = update.IsPressed;
        }
    }
}