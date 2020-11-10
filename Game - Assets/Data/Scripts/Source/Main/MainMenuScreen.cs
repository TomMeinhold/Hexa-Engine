using HexaEngine.Core;
using HexaEngine.Core.UI;
using HexaEngine.Core.UI.BaseTypes;
using HexaEngine.Core.UI.Enum;
using HexaEngine.Core.UI.Events;
using HexaEngine.Core.UI.Structs;

namespace Main
{
    public class MainMenuScreen : Screen
    {
        public Button Button1;
        public Button Button2;
        public Button Button3;
        public Image Background;

        public MainMenuScreen()
        {
            Button1 = new Button() { Content = "Scene 1", Margin = new Thickness(0, 0, 0, 200) };
            Button2 = new Button() { Content = "Scene 2", Margin = new Thickness(0, 0, 0, 170) };
            Button3 = new Button() { Content = "Scene 3", Margin = new Thickness(0, 0, 0, 140) };
            SetDefaults(Button1);
            SetDefaults(Button2);
            SetDefaults(Button3);
            Button1.Click += Button1_Click;
            Button2.Click += Button2_Click;
            Button3.Click += Button3_Click;
            UserInterfaces.Add(Button1);
            UserInterfaces.Add(Button2);
            UserInterfaces.Add(Button3);
        }

        private void SetDefaults(UIElement element)
        {
            element.Border = new Thickness(5, 5, 5, 5);
            element.Padding = new Thickness(0, 0, 0, 0);
            element.HorizontalAlignment = HorizontalAlignment.Center;
            element.VerticalAlignment = VerticalAlignment.Center;
        }

        private void Button3_Click(object sender, MouseEventArgs e)
        {
            Engine.Current.SceneManager.SetSceneByType(typeof(Scene3), true);
        }

        private void Button2_Click(object sender, MouseEventArgs e)
        {
            Engine.Current.SceneManager.SetSceneByType(typeof(Scene2));
        }

        private void Button1_Click(object sender, MouseEventArgs e)
        {
            Engine.Current.SceneManager.SetSceneByType(typeof(Scene1));
        }
    }
}