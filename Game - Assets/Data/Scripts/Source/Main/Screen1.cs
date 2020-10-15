using HexaEngine.Core;
using HexaEngine.Core.UI.BaseTypes;
using SharpDX;

namespace Main
{
    public class Screen1 : Screen
    {
        public Button Button1;
        public Button Button2;
        public Button Button3;

        public Engine Engine { get; }

        public Screen1(Engine engine)
        {
            Engine = engine;
            Button1 = new Button(engine, new Size2F(60, 20), new Vector3(0, 50, 0)) { Content = "Scene 1", };
            Button2 = new Button(engine, new Size2F(60, 20), new Vector3(60, 50, 0)) { Content = "Scene 2", };
            Button3 = new Button(engine, new Size2F(60, 20), new Vector3(120, 50, 0)) { Content = "Scene 3", };
            Button1.Click += Button1_Click;
            Button2.Click += Button2_Click;
            Button3.Click += Button3_Click;
            UserInterfaces.Add(Button1);
            UserInterfaces.Add(Button2);
            UserInterfaces.Add(Button3);
        }

        private void Button3_Click(object sender, System.EventArgs e)
        {
            Engine.SceneManager.SetSceneByType(typeof(Scene3), true);
        }

        private void Button2_Click(object sender, System.EventArgs e)
        {
            Engine.SceneManager.SetSceneByType(typeof(Scene2));
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            Engine.SceneManager.SetSceneByType(typeof(Scene1));
        }
    }
}