// <copyright file="KeyboardUpdate.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input.Component
{
    using System;
    using System.Windows.Forms;

    public struct KeyboardUpdate : IEquatable<KeyboardUpdate>
    {
        public KeyboardUpdate(bool isPressed, Keys key)
        {
            this.IsPressed = isPressed;
            this.Key = key;
        }

        public bool IsPressed { get; set; }

        public Keys Key { get; set; }

        public static bool operator ==(KeyboardUpdate left, KeyboardUpdate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(KeyboardUpdate left, KeyboardUpdate right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is KeyboardUpdate update)
            {
                return update.Key == this.Key && update.IsPressed == this.IsPressed;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.IsPressed.GetHashCode() + this.Key.GetHashCode();
        }

        public bool Equals(KeyboardUpdate other)
        {
            return other.Key == this.Key && other.IsPressed == this.IsPressed;
        }
    }
}