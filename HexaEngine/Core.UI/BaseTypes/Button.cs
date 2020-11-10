using HexaEngine.Core.UI.Structs;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace HexaEngine.Core.UI.BaseTypes
{
    public class Button : UIElement
    {
        private TextLayout textLayout;
        private string content;

        public Button()
        {
            ForegroundBrush = new SolidColorBrush(Context, Color.Black);
            BackgroundBrush = new SolidColorBrush(Context, Color.White);
            BorderBrush = new SolidColorBrush(Context, Color.DarkGray);
            HighlightBrush = new SolidColorBrush(Context, Color.LightGray);
        }

        public Brush ForegroundBrush { get; set; }

        public Brush HighlightBrush { get; set; }

        public bool Invaildate { get; set; }

        public string Content { get => content; set { content = value; UpdateContent(); RecalculateBoundings(); } }

        public void UpdateContent()
        {
            textLayout = Engine.Current.RenderSystem.DirectWrite.GetTextLayout(content, Engine.Current.RenderSystem.DirectWrite.DefaultTextFormat, float.MaxValue);
        }

        public override Thickness GetContentSize()
        {
            return new Thickness(textLayout.Metrics.Height, 0, textLayout.Metrics.Width, 0);
        }

        public override void RenderContent(DeviceContext context)
        {
            if (textLayout != null)
            {
                context.DrawTextLayout(Vector2.Zero, textLayout, ForegroundBrush);
            }
        }
    }
}