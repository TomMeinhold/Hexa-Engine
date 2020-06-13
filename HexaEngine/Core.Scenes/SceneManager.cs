using HexaEngine.Core.Objects.BaseTypes;
using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Render.Interfaces;
using SharpDX.Direct2D1;
using System;

namespace HexaEngine.Core.Scenes
{
    public class SceneManager
    {
        public Scene SelectedScene { get; set; }

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
    }
}