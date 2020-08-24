using HexaEngine.Core.Extensions;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.UI.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexaEngine.Core.UI.BaseTypes
{
    public class Button : IUserInterface
    {
        private BoundingBox boundingBox;

        private Vector3 position;

        private Size2F size;

        private object content;

        public Button(Engine engine, Size2F size, Vector3 position)
        {
            Engine = engine;
            Context = engine.RenderSystem.RessouceManager.D2DDeviceContext;
            ForegroundBrush = new SolidColorBrush(Context, Color.Black);
            BackgroundBrush = new SolidColorBrush(Context, Color.White);
            BorderBrush = new SolidColorBrush(Context, Color.DarkGray);
            HighlightBrush = new SolidColorBrush(Context, Color.LightGray);
            Size = size;
            SetPosition(position);
        }

        public event EventHandler Click;

        public bool MouseHover { get; set; }

        public bool MousePress { get; set; }

        public bool Focus { get; set; }

        public Brush ForegroundBrush { get; set; }

        public Brush BackgroundBrush { get; set; }

        public Brush BorderBrush { get; set; }

        public Brush HighlightBrush { get; set; }

        public DeviceContext Context { get; }

        public Size2F Size { get => size; set => size = value; }

        public Vector3 Position { get => position; set => position = value; }

        public Bitmap1 CacheMap { get; set; }

        public bool Invaildate { get; set; }

        public object Content
        {
            get => content;
            set
            {
                Invaildate = true;
                content = value;
            }
        }

        public virtual void SetPosition(Vector3 vector3)
        {
            this.Position = vector3;
            Vector3 max = new Vector3(vector3.X + Size.Width, vector3.Y + Size.Height, vector3.Z);
            boundingBox = new BoundingBox(vector3, max);
        }

        public Engine Engine { get; }

        public virtual void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
        }

        public virtual void MouseInput(MouseState state, MouseUpdate update)
        {
            if (boundingBox.ContainsVector(new Vector3(state.LocationRaw.X, state.LocationRaw.Y - Size.Height, state.LocationRaw.Z)))
            {
                MouseHover = true;
            }
            else
            {
                MouseHover = false;
            }

            if (MouseHover)
            {
                if (update.MouseButton == MouseButtonUpdate.Left && update.IsPressed)
                {
                    MousePress = true;
                }
                if (update.MouseButton == MouseButtonUpdate.Left && !update.IsPressed)
                {
                    Click?.Invoke(this, null);
                }
            }

            if (update.MouseButton == MouseButtonUpdate.Left && !update.IsPressed)
            {
                MousePress = false;
            }
        }

        public virtual void Render(DeviceContext context)
        {
            if (CacheMap is null)
            {
                CacheMap = Engine.RenderSystem.RessouceManager.GetNewBitmap();
            }
            else if (CacheMap.IsDisposed)
            {
                CacheMap = Engine.RenderSystem.RessouceManager.GetNewBitmap();
            }

            context.Transform = (Matrix3x2)Matrix.Translation(Position);
            var rect = boundingBox.BoundingBoxToRectNoPos();
            if (MouseHover)
            {
                context.FillRectangle(rect, HighlightBrush);
            }
            else
            {
                context.FillRectangle(rect, BackgroundBrush);
            }

            if (MousePress)
            {
                context.FillRectangle(rect, HighlightBrush);
            }
            else
            {
                context.DrawRectangle(rect, BorderBrush, 5);
            }

            rect.Location += 5;
            var targetbefore = context.Target;
            context.Target = CacheMap;
            if (Invaildate)
            {
                Invaildate = false;
                if (Content is string str)
                {
                    context.DrawText(str, Engine.RenderSystem.DirectWrite.DefaultTextFormat, rect, ForegroundBrush);
                }

                if (Content is IUserInterface ui)
                {
                    ui.Render(context);
                }
            }

            context.Target = targetbefore;
            context.Transform = (Matrix3x2)Matrix.Identity;
            context.DrawBitmap(CacheMap, 1, BitmapInterpolationMode.Linear);
        }
    }
}