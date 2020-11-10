using HexaEngine.Core.Extensions;
using HexaEngine.Core.Input.Component;
using HexaEngine.Core.UI.Enum;
using HexaEngine.Core.UI.Interfaces;
using HexaEngine.Core.UI.Structs;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;

namespace HexaEngine.Core.UI.BaseTypes
{
    public class Button : IUserInterface
    {
        private BoundingBox boundingBox;

        private Vector3 position;

        private Size2F size;

        private Thickness margin;

        private string content;

        private TextLayout textLayout;
        private HorizontalAlignment horizontalAlignment;
        private VerticalAlignment verticalAlignment;
        private float width = float.NaN;
        private float height = float.NaN;

        public Button(Engine engine, Size2F size, Vector3 position)
        {
            Engine = engine;
            Context = engine.RenderSystem.DriectXManager.D2DDeviceContext;
            ForegroundBrush = new SolidColorBrush(Context, Color.Black);
            BackgroundBrush = new SolidColorBrush(Context, Color.White);
            BorderBrush = new SolidColorBrush(Context, Color.DarkGray);
            HighlightBrush = new SolidColorBrush(Context, Color.LightGray);
            Width = size.Width;
            Height = size.Height;
            SetPosition(position);
        }

        public Button(Engine engine, Vector3 position)
        {
            Engine = engine;
            Context = engine.RenderSystem.DriectXManager.D2DDeviceContext;
            ForegroundBrush = new SolidColorBrush(Context, Color.Black);
            BackgroundBrush = new SolidColorBrush(Context, Color.White);
            BorderBrush = new SolidColorBrush(Context, Color.DarkGray);
            HighlightBrush = new SolidColorBrush(Context, Color.LightGray);
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

        public float BorderWidth { get; set; } = 5;

        public float Width { get => width; set { width = value; RecalculateBounds(); } }

        public float Height { get => height; set { height = value; RecalculateBounds(); } }

        public HorizontalAlignment HorizontalAlignment { get => horizontalAlignment; set { horizontalAlignment = value; RecalculateBounds(); } }

        public VerticalAlignment VerticalAlignment { get => verticalAlignment; set { verticalAlignment = value; RecalculateBounds(); } }

        public Thickness Margin { get => margin; set { margin = value; RecalculateBounds(); } }

        public Vector3 Position { get => position; set => position = value; }

        public Bitmap1 CacheMap { get; set; }

        public bool Invaildate { get; set; }

        public string Content
        {
            get => content;
            set
            {
                content = value;
                RecalculateBounds();
                Invaildate = true;
            }
        }

        public virtual void SetPosition(Vector3 vector3)
        {
            Position = vector3;
            RecalculateBounds();
        }

        public void RecalculateBounds()
        {
            AutoSize();
            AutoPos();
            Vector3 max = new Vector3(Position.X + size.Width, Position.Y + size.Height, Position.Z);
            boundingBox = new BoundingBox(Position, max);
        }

        public Engine Engine { get; }

        public virtual void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
        }

        public virtual void MouseInput(MouseState state, MouseUpdate update)
        {
            if (boundingBox.ContainsVector(new Vector3(state.LocationRaw.X, state.LocationRaw.Y - size.Height, state.LocationRaw.Z)))
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
                CacheMap = Engine.RessouceManager.GetNewBitmap();
            }
            else if (CacheMap.IsDisposed)
            {
                CacheMap = Engine.RessouceManager.GetNewBitmap();
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
                context.DrawTextLayout(Vector2.Zero, textLayout, ForegroundBrush);
            }

            context.Target = targetbefore;
            context.Transform = (Matrix3x2)Matrix.Identity;
            context.DrawBitmap(CacheMap, 1, BitmapInterpolationMode.Linear);
        }

        public void AutoPos()
        {
            position = Vector3.Zero;
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    position = new Vector3(margin.Left, position.Y, position.Z);
                    break;

                case HorizontalAlignment.Center:
                    position = new Vector3(margin.Left, position.Y, position.Z);
                    break;

                case HorizontalAlignment.Right:
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    break;

                case VerticalAlignment.Center:
                    break;

                case VerticalAlignment.Bottom:
                    break;
            }
        }

        public void AutoSize()
        {
            if (Content is string str)
            {
                size = new Size2F(Width + BorderWidth, Height + BorderWidth);
                if (float.IsNaN(Width))
                {
                    textLayout = Engine.RenderSystem.DirectWrite.GetTextLayout(str, Engine.RenderSystem.DirectWrite.DefaultTextFormat, float.MaxValue);
                    size = new Size2F(textLayout.Metrics.Width + BorderWidth, size.Height);
                }
                else
                {
                    textLayout = Engine.RenderSystem.DirectWrite.GetTextLayout(str, Engine.RenderSystem.DirectWrite.DefaultTextFormat, Width);
                }

                if (float.IsNaN(Height))
                {
                    size = new Size2F(size.Width, textLayout.Metrics.Height + BorderWidth);
                }
            }
        }

        public Size2F MeasureString(TextLayout layout)
        {
            return new Size2F(layout.Metrics.Width, layout.Metrics.Height);
        }
    }
}