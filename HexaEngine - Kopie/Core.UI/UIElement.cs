using HexaEngine.Core.Extensions;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.UI.Events;
using SharpDX;
using System;

namespace HexaEngine.Core.UI
{
    public class UIElement : BaseElement
    {
        public event EventHandler<KeyboardEventArgs> OnKeyDown;

        public event EventHandler<KeyboardEventArgs> OnKeyUp;

        public event EventHandler<MouseEventArgs> OnMouseButtonDown;

        public event EventHandler<MouseEventArgs> OnMouseButtonUp;

        public event EventHandler<MouseEventArgs> OnMouseEnter;

        public event EventHandler<MouseEventArgs> OnMouseHover;

        public event EventHandler<MouseEventArgs> OnMouseLeave;

        public event EventHandler<MouseEventArgs> Click;

        public bool MouseHover { get; private set; }

        public override void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
            if (update.IsPressed)
            {
                OnKeyDown?.Invoke(this, new KeyboardEventArgs(state, update, update.Key));
            }
            else
            {
                OnKeyUp?.Invoke(this, new KeyboardEventArgs(state, update, update.Key));
            }
        }

        public override void MouseInput(MouseState state, MouseUpdate update)
        {
            if (BoundingBox.ContainsVector(new Vector3(state.LocationRaw.X, state.LocationRaw.Y - ActualHeight, state.LocationRaw.Z)))
            {
                if (MouseHover)
                {
                    OnMouseHover?.Invoke(this, new MouseEventArgs(state, update, default));
                }
                else
                {
                    OnMouseEnter?.Invoke(this, new MouseEventArgs(state, update, default));
                }

                MouseHover = true;
            }
            else
            {
                MouseHover = false;
                OnMouseLeave?.Invoke(this, new MouseEventArgs(state, update, default));
            }

            if (MouseHover)
            {
                if (update.MouseButton == MouseButtonUpdate.Left && !update.IsPressed)
                {
                    Click?.Invoke(this, null);
                }
                if (update.IsPressed && update.MouseButton != MouseButtonUpdate.None)
                {
                    OnMouseButtonDown?.Invoke(this, new MouseEventArgs(state, update, update.MouseButton));
                }
                else
                {
                    OnMouseButtonUp?.Invoke(this, new MouseEventArgs(state, update, update.MouseButton));
                }
            }
        }
    }
}