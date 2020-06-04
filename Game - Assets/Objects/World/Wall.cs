using HexaEngine.Core;
using HexaEngine.Core.Extentions;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Interfaces;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
using SharpDX.Mathematics.Interop;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GameAssets
{
    public class Wall : BaseObject, IInputKeyboard, IPhysicsObject
    {
        public float Mass { get; set; } = 100;

        public Vector3 Velocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 Force { get; set; }

        public BlockedDirection Sides { get; set; }

        public float ForceAbsorbtion { get; set; }

        public float ReflectionStrength { get; set; } = 1000;

        public Color ReflectionColor { get; set; } = System.Drawing.Color.FromArgb(255, 255, 255, 255).Convert();

        public List<Ray> ReflectedRays { get; set; }

        public Bitmap1 GodRayMap { get; private set; }

        public bool Static { get; set; }

        public Wall(Engine engine, Bitmap1 bitmap, RawVector3 position)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            SetPosition(position);
            SetRotation(0);
            engine.InputSystem.InputKeyboards.Add(this);
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

            if (update.Key == Keys.U && update.IsPressed)
            {
                Static = !Static;
            }
        }

        public override void Draw(DeviceContext deviceContext)
        {
            deviceContext.Transform = (Matrix3x2)Matrix.Identity;
            if (GodRayMap is null)
            {
                GodRayMap = Engine.RenderSystem.RessouceManager.GetNewBitmap();
            }

            deviceContext.Target = GodRayMap;
            deviceContext.Clear(Color.Transparent);
            using var brush = new SolidColorBrush(deviceContext, ReflectionColor);
            if (!(ReflectedRays is null))
            {
                lock (ReflectedRays)
                {
                    foreach (Ray ray in ReflectedRays)
                    {
                        deviceContext.DrawLine(ray.Position.Downgrade(), ray.Direction.Downgrade(), brush);
                    }
                }
            }

            using GaussianBlur blur = new GaussianBlur(deviceContext)
            {
                StandardDeviation = 0
            };
            blur.SetInput(0, GodRayMap, true);

            deviceContext.Target = Engine.RenderSystem.RessouceManager.RayBitmap;
            deviceContext.DrawImage(blur);
            deviceContext.Target = Engine.RenderSystem.RessouceManager.ObjectsBitmap;
            deviceContext.Transform = (Matrix3x2)Matrix.Translation(Position.X, Position.Y * -1, Position.Z);
            deviceContext.DrawBitmap(Bitmap, 1, BitmapInterpolationMode.Linear);
            deviceContext.Transform = (Matrix3x2)Matrix.Identity;
        }
    }
}