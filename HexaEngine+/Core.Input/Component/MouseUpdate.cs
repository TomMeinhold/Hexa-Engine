// <copyright file="MouseUpdate.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input.Component
{
    using SharpDX.Mathematics.Interop;
    using System;

    public struct MouseUpdate : IEquatable<MouseUpdate>
    {
        public MouseUpdate(MouseButtonUpdate mouseButton, bool isPressed, RawVector3 location)
        {
            MouseButton = mouseButton;
            IsPressed = isPressed;
            Location = location;
        }

        public MouseButtonUpdate MouseButton { get; set; }

        public bool IsPressed { get; set; }

        public RawVector3 Location { get; set; }

        public static bool operator ==(MouseUpdate left, MouseUpdate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MouseUpdate left, MouseUpdate right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is MouseUpdate update)
            {
                return Equals(update);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return MouseButton.GetHashCode() + Location.GetHashCode();
        }

        public bool Equals(MouseUpdate other)
        {
            return MouseButton == other.MouseButton && Location.X == other.Location.X && Location.Y == other.Location.Y && Location.Z == other.Location.Z && IsPressed == other.IsPressed;
        }
    }
}