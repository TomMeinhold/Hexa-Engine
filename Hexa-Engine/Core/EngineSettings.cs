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

        /// <summary>
        /// Gets or sets viewport. Currently not in use.
        /// </summary>
        public float Viewport { get; set; } = 1;

        /// <summary>
        /// Gets or sets grid padding.
        /// </summary>
        public float GridPadding { get; set; } = 100;

        /// <summary>
        /// Gets or sets Overscan.
        /// </summary>
        public int Overscan { get; set; } = 0;

        /// <summary>
        /// Gets or sets point multiplier.
        /// </summary>
        public float PointMultiplier { get; set; } = 1;

        /// <summary>
        /// Gets or sets BrushFocusMultiplier.
        /// </summary>
        public float BrushFocusMultiplier { get; set; } = 4;

        /// <summary>
        /// Gets or sets draw precision. Is the interval of Point per Pixel. 1 means each 1 pixel 1
        /// point calcualted. 10 means each 10 pixel 1 point calcualted.
        /// </summary>
        public float DrawPrecision { get; set; }
        public bool AntialiasMode { get; internal set; }
        public int VSync { get; internal set; }
    }
}