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
using SharpDX.Mathematics.Interop;
using System.Windows.Forms;

namespace GameAssets.Objects.World
{
    public class Planet : BaseObject, IBaseObject, IDrawable, IPhysicsObject, IInputKeyboard
    {
        public float Mass { get; set; } = 1;

        public Vector3 Velocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 Force { get; set; }

        public BlockedDirection Sides { get; set; }

        public float ForceAbsorbtion { get; set; }

        public bool Static { get; set; }

        public Planet(Engine engine, Bitmap1 bitmap, RawVector3 position)
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

        public void Render(DeviceContext deviceContext)
        {
            deviceContext.Target = Engine.RenderSystem.RessouceManager.ObjectsBitmap;
            using var brush = new SolidColorBrush(deviceContext, Color.Red);
            string acceleration = $"{Acceleration.X}ms², {Acceleration.Y}ms²";
            string speed = $"{Velocity.X}ms, {Velocity.Y}ms";

            deviceContext.DrawText(acceleration, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 150, 100, 50), brush);
            deviceContext.DrawText(speed, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 200, 100, 50), brush);
            deviceContext.Transform = (Matrix3x2)Matrix.Translation(Position.X, Position.Y * -1 + Size.Height, Position.Z);

            deviceContext.DrawBitmap(Bitmap, 1, BitmapInterpolationMode.Linear);
            deviceContext.DrawLine(new Vector2(this.BoundingBox.Center.X + 10 - Position.X, this.BoundingBox.Center.Y - Position.Y), new Vector2(this.BoundingBox.Center.X - 10 - Position.X, this.BoundingBox.Center.Y - Position.Y), brush);
            deviceContext.DrawLine(new Vector2(this.BoundingBox.Center.X - Position.X, this.BoundingBox.Center.Y + 10 - Position.Y), new Vector2(this.BoundingBox.Center.X - Position.X, this.BoundingBox.Center.Y - 10 - Position.Y), brush);
            deviceContext.DrawRectangle(this.BoundingBox.BoundingBoxToRectNoPos(), brush);
            deviceContext.Transform = (Matrix3x2)Matrix.Identity;
        }
    }
}