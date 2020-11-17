using HexaEngine.Core.Input.Component;
using HexaEngine.Core.Timers;
using HexaEngine.Core.UI.Structs;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Keys = HexaEngine.Core.Input.Component.Keys;
using Timer = HexaEngine.Core.Timers.Timer;

namespace HexaEngine.Core.UI.BaseTypes
{
    public class TextBox : UIElement
    {
        private TextLayout textLayout;
        private string text = string.Empty;
        private bool upper = false;
        private bool cursorshow;

        public TextBox()
        {
            ForegroundBrush = new SolidColorBrush(Context, Color.Black);
            BackgroundBrush = new SolidColorBrush(Context, Color.White);
            BorderBrush = new SolidColorBrush(Context, Color.DarkGray);
            HighlightBrush = new SolidColorBrush(Context, Color.LightGray);
            Timer timer = new Timer(new System.TimeSpan(0, 0, 0, 0, 500));
            timer.Start();
            timer.TimerTick += Timer_TimerTick;
        }

        private void Timer_TimerTick(object sender, TimerTickEventArgs e)
        {
            cursorshow = !cursorshow;
        }

        public Brush ForegroundBrush { get; set; }

        public Brush HighlightBrush { get; set; }

        public string Text { get => text; set { text = value; UpdateContent(); RecalculateBoundings(); } }

        public Thickness GetThickness(string str, float width)
        {
            // measure text width including white spaces
            using var tl = Engine.Current.RenderSystem.DirectWrite.GetTextLayout(str, Engine.Current.RenderSystem.DirectWrite.DefaultTextFormat, width);
            using var tl0 = Engine.Current.RenderSystem.DirectWrite.GetTextLayout("A", Engine.Current.RenderSystem.DirectWrite.DefaultTextFormat, width);
            using var tl1 = Engine.Current.RenderSystem.DirectWrite.GetTextLayout(str + "A", Engine.Current.RenderSystem.DirectWrite.DefaultTextFormat, width);
            int result = (int)(tl1.Metrics.Width - tl0.Metrics.Width);
            return new Thickness(0, result > width ? width : result, 0, tl.Metrics.Height);
        }

        public void UpdateContent()
        {
            textLayout = Engine.Current.RenderSystem.DirectWrite.GetTextLayout(text, Engine.Current.RenderSystem.DirectWrite.DefaultTextFormat, float.MaxValue);
        }

        public override Thickness GetContentSize()
        {
            UpdateContent();
            return GetThickness(Text, float.MaxValue);
        }

        public override void RenderContent(DeviceContext context)
        {
            if (textLayout != null)
            {
                context.DrawTextLayout(Vector2.Zero, textLayout, ForegroundBrush);
                if (Focus && cursorshow)
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
                    upper = update.IsPressed;
                    return;
                }

                if (update.IsPressed && update.Key == Keys.Space)
                {
                    Text += " ";
                    return;
                }

                if (update.IsPressed && update.Key == Keys.Enter)
                {
                    Text += "\n";
                    return;
                }

                if (update.IsPressed && update.Key == Keys.Back)
                {
                    if (Text.Length > 0)
                    {
                        Text = Text.Remove(Text.Length - 1);
                    }

                    return;
                }

                if (update.IsPressed)
                {
                    KeysConverter converter = new KeysConverter();
                    if (upper)
                    {
                        Text += converter.ConvertToString(update.Key);
                    }
                    else
                    {
                        Text += converter.ConvertToString(update.Key).ToLower();
                    }
                }
            }
        }
    }
}