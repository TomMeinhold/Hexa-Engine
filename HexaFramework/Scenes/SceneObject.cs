using HexaFramework.Resources;
using HexaFramework.Scripts;
using PhysX;
using System.Numerics;
using Vortice.Direct3D11;

namespace HexaFramework.Scenes
{
    public class SceneObject : ScriptableElement
    {
        public Model Model { get; set; }

        public Shader Shader { get; set; }

        public Texture[] Textures { get; private set; }

        public ID3D11ShaderResourceView[] ResourceViews { get; private set; }

        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;

        public RigidActor RigidActor { get; set; }

        public void Render()
        {
            Shader.Render(this);
        }

        public void InitializeModelObj(ResourceManager manager, string path)
        {
            Model = manager.LoadModelObj(path);
        }

        public void InitializeTextures(ResourceManager manager, params string[] paths)
        {
            Textures = new Texture[paths.Length];
            ResourceViews = new ID3D11ShaderResourceView[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                ResourceViews[i] = Textures[i] = manager.LoadTexture(paths[i]);
            }
        }
    }
}