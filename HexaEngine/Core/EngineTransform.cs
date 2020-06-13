// <copyright file="EngineTransform.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core
{
    using SharpDX.Direct2D1;

    public class EngineTransform
    {
        public EngineTransform(Engine engine)
        {
            this.Engine = engine;
        }

        public Engine Engine { get; }

        public Bitmap1 ConvertBitmap(System.Drawing.Bitmap bitmap, bool alhpa = false)
        {
            return Common.ConvertBitmap.Convert(this.Engine.RenderSystem.RessouceManager.D2DDeviceContext, bitmap, alhpa);
        }
    }
}