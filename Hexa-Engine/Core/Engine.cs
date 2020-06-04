using HexaEngine.Core.Common;
using HexaEngine.Core.Input;
using HexaEngine.Core.Objects;
using HexaEngine.Core.Physics;
using HexaEngine.Core.Render;
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
            Transform = new EngineTransform(this);

            InputSystem = new InputSystem(this, renderForm);
            Camera = new Camera(this);
            RenderSystem = new RenderSystem(this, renderForm);

            PhysicsEngine = new PhysicsEngine(this);
        }

        public void Dispose()
        {
            PhysicsEngine.Dispose();
            Brushpalette.Dispose();
        }
    }
}