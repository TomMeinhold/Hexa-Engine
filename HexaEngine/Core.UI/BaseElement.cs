using HexaEngine.Core.Input.Component;
using HexaEngine.Core.UI.Enum;
using HexaEngine.Core.UI.Interfaces;
using HexaEngine.Core.UI.Structs;
using SharpDX;
using SharpDX.Direct2D1;

namespace HexaEngine.Core.UI
{
    public class BaseElement : IUserInterface
    {
        public Brush BackgroundBrush;

        public Brush BorderBrush;

        public HorizontalAlignment HorizontalAlignment { get => horizontalAlignment; set { horizontalAlignment = value; RecalculateBoundings(); } }

        public VerticalAlignment VerticalAlignment { get => verticalAlignment; set { verticalAlignment = value; RecalculateBoundings(); } }

        public Thickness Padding { get => padding; set { padding = value; RecalculateBoundings(); } }

        public Thickness Margin { get => margin; set { margin = value; RecalculateBoundings(); } }

        public Thickness Border { get => border; set { border = value; RecalculateBoundings(); } }

        public float Height { get => height; set { height = value; RecalculateBoundings(); } }

        public float Width { get => width; set { width = value; RecalculateBoundings(); } }

        public float ActualHeight { get; private set; }

        public float ActualWidth { get; private set; }

        public float InnerHeight { get; private set; }

        public float InnerWidth { get; private set; }

        public float ContentHeight { get; private set; }

        public float ContentWidth { get; private set; }

        public BoundingBox BoundingBox { get; private set; }

        public RectangleF OuterRectangle;

        public RectangleF InnerRectangle;

        public RectangleF ContentRectangle;

        public Vector3 AbsoluteCenteredPosition;

        public Vector3 AbsolutePosition;

        public Vector3 AbsolutePositionExtented;

        public Vector3 AbsoluteInnerPosition;

        public Vector3 AbsoluteInnerPositionExtented;

        public Vector3 AbsoluteContentPosition;

        public Vector3 AbsoluteContentPositionExtented;

        public Thickness AbsoluteThickness;

        public Matrix3x2 PositionMatrix;

        public Matrix3x2 PositionInnerMatrix;

        public Matrix3x2 PositionContentMatrix;
        private HorizontalAlignment horizontalAlignment;
        private VerticalAlignment verticalAlignment;
        private Thickness padding;
        private Thickness margin;
        private Thickness border;
        private float height = float.NaN;
        private float width = float.NaN;

        public DeviceContext Context { get => Engine.Current.RenderSystem.DriectXManager.D2DDeviceContext; }

        public virtual void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
            return;
        }

        public virtual void MouseInput(MouseState state, MouseUpdate update)
        {
            return;
        }

        public virtual void Render(DeviceContext context)
        {
            if (BackgroundBrush is null)
            {
                BackgroundBrush = new SolidColorBrush(context, Color.Red);
                BorderBrush = new SolidColorBrush(context, Color.DarkGray);
            }
            var before = context.Transform;
            context.Transform = PositionMatrix;
            context.FillRectangle(OuterRectangle, BorderBrush);
            context.Transform = PositionInnerMatrix;
            context.FillRectangle(InnerRectangle, BackgroundBrush);
            context.Transform = PositionContentMatrix;
            RenderContent(context);
            context.Transform = before;
        }

        public virtual void RenderContent(DeviceContext context)
        {
            return;
        }

        public void RecalculateBoundings()
        {
            CalculateAbsolutePosition();
            CaclulateAbsoluteSize();
            BoundingBox = new BoundingBox(AbsolutePosition, AbsolutePosition + new Vector3(ActualWidth, ActualHeight, 0));
        }

