// <copyright file="Camera.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Common
{
    using SharpDX;
    using SharpDX.Mathematics.Interop;
    using System;

    public class Camera
    {
        public Camera(Engine engine)
        {
            this.Z = this.InitialZoom;
            this.Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            UpdateTranslationMatrix();
        }

        public Matrix3x2 TranslationMatrix { get; private set; }

        public Matrix3x2 TranslationMatrixCentered { get; private set; }

        public float InitialZoom { get; } = 1;

        public float X { get; private set; } = 0;

        public float Y { get; private set; } = 0;

        public float Z { get; private set; }

        public bool Up { get; set; } = true;

        public bool Down { get; set; } = true;

        public bool Right { get; set; } = true;

        public bool Left { get; set; } = true;

        public bool IsLocked { get; set; } = false;

        public Engine Engine { get; }

        public void UpdateVisibility()
        {
            float yCenter = this.Engine.Settings.Height / 2;
            float xCenter = this.Engine.Settings.Width / 2;
            if (this.Y * this.Z > yCenter)
            {
                this.Down = false;
            }
            else
            {
                this.Down = true;
            }

            if (this.Y * this.Z * -1 > yCenter - 20)
            {
                this.Up = false;
            }
            else
            {
                this.Up = true;
            }

            if (this.X * this.Z > xCenter)
            {
                this.Right = false;
            }
            else
            {
                this.Right = true;
            }

            if (this.X * this.Z * -1 > xCenter)
            {
                this.Left = false;
            }
            else
            {
                this.Left = true;
            }
        }

        public void SetPosition(RawVector3 rawVector3)
        {
            this.X = rawVector3.X;
            this.Y = rawVector3.Y;
            this.Z = rawVector3.Z;
            UpdateVisibility();
            UpdateTranslationMatrix();
        }

        public void SetPosition(RawVector2 rawVector2)
        {
            this.X = rawVector2.X;
            this.Y = rawVector2.Y;
            UpdateVisibility();
            UpdateTranslationMatrix();
        }

        public void UpdateTranslationMatrix()
        {
            float x = X, y = Y, z = Z;
            x *= z;
            Y *= z;

            this.TranslationMatrix = Matrix.Translation(new RawVector3(x, y, z));
        }
    }
}