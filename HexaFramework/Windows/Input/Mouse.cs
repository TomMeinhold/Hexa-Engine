using HexaFramework.Windows.Input.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace HexaFramework.Windows.Input
{
    public class Mouse
    {
        private readonly Dictionary<MouseButton, MouseButtonState> buttons = new();
        private bool first_pos = true;
        private Point last_pos;

        public Mouse()
        {
            foreach (MouseButton button in Enum.GetValues(typeof(MouseButton)))
            {
                buttons.Add(button, MouseButtonState.Released);
            }
        }

        public IReadOnlyDictionary<MouseButton, MouseButtonState> Buttons => buttons;

        public Vector2 PositionVector { get; private set; }

        public Point Position { get; private set; }

        public bool Hover { get; private set; }

        public Vector2 Delta { get; private set; }

        internal MouseEventArgs Update(MouseButton button, MouseButtonState state)
        {
            buttons[button] = state;
            return new MouseEventArgs(button, state, Position);
        }

        internal MouseEventArgs Update(Point point)
        {
            if (first_pos)
            {
                first_pos = false;
                last_pos = point;
            }
            Position = point;
            PositionVector = new Vector2(point.X, point.Y);
            Delta = PositionVector - new Vector2(last_pos.X, last_pos.Y);
            last_pos = point;
            return new MouseEventArgs(MouseButton.None, MouseButtonState.Released, Position);
        }

        internal MouseEventArgs Update(bool state)
        {
            Hover = state;
            return new MouseEventArgs(MouseButton.None, MouseButtonState.Released, Position);
        }

        public void ClearDelta()
        {
            Delta = Vector2.Zero;
        }
    }
}