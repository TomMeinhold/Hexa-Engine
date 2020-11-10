// <copyright file="EngineTransform.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core
{
    using SharpDX.Direct2D1;
    using SharpDX.Direct3D11;

    public class BitmapConverter
    {
        public BitmapConverter()
        {
        }

        public Bitmap1 ConvertBitmap(System.Drawing.Bitmap bitmap, bool alhpa = false)
        {
            return Common.ConvertBitmap.Convert(Engine.Current.RenderSystem.DriectXManager.D2DDeviceContext, bitmap, alhpa);
        }

        public Texture2D ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            return Common.ConvertBitmap.Convert(Engine.Current.RenderSystem.DriectXManager.D3DDeviceContext, bitmap);
        }
    }
}