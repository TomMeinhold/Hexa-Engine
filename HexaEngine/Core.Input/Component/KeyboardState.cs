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
            if (this.Keys.ContainsKey(keys))
            {
                return this.Keys[keys];
            }
            else
            {
                this.Keys[keys] = false;
                return this.Keys[keys];
            }
        }

        public bool KeyIsReleased(Keys keys)
        {
            if (this.Keys.ContainsKey(keys))
            {
                return !this.Keys[keys];
            }
            else
            {
                this.Keys[keys] = false;
                return !this.Keys[keys];
            }
        }

        public void Update(KeyboardUpdate update)
        {
            this.Keys[update.Key] = update.IsPressed;
        }
    }
}