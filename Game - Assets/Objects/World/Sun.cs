using HexaEngine.Core;
using HexaEngine.Core.Extentions;
using HexaEngine.Core.Input;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Interfaces;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Components;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GameAssets
{
    public class Sun : BaseObject, IInputKeyboard, IInputMouse, IBaseObject, IDrawable, IPhysicsObject, IRayCasting
    {
        public float Mass { get; set; } = 100;

        public Vector3 Velocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 Force { get; set; }

        public BlockedDirection Sides { get; set; }

        public float ForceAbsorbtion { get; set; }

        public float RayDensity { get; set; } = 1;

        public List<Ray> Rays { get; set; }

        public float RayRange { get; set; } = 500;

        public float StartAngle { get; set; } = 0;

        public float EndAngle { get; set; } = 360;

        public bool Static { get; set; }

        public Sun(Engine engine, Bitmap1 bitmap, RawVector3 position)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            SetPosition(position);
            MassCenter = BoundingBox.Center - Position;
            InputSystem.MouseUpdate += MouseInput;
            InputSystem.KeyboardUpdate += KeyboardInput;
            RayCastingModule = new RayCastingModule(this);
        }

        public void KeyboardInput(object sender, KeyboardUpdatePackage package)
        {
            if (package.KeyboardUpdate.Key == Keys.B && package.KeyboardUpdate.IsPressed)
            {
                if (RayCastingModule.Blur.StandardDeviation > 0)
                {
                    RayCastingModule.Blur.StandardDeviation = 0;
                }
                else
                {
                    RayCastingModule.Blur.StandardDeviation = 10;
                }
            }

            if (package.KeyboardUpdate.Key == Keys.R && package.KeyboardUpdate.IsPressed)
            {
                Rotate = !Rotate;
            }

            if (package.KeyboardUpdate.Key == Keys.L && package.KeyboardUpdate.IsPressed)
            {
                if (MouseHover)
                {
                    RaysEnabled = !RaysEnabled;
                }
            }
        }

        public bool MouseHover { get; set; }

        public bool MouseDown { get; set; }

        public bool Rotate { get; set; }

        public Vector3 MassCenter { get; set; }

        public Vector3 RotationVelocity { get; set; }

        public Vector3 RotationAcceleration { get; set; }

        public bool RaysEnabled { get; set; } = true;

        public Color4 GlowColor { get; set; } = System.Drawing.Color.FromArgb(255, 255, 255, 255).Convert();

        public RayCastingModule RayCastingModule { get; set; }

        public void MouseInput(object sender, MouseUpdatePackage package)
        {
            if (BoundingBox.ContainsVector(new Vector3(package.MouseState.LocationRaw.X, package.MouseState.LocationRaw.Y - Size.Height, package.MouseState.LocationRaw.Z)))
            {
                if (MouseDown)
                {
                    if (Rotate)
                    {
                        this.StartAngle = (this.StartAngle + package.MouseUpdate.Location.X) % 360;
                        this.EndAngle = (this.EndAngle + package.MouseUpdate.Location.X) % 360;
                    }
                    else
                    {
                        Vector3 oldPos = Position;
                        oldPos += package.MouseUpdate.Location;
                        SetPosition(oldPos);
                    }
                }

                MouseHover = true;
            }
            else
            {
                MouseHover = false;
            }

            if (package.MouseUpdate.MouseButton == MouseButtonUpdate.Left)
            {
                MouseDown = package.MouseUpdate.IsPressed;
            }
        }
    }
}