using HexaEngine.Core.Extensions;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.UI.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using System;

namespace HexaEngine.Core.UI.BaseTypes
{
    public class ProgressBar : IUserInterface
    {
        private BoundingBox boundingBox;

        private Vector3 position;

        private Size2F size;

        private object content;

        public ProgressBar(Engine engine, Size2F size, Vector3 position)
        {
            Engine = engine;
            Context = engine.RenderSystem.DriectXManager.D2DDeviceContext;
            ForegroundBrush = new SolidColorBrush(Context, Color.Black);
            BackgroundBrush = new SolidColorBrush(Context, Color.White);
            BorderBrush = new SolidColorBrush(Context, Color.DarkGray);
            Size = size;
            SetPosition(position);
        }

        public int Value { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public bool Focus { get; set; }

        public Brush ForegroundBrush { get; set; }

        public Brush BackgroundBrush { get; set; }

        public Brush BorderBrush { get; set; }

        public DeviceContext Context { get; }

        public Size2F Size { get => size; set => size = value; }

        public Vector3 Position { get => position; set => position = value; }

        public Bitmap1 CacheMap { get; set; }

        public bool Invaildate { get; set; }

        public virtual void SetPosition(Vector3 vector3)
        {
            Position = vector3;
            Vector3 max = new Vector3(vector3.X + Size.Width, vector3.Y + Size.Height, vector3.Z);
            boundingBox = new BoundingBox(vector3, max);
        }

        public Engine Engine { get; }

        public void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
            return;
        }

        public void MouseInput(MouseState state, MouseUpdate update)
        {
            return;
        }

        public void Render(DeviceContext context)
        {
            if (CacheMap is null)
            {
                CacheMap = Engine.RessouceManager.GetNewBitmap();
            }
            else if (CacheMap.IsDisposed)
            {
                CacheMap = Engine.RessouceManager.GetNewBitmap();
            }

            context.Transform = (Matrix3x2)Matrix.Translation(Position);
            var rect = boundingBox.BoundingBoxToRectNoPos();

            rect.Location += 5;
            var targetbefore = context.Target;
            context.Target = CacheMap;
            if (Invaildate)
            {
                Invaildate = false;

                context.DrawRectangle(rect, BackgroundBrush);
            }

            context.Target = targetbefore;
            context.Transform = (Matrix3x2)Matrix.Identity;
            context.DrawBitmap(CacheMap, 1, BitmapInterpolationMode.Linear);
        }
    }
}