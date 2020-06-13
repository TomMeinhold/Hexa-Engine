using HexaEngine.Core;
using HexaEngine.Core.Extentions;
using HexaEngine.Core.Input;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Input.Interfaces;
using HexaEngine.Core.Mathmatics;
using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Runtime.CompilerServices;
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

        public Vector3 MassCenter { get; set; }

        public Vector3 RotationVelocity { get; set; }

        public Vector3 RotationAcceleration { get; set; }

        public Planet(Engine engine, Bitmap1 bitmap, RawVector3 position)
        {
            Engine = engine;
            Bitmap = bitmap;
            Size = bitmap.Size;
            SetPosition(position);
            MassCenter = BoundingBox.Center - Position;
            InputSystem.KeyboardUpdate += KeyboardInput;
        }

        public void KeyboardInput(object sender, KeyboardUpdatePackage package)
        {
            KeyboardUpdate update = package.KeyboardUpdate;
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

        public override void Render(DeviceContext deviceContext)
        {
            base.Render(deviceContext);
            DrawDebugInfo(deviceContext);
        }

        private void DrawDebugInfo(DeviceContext deviceContext)
        {
            using var brush = new SolidColorBrush(deviceContext, Color.Red);
            string force = $"{Force.X}N, {Force.Y}N";
            string acceleration = $"{Acceleration.X}m/s², {Acceleration.Y}m/s² {Acceleration.Z}m/s²";
            string speed = $"{Velocity.X}m/s, {Velocity.Y}m/s {Velocity.Z}m/s";
            string pos = $"{Position.X}m, {Position.Y}m {Position.Z}m";
            string angleAcceleration = $"{RotationAcceleration.X}deg/s², {RotationAcceleration.Y}deg/s², {RotationAcceleration.Z}deg/s²";
            string angleSpeed = $"{RotationVelocity.X}deg/s, {RotationVelocity.Y}deg/s, {RotationVelocity.Z}deg/s";
            string angle = $"{Rotation.X}deg, {Rotation.Y}deg, {Rotation.Z}deg";
            deviceContext.Transform = (Matrix3x2)Matrix.Identity;
            deviceContext.DrawText(force, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 150, 300, 50), brush);
            deviceContext.DrawText(acceleration, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 200, 300, 50), brush);
            deviceContext.DrawText(speed, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 250, 300, 50), brush);
            deviceContext.DrawText(angleAcceleration, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 300, 300, 50), brush);
            deviceContext.DrawText(angleSpeed, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 350, 300, 50), brush);
            deviceContext.DrawText(pos, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 400, 300, 50), brush);
            deviceContext.DrawText(angle, Engine.RenderSystem.DirectWrite.DefaultTextFormat, new RectangleF(0, 450, 300, 50), brush);
            deviceContext.Transform = (Matrix3x2)Matrix.Translation(Position);
            deviceContext.DrawLine(new Vector2(this.MassCenter.X + 10, this.MassCenter.Y), new Vector2(this.MassCenter.X - 10, this.MassCenter.Y), brush);
            deviceContext.DrawLine(new Vector2(this.MassCenter.X, this.MassCenter.Y + 10), new Vector2(this.MassCenter.X, this.MassCenter.Y - 10), brush);
            deviceContext.DrawRectangle(this.BoundingBox.BoundingBoxToRectNoPos(), brush);
        }
    }
}