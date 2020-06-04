// <copyright file="IInputMouse.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using HexaEngine.Core.Input.Component;

namespace HexaEngine.Core.Input.Interfaces
{
    /// <summary>
    /// Implementing input function for mouse.
    /// </summary>
    public interface IInputMouse
    {
        /// <summary>
        /// Void for input logic.
        /// </summary>
        /// <param name="state">Displays all states.</param>
        /// <param name="update">Displays altered states.</param>
        void MouseInput(MouseState state, MouseUpdate update);
    }
}