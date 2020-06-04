﻿// <copyright file="MouseUpdate.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input.Component
{
    using System;
    using System.Windows.Input;
    using SharpDX.Mathematics.Interop;

    public struct MouseUpdate : IEquatable<MouseUpdate>
    {
        public MouseUpdate(MouseButtonUpdate mouseButton, bool isPressed, RawVector3 location)
        {
            this.MouseButton = mouseButton;
            this.IsPressed = isPressed;
            this.Location = location;
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
                return this.Equals(update);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.MouseButton.GetHashCode() + this.Location.GetHashCode();
        }

        public bool Equals(MouseUpdate other)
        {
            return this.MouseButton == other.MouseButton && this.Location.X == other.Location.X && this.Location.Y == other.Location.Y && this.Location.Z == other.Location.Z && this.IsPressed == other.IsPressed;
        }
    }
}