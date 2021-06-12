using HexaFramework.Models;
using HexaFramework.Windows;
using System.Collections.Generic;
using System.Numerics;

namespace HexaFramework.Scenes
{
    public class RenderScene
    {
        private bool firstFrame = true;

        public RenderScene(RenderWindow window)
        {
            Window = window;
        }

        public List<SceneObject> Objects { get; } = new();

        public List<Camera> Cameras { get; } = new();

        public RenderWindow Window { get; }

        public Matrix4x4 WorldMatrix { get; set; } = Matrix4x4.Identity;

        public void Render()
        {
            if (firstFrame)
            {
                firstFrame = false;
                Objects.ForEach(x => x.Script?.Initialize());
                Cameras.ForEach(x => x.Script?.Initialize());
            }

            Objects.ForEach(x => x.Script?.Update());
            Cameras.ForEach(x => x.Script?.Update());
            Cameras.ForEach(x => x?.UpdateView());
            Objects.ForEach(x => x.Render());
        }

        public SceneObject Add(SceneObject model)
        {
            if (!firstFrame)
                model.Script?.Initialize();
            model.AttachMouseAndKeyboardFromWindow(Window);
            Objects.Add(model);
            return model;
        }

        public SceneObject Remove(SceneObject model)
        {
            model.Script?.Uninitialize();
            Objects.Remove(model);
            return model;
        }

        public Camera Add(Camera camera)
        {
            if (!firstFrame)
                camera.Script?.Initialize();
            camera.AttachMouseAndKeyboardFromWindow(Window);
            Cameras.Add(camera);
            return camera;
        }

        public Camera Remove(Camera camera)
        {
            camera.Script?.Uninitialize();
            Cameras.Remove(camera);
            return camera;
        }
    }
}