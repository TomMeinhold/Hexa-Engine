using HexaEngine.Core.Common;
using HexaEngine.Core.Input;
using HexaEngine.Core.Physics;
using HexaEngine.Core.Render;
using HexaEngine.Core.Scenes;
using HexaEngine.Core.UI;
using SharpDX;
using SharpDX.Windows;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        public Engine(RenderForm renderForm)
        {
            Settings = new EngineSettings
            {
                Width = renderForm.Width,
                Height = renderForm.Height
            };
            BitmapConverter = new BitmapConverter(this);

            InputSystem = new InputSystem(renderForm);
            Camera = new Camera(this);
            SceneManager = new SceneManager(this);
            UIManager = new UserInterfaceManager(this);
            RenderSystem = new RenderSystem(this, renderForm);
            PhysicsEngine = new PhysicsEngine(this);
        }

        public Engine(RenderForm renderForm, Size2F size2F)
        {
            Settings = new EngineSettings
            {
                Width = size2F.Width,
                Height = size2F.Height
            };
            BitmapConverter = new BitmapConverter(this);

            InputSystem = new InputSystem(renderForm);
            Camera = new Camera(this);
            RenderSystem = new RenderSystem(this, renderForm);
            PhysicsEngine = new PhysicsEngine(this);
            UIManager = new UserInterfaceManager(this);
        }

        public void Dispose()
        {
            PhysicsEngine.Dispose();
            Brushpalette.Dispose();
        }
    }
}