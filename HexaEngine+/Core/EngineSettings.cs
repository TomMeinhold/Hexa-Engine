// <copyright file="EngineSettings.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core
{
    /// <summary>
    /// Properties for the drawn objects.
    /// </summary>
    public class EngineSettings
    {
        /// <summary>
        /// Gets or sets height.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Gets or sets width.
        /// </summary>
        public float Width { get; set; }

        public bool DebugMode { get; set; }

        /// <summary>
        /// Gets or sets draw precision. Is the interval of Point per Pixel. 1 means each 1 pixel 1
        /// point calcualted. 10 means each 10 pixel 1 point calcualted.
        /// </summary>
        public float DrawPrecision { get; set; }

        public bool AntialiasMode { get; set; }

        public int VSync { get; set; }
    }
}