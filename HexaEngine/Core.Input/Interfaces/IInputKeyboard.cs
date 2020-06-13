// <copyright file="IInputKeyboard.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using HexaEngine.Core.Input.Component;

namespace HexaEngine.Core.Input.Interfaces
{
    /// <summary>
    /// Implementing input function for keyboard.
    /// </summary>
    public interface IInputKeyboard
    {
        /// <summary>
        /// Void for input logic. <see cref="InputSystem.KeyboardUpdate"/>
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="package">Displays all states.</param>
        void KeyboardInput(object sender, KeyboardUpdatePackage package);
    }
}