        private void CalculateAbsolutePosition()
        {
            AbsoluteThickness = GetContentSize() + Padding + Border;
            AbsoluteCenteredPosition = Vector3.Zero;
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    AbsoluteCenteredPosition.X = Margin.Left + AbsoluteThickness.Left;
                    break;

                case HorizontalAlignment.Center:
                    AbsoluteCenteredPosition.X = Margin.Left + (Engine.Current.RenderSystem.Form.Width / 2) - Margin.Right;
                    break;

                case HorizontalAlignment.Right:
                    AbsoluteCenteredPosition.X = Engine.Current.RenderSystem.Form.Width - Margin.Right;
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    AbsoluteCenteredPosition.Y = Margin.Top + AbsoluteThickness.Top;
                    break;

                case VerticalAlignment.Center:
                    AbsoluteCenteredPosition.Y = Margin.Top + (Engine.Current.RenderSystem.Form.Height / 2) - Margin.Bottom;
                    break;

                case VerticalAlignment.Bottom:
                    AbsoluteCenteredPosition.Y = Engine.Current.RenderSystem.Form.Height - Margin.Bottom;
                    break;
            }
        }

        private void CaclulateAbsoluteSize()
        {
            AbsolutePosition = AbsoluteCenteredPosition;
            AbsolutePositionExtented = AbsoluteCenteredPosition;
            if (!float.IsNaN(Height))
            {
                AbsolutePosition.Y -= Height / 2;
                AbsolutePositionExtented.Y += Height / 2;
            }
            else
            {
                AbsolutePosition.Y -= AbsoluteThickness.Top;
                AbsolutePositionExtented.Y += AbsoluteThickness.Bottom;
            }

            if (!float.IsNaN(Width))
            {
                AbsolutePosition.Y -= Width / 2;
                AbsolutePositionExtented.Y += Width / 2;
            }
            else
            {
                AbsolutePosition.X -= AbsoluteThickness.Left;
                AbsolutePositionExtented.X += AbsoluteThickness.Right;
            }

            ActualWidth = AbsolutePositionExtented.X - AbsolutePosition.X;
            ActualHeight = AbsolutePositionExtented.Y - AbsolutePosition.Y;

            if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                AbsolutePosition.X -= ActualWidth;
                AbsolutePositionExtented.X -= ActualWidth;
            }

            if (VerticalAlignment == VerticalAlignment.Bottom)
            {
                AbsolutePosition.Y -= ActualHeight;
                AbsolutePositionExtented.Y -= ActualHeight;
            }

            ActualWidth = AbsolutePositionExtented.X - AbsolutePosition.X;
            ActualHeight = AbsolutePositionExtented.Y - AbsolutePosition.Y;

            AbsoluteInnerPosition = AbsolutePosition;
            AbsoluteInnerPosition.X += Border.Left;
            AbsoluteInnerPosition.Y += Border.Top;
            AbsoluteInnerPositionExtented = AbsolutePositionExtented;
            AbsoluteInnerPositionExtented.X -= Border.Right;
            AbsoluteInnerPositionExtented.Y -= Border.Bottom;
            InnerWidth = AbsoluteInnerPositionExtented.X - AbsoluteInnerPosition.X;
            InnerHeight = AbsoluteInnerPositionExtented.Y - AbsoluteInnerPosition.Y;

            AbsoluteContentPosition = AbsoluteInnerPosition;
            AbsoluteContentPosition.X += Padding.Left;
            AbsoluteContentPosition.Y += Padding.Top;
            AbsoluteContentPositionExtented = AbsoluteInnerPositionExtented;
            AbsoluteContentPositionExtented.X -= Padding.Right;
            AbsoluteContentPositionExtented.Y -= Padding.Bottom;
            ContentWidth = AbsoluteContentPositionExtented.X - AbsoluteContentPosition.X;
            ContentHeight = AbsoluteContentPositionExtented.Y - AbsoluteContentPosition.Y;

            OuterRectangle = new RectangleF(0, 0, ActualWidth, ActualHeight);
            InnerRectangle = new RectangleF(0, 0, InnerWidth, InnerHeight);
            ContentRectangle = new RectangleF(0, 0, ContentWidth, ContentHeight);
            PositionMatrix = Matrix.Translation(AbsolutePosition);
            PositionInnerMatrix = Matrix.Translation(AbsoluteInnerPosition);
            PositionContentMatrix = Matrix.Translation(AbsoluteContentPosition);
        }

        public virtual Thickness GetContentSize()
        {
            return Thickness.Zero;
        }
    }
}