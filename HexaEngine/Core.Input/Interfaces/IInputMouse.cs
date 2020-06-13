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
        /// Void for input logic. <see cref="InputSystem.MouseUpdate"/>
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="package">Displays all states.</param>
        void MouseInput(object sender, MouseUpdatePackage package);
    }
}