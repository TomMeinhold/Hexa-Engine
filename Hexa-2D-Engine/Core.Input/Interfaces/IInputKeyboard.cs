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
        /// Void for input logic.
        /// </summary>
        /// <param name="state">Displays all states.</param>
        /// <param name="update">Displays altered states.</param>
        void KeyboardInput(KeyboardState state, KeyboardUpdate update);
    }
}