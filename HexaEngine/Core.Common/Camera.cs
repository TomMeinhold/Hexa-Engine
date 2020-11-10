// <copyright file="Camera.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Common
{
    using SharpDX;
    using SharpDX.Mathematics.Interop;

    public class Camera
    {
        public Camera()
        {
            Z = InitialZoom;
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

        public void UpdateVisibility()
        {
            float yCenter = Engine.Current.Settings.Height / 2;
            float xCenter = Engine.Current.Settings.Width / 2;
            if (Y * Z > yCenter)
            {
                Down = false;
            }
            else
            {
                Down = true;
            }

            if (Y * Z * -1 > yCenter - 20)
            {
                Up = false;
            }
            else
            {
                Up = true;
            }

            if (X * Z > xCenter)
            {
                Right = false;
            }
            else
            {
                Right = true;
            }

            if (X * Z * -1 > xCenter)
            {
                Left = false;
            }
            else
            {
                Left = true;
            }
        }

        public void SetPosition(RawVector3 rawVector3)
        {
            X = rawVector3.X;
            Y = rawVector3.Y;
            Z = rawVector3.Z;
            UpdateVisibility();
            UpdateTranslationMatrix();
        }

        public void SetPosition(RawVector2 rawVector2)
        {
            X = rawVector2.X;
            Y = rawVector2.Y;
            UpdateVisibility();
            UpdateTranslationMatrix();
        }

        public void UpdateTranslationMatrix()
        {
            float x = X, y = Y, z = Z;
            x *= z;
            Y *= z;

            TranslationMatrix = Matrix.Translation(new RawVector3(x, y, z));
        }
    }
}