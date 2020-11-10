using HexaEngine.Core.Input.Component;
using HexaEngine.Core.UI.Structs;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System.Linq;

namespace HexaEngine.Core.UI.BaseTypes
{
    public class TextBox : UIElement
    {
        private TextLayout textLayout;
        private string text = string.Empty;
        private bool upper;

        public TextBox()
        {
            ForegroundBrush = new SolidColorBrush(Context, Color.Black);
            BackgroundBrush = new SolidColorBrush(Context, Color.White);
            BorderBrush = new SolidColorBrush(Context, Color.DarkGray);
            HighlightBrush = new SolidColorBrush(Context, Color.LightGray);
        }

        public Brush ForegroundBrush { get; set; }

        public Brush HighlightBrush { get; set; }

        public string Text { get => text; set { text = value; UpdateContent(); RecalculateBoundings(); } }

        public void UpdateContent()
        {
            textLayout = Engine.Current.RenderSystem.DirectWrite.GetTextLayout(text, Engine.Current.RenderSystem.DirectWrite.DefaultTextFormat, float.MaxValue);
        }

        public override Thickness GetContentSize()
        {
            UpdateContent();
            return new Thickness(textLayout.Metrics.Height, 0, textLayout.Metrics.Width, 0);
        }

        public override void RenderContent(DeviceContext context)
        {
            if (textLayout != null)
            {
                context.DrawTextLayout(Vector2.Zero, textLayout, ForegroundBrush);
                if (Focus)
                {
                    context.DrawLine(new Vector2(textLayout.Metrics.Width, textLayout.Metrics.Height), new Vector2(textLayout.Metrics.Width, 0), ForegroundBrush);
                }
            }
        }

        public override void KeyboardInput(KeyboardState state, KeyboardUpdate update)
        {
            if (Focus)
            {
                if (update.Key == Keys.ShiftKey)
                {
                    upper = !update.IsPressed;
                    return;
                }

                if (update.IsPressed && update.Key == Keys.Space)
                {
                    Text += " ";
                    return;
                }

                if (update.IsPressed && update.Key == Keys.Back)
                {
                    var text = Text.ToList();
                    text.RemoveAt(text.Count - 1);
                    Text = string.Join("", text);
                    return;
                }

                if (update.IsPressed)
                {
                    if (upper)
                    {
                        Text += update.Key.ToString().ToLower();
                    }
                    else
                    {
                        Text += update.Key.ToString();
                    }
                }
            }
        }
    }
}