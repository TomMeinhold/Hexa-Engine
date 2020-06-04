using HexaEngine.Core;
using HexaEngine.Core.Extentions;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Interfaces;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
using SharpDX.Mathematics.Interop;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GameAssets
{
    public class Sun : BaseObject, IInputKeyboard, IInputMouse, IBaseObject, IDrawable, IPhysicsObject, IRayCasting
    {
        private Bitmap1 GodRayMap;

        public float Mass { get; set; } = 100;

        public Vector3 Velocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 Force { get; set; }

        public BlockedDirection Sides { get; set; }

        public float ForceAbsorbtion { get; set; }

        public float RayDensity { get; set; } = 1;

        public List<Ray> Rays { get; set; }

        public float RayRange { get; set; } = 500;

        public Color GlowColor { get; set; } = System.Drawing.Color.FromArgb(255, 255, 255, 255).Convert();

        public float StartAngle { get; set; } = 0;

        public float EndAngle { get; set; } = 360;

        private float Blur { get; set; } = 10;

        public bool Static { get; set; }

        public Sun(Engine engine, Bitmap1 bitmap, RawVector3 position)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            SetPosition(position);
            engine.InputSystem.InputKeyboards.Add(this);
            engine.InputSystem.InputMice.Add(this);
            State = BaseObjectState.Initialized;
        }

        public void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
            if (update.Key == Keys.A && update.IsPressed)
            {
                this.Force = new Vector3(-0.01F, 0, 0);
            }

            if (update.Key == Keys.A && !update.IsPressed)
            {
                this.Force = new Vector3(0, 0, 0);
            }

            if (update.Key == Keys.W && update.IsPressed)
            {
                this.Force = new Vector3(0, 0.01F, 0);
            }

            if (update.Key == Keys.W && !update.IsPressed)
            {
                this.Force = new Vector3(0, 0, 0);
            }

            if (update.Key == Keys.S && update.IsPressed)
            {
                this.Force = new Vector3(0, -0.01F, 0);
            }

            if (update.Key == Keys.S && !update.IsPressed)
            {
                this.Force = new Vector3(0, 0, 0);
            }

            if (update.Key == Keys.D && update.IsPressed)
            {
                this.Force = new Vector3(0.01F, 0, 0);
            }

            if (update.Key == Keys.D && !update.IsPressed)
            {
                this.Force = new Vector3(0, 0, 0);
            }

            if (update.Key == Keys.B && update.IsPressed)
            {
                if (Blur > 0)
                {
                    Blur = 0;
                }
                else
                {
                    Blur = 10;
                }
            }

            if (update.Key == Keys.R && update.IsPressed)
            {
                Rotate = !Rotate;
            }

            if (update.Key == Keys.L && update.IsPressed)
            {
                if (MouseHover)
                {
                    LightingEnabled = !LightingEnabled;
                }
            }
        }

        public void Render(DeviceContext deviceContext)
        {
            deviceContext.Transform = (Matrix3x2)Matrix.Identity;
            if (GodRayMap is null)
            {
                GodRayMap = Engine.RenderSystem.RessouceManager.GetNewBitmap();
            }

            deviceContext.Target = GodRayMap;
            deviceContext.Clear(Color.Transparent);
            using var brush = new SolidColorBrush(deviceContext, GlowColor);
            if (!(Rays is null) && LightingEnabled)
            {
                lock (Rays)
                {
                    foreach (Ray ray in Rays)
                    {
                        deviceContext.DrawLine(ray.Position.Downgrade(), ray.Direction.Downgrade(), brush);
                    }
                }
            }

            using GaussianBlur blur = new GaussianBlur(deviceContext)
            {
                StandardDeviation = Blur
            };
            blur.SetInput(0, GodRayMap, true);

            deviceContext.Target = Engine.RenderSystem.RessouceManager.RayBitmap;
            deviceContext.DrawImage(blur, InterpolationMode.HighQualityCubic, CompositeMode.Plus);
            deviceContext.Target = Engine.RenderSystem.RessouceManager.ObjectsBitmap;
            deviceContext.Transform = (Matrix3x2)Matrix.Translation(Position.X, Position.Y * -1, Position.Z);

            deviceContext.DrawBitmap(Bitmap, 1, BitmapInterpolationMode.Linear);
            deviceContext.DrawText($"{Force.X}, {((IPhysicsObject)this).Acceleration.X}, {((IPhysicsObject)this).Acceleration.Y}, {Velocity.X}", Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 0, 100, 50), brush);
            deviceContext.DrawLine(new Vector2(this.BoundingBox.Center.X + 10 - Position.X, this.BoundingBox.Center.Y - Position.Y), new Vector2(this.BoundingBox.Center.X - 10 - Position.X, this.BoundingBox.Center.Y - Position.Y), brush);
            deviceContext.DrawLine(new Vector2(this.BoundingBox.Center.X - Position.X, this.BoundingBox.Center.Y + 10 - Position.Y), new Vector2(this.BoundingBox.Center.X - Position.X, this.BoundingBox.Center.Y - 10 - Position.Y), brush);
            deviceContext.DrawRectangle(this.BoundingBox.BoundingBoxToRectNoPos(), brush);
            deviceContext.Transform = (Matrix3x2)Matrix.Identity;
        }

        public bool MouseHover { get; set; }

        public bool MouseDown { get; set; }

        public bool Rotate { get; set; }

        public bool LightingEnabled { get; set; } = true;

        public void MouseInput(MouseState state, MouseUpdate update)
        {
            if (BoundingBox.ContainsVector(state.LocationRaw))
            {
                if (MouseDown)
                {
                    if (Rotate)
                    {
                        this.StartAngle = (this.StartAngle + update.Location.X) % 360;
                        this.EndAngle = (this.EndAngle + update.Location.X) % 360;
                    }
                    else
                    {
                        Vector3 oldPos = Position;
                        oldPos += update.Location;
                        SetPosition(oldPos);
                    }
                }

                MouseHover = true;
            }
            else
            {
                MouseHover = false;
            }

            if (update.MouseButton == MouseButtonUpdate.Left)
            {
                MouseDown = update.IsPressed;
            }
        }
    }
}