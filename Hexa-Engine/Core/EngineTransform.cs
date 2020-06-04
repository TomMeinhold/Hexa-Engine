// <copyright file="EngineTransform.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core
{
    using SharpDX.Direct2D1;
    using SharpDX.Mathematics.Interop;

    public class EngineTransform
    {
        public EngineTransform(Engine engine)
        {
            this.Engine = engine;
        }

        public Engine Engine { get; }

        public RawVector2 InsertRelativePosition(RawVector2 vector2)
        {
            vector2.X += this.Engine.Camera.X;
            vector2.Y += this.Engine.Camera.Y;
            vector2.X *= this.Engine.Camera.Z;
            vector2.Y *= this.Engine.Camera.Z;
            return vector2;
        }

        public RawVector2 ReventRelativePosition(RawVector2 vector2)
        {
            vector2.X /= this.Engine.Camera.Z;
            vector2.Y /= this.Engine.Camera.Z;
            vector2.X -= this.Engine.Camera.X;
            vector2.Y -= this.Engine.Camera.Y;
            return vector2;
        }

        public RawVector2 InsertRelativePositionNonZoom(RawVector2 vector2)
        {
            vector2.X /= this.Engine.Camera.Z;
            vector2.Y /= this.Engine.Camera.Z;
            vector2.X += this.Engine.Camera.X;
            vector2.Y += this.Engine.Camera.Y;
            vector2.X *= this.Engine.Camera.Z;
            vector2.Y *= this.Engine.Camera.Z;
            return vector2;
        }

        public RawVector2 RemoveZoom(RawVector2 vector2)
        {
            vector2.X *= this.Engine.Camera.Z;
            vector2.Y *= this.Engine.Camera.Z;
            return vector2;
        }

        public RawVector2 Zoom(RawVector2 vector2)
        {
            vector2.X *= this.Engine.Camera.Z;
            vector2.Y *= this.Engine.Camera.Z;
            return vector2;
        }

        public RawVector2 ReventRelativePositionNonZoom(RawVector2 vector2)
        {
            vector2.X /= this.Engine.Camera.Z;
            vector2.Y /= this.Engine.Camera.Z;
            vector2.X -= this.Engine.Camera.X;
            vector2.Y -= this.Engine.Camera.Y;
            vector2.X *= this.Engine.Camera.Z;
            vector2.Y *= this.Engine.Camera.Z;
            return vector2;
        }

        public RawVector2 CenterVector(RawVector2 vector2)
        {
            vector2.X += this.Engine.Settings.Width / 2;
            vector2.Y += this.Engine.Settings.Height / 2;
            return vector2;
        }

        public RawVector2 UncenterVector(RawVector2 vector2)
        {
            vector2.X -= this.Engine.Settings.Width / 2;
            vector2.Y -= this.Engine.Settings.Height / 2;
            return vector2;
        }

        public RawVector2 GetStartpoint()
        {
            RawVector2 rawVector = new RawVector2
            {
                X = this.Engine.Camera.X * -1 * this.Engine.Camera.Z,
                Y = this.Engine.Camera.Y * -1 * this.Engine.Camera.Z,
            };
            return rawVector;
        }

        /// <summary>
        /// Erstellt den Punkt der als letztes gezeichnet wird.
        /// </summary>
        /// <returns>Letzter Punkt.</returns>
        public RawVector2 GetEndpoint()
        {
            RawVector2 rawVector = new RawVector2
            {
                X = this.Engine.Settings.Width / 2 / this.Engine.Camera.Z / this.Engine.Settings.DrawPrecision,
                Y = this.Engine.Settings.Height / 2 / this.Engine.Camera.Z / this.Engine.Settings.DrawPrecision,
            };
            return rawVector;
        }

        public RawVector2 ConvertToRelGrid(RawVector2 vector2)
        {
            vector2 = this.UncenterVector(vector2);
            vector2.X /= this.Engine.Camera.Z / this.Engine.Camera.InitialZoom;
            vector2.Y /= this.Engine.Camera.Z / this.Engine.Camera.InitialZoom;
            vector2 = this.ReventRelativePositionNonZoom(vector2);
            vector2.X /= this.Engine.Settings.GridPadding;
            vector2.Y /= this.Engine.Settings.GridPadding;
            vector2.Y *= -1;
            return vector2;
        }

        public Bitmap1 ConvertBitmap(System.Drawing.Bitmap bitmap, bool alhpa = false)
        {
            return Common.ConvertBitmap.Convert(this.Engine.RenderSystem.RessouceManager.D2DDeviceContext, bitmap, alhpa);
        }
    }
}