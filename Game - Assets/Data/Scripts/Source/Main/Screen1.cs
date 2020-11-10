using HexaEngine.Core;
using HexaEngine.Core.UI.BaseTypes;
using HexaEngine.Core.UI.Events;
using HexaEngine.Core.UI.Structs;

namespace Main
{
    public class Screen1 : Screen
    {
        public Button Button1;
        public Button Button2;
        public Button Button3;
        public ProgressBar Progress;

        public Screen1()
        {
            Button1 = new Button() { Content = "Scene 1", Margin = new Thickness(0, 0, 0, 0) };
            Button2 = new Button() { Content = "Scene 2", Margin = new Thickness(30, 0, 0, 0) };
            Button3 = new Button() { Content = "Scene 3", Margin = new Thickness(60, 0, 0, 0) };
            Button1.Click += Button1_Click;
            Button2.Click += Button2_Click;
            Button3.Click += Button3_Click;
            UserInterfaces.Add(Button1);
            UserInterfaces.Add(Button2);
            UserInterfaces.Add(Button3);
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