using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace HexaEngine.Core.Scenes
{
    public class SceneManager
    {
        private Scene selectedScene;

        public SceneManager()
        {
        }

        public Dictionary<Type, Scene> Instances { get; } = new Dictionary<Type, Scene>();

        public event EventHandler<Scene> SceneChanged;

        public Scene SelectedScene
        {
            get => selectedScene;
            set
            {
                if (selectedScene != null)
                {
                    foreach (object obj in selectedScene.Objects)
                    {
                        if (obj is BaseObject baseObject)
                        {
                            baseObject.Enabled = false;
                        }
                    }
                }

                foreach (object obj in value.Objects)
                {
                    if (obj is BaseObject baseObject)
                    {
                        baseObject.Enabled = true;
                    }
                }

                var before = selectedScene;
                if (SelectedScene != null)
                {
                    lock (SelectedScene)
                    {
                        value.LoadRessources();
                        selectedScene = value;
                        before.UnloadRessources();
                    }
                }
                else
                {
                    value.LoadRessources();
                    selectedScene = value;
                }

                SceneChanged?.Invoke(this, value);
            }
        }

        public void SetSceneByType(Type type, bool createNew = false)
        {
            if (!createNew && Instances.ContainsKey(type))
            {
                SelectedScene = Instances[type];
            }
            else
            {
                Instances[type] = SelectedScene = (Scene)Activator.CreateInstance(type);
            }
        }

        internal void RenderScene(DeviceContext d2DDeviceContext)
        {
            if (SelectedScene == null)
            {
                return;
            }

            lock (SelectedScene)
            {
                lock (SelectedScene.Objects)
                {
                    foreach (IBaseObject baseObject in SelectedScene.Objects)
                    {
                        if (baseObject is IDrawable drawable)
                        {
                            drawable.Render(d2DDeviceContext);
                        }
                    }
                }
            }
        }

        internal void RenderScene(SharpDX.Direct3D11.DeviceContext deviceContext)
        {
            if (SelectedScene == null)
            {
                return;
            }

            lock (SelectedScene)
            {
                lock (SelectedScene.Objects)
                {
                    foreach (IBaseObject baseObject in SelectedScene.Objects)
                    {
                        if (baseObject is IDrawable3D drawable)
                        {
                            drawable.Render3D(deviceContext);
                        }
                    }
                }
            }
        }
    }
}