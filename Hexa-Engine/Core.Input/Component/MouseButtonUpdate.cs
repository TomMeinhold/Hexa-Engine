// <copyright file="MouseButtonUpdate.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input.Component
{
    public enum MouseButtonUpdate
    {
        /// <summary>
        /// Zusammenfassung: Keine Maustaste gedrückt.
        /// </summary>
        None = -1,

        /// <summary>
        /// Zusammenfassung: Die linke Maustaste gedrückt.
        /// </summary>
        Left = 0,

        /// <summary>
        /// Zusammenfassung: Der mittleren Maustaste.
        /// </summary>
        Middle = 1,

        /// <summary>
        /// Zusammenfassung: Die rechte Maustaste.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Zusammenfassung: Der ersten erweiterten Maustaste.
        /// </summary>
        XButton1 = 3,

        /// <summary>
        /// Zusammenfassung: Der zweiten erweiterten Maustaste.
        /// </summary>
        XButton2 = 4,
    }
}