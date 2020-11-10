// <copyright file="MouseState.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input.Component
{
    using System.Collections.Generic;
    using SharpDX;

    public class MouseState
    {
        public Vector3 Location { get; set; }

        public Vector3 LocationRaw { get; set; }

        private Dictionary<MouseButtonUpdate, bool> MouseButtons { get; } = new Dictionary<MouseButtonUpdate, bool>();

        public bool MouseButtonIsPressed(MouseButtonUpdate keys)
        {
            if (MouseButtons.ContainsKey(keys))
            {
                return MouseButtons[keys];
            }
            else
            {
                MouseButtons[keys] = false;
                return MouseButtons[keys];
            }
        }

        public bool MouseButtonIsReleased(MouseButtonUpdate keys)
        {
            if (MouseButtons.ContainsKey(keys))
            {
                return !MouseButtons[keys];
            }
            else
            {
                MouseButtons[keys] = false;
                return !MouseButtons[keys];
            }
        }

        public void UpdateButton(MouseUpdate update)
        {
            MouseButtons[update.MouseButton] = update.IsPressed;
        }

        public void UpdateLocation(MouseUpdate update)
        {
            Location = update.Location;
        }

        public void UpdateRawLocation(Vector3 update)
        {
            LocationRaw = update;
        }
    }
}