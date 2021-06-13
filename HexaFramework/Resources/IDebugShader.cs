using HexaFramework.Scenes;
using Vortice.Direct3D;

namespace HexaFramework.Resources
{
    public interface IDebugShader
    {
        public void Render(SceneObject sceneObject);

        public void Render(VertexPositionColor[] vertices, PrimitiveTopology topology);
    }
}