using App.Scripts;
using App.Shaders;
using HexaFramework.Resources;
using HexaFramework.Scenes;
using HexaFramework.Windows;
using System.Numerics;

namespace App
{
    public class MainWindow : RenderWindow
    {
        private Camera Camera;
        private readonly SceneObject sceneObject = new();
        private readonly SceneObject sceneObject1 = new();
        private ShaderTessellation Shader;

        public MainWindow() : base("Test", 1280, 720)
        {
            LimitFPS = true;
        }

        protected override void InitializeComponent()
        {
            Camera = new Camera(DeviceManager)
            {
                Fov = 90,
                FarPlane = 100f,
                PositionZ = -10
            };
            Camera.AttachScript<CameraScript>();
            var sound = Sound.Load(ResourceManager, "Resources/sound.wav");
            Camera.AddResource("Sound1", sound);
            Scene.Add(Camera);
            Shader = new ShaderTessellation(DeviceManager, Scene, Camera,
                "Shaders/vertexShader.hlsl",
                "Shaders/hullShader.hlsl",
                "Shaders/domainShader.hlsl",
                "Shaders/pixelShader.hlsl",
                "main",
                "ColorHullShader",
                "ColorDomainShader",
                "main",
                "vs_5_0",
                "hs_5_0",
                "ds_5_0",
                "ps_5_0"
                )
            {
                Light = new LightDirectional()
                {
                    DiffuseColour = new Vector4(1, 1, 1, 1),
                    Direction = new Vector3(0, -1, 0),
                    AmbientColor = new Vector4(0.15f, 0.15f, 0.15f, 1.0f),
                    SpecularColor = new Vector4(1, 1, 1, 1),
                    SpecularPower = 8
                }
            };
            //Model.AttachScript<TriagleMoveScript>();

            Shader.TessellationAmount = 1;
            sceneObject.Shader = Shader;
            sceneObject.InitializeModelObj(ResourceManager, "Models/Cube.obj");
            sceneObject.InitializeTextures(ResourceManager, "Resources/d.png", "Resources/n.png", "Resources/s.png", "Resources/h.png");
            sceneObject.Transform = Matrix4x4.CreateTranslation(0, 1, 0);
            Scene.Add(sceneObject);
            sceneObject1.Shader = Shader;
            sceneObject1.InitializeModelObj(ResourceManager, "Models/Plane.obj");
            sceneObject1.InitializeTextures(ResourceManager, "Resources/d.png", "Resources/n.png", "Resources/s.png");
            Scene.Add(sceneObject1);

            //Scene.Add(sceneObject.Clone(Matrix4x4.CreateTranslation(0, 0, 2)));
            //Scene.Add(sceneObject.Clone(Matrix4x4.CreateTranslation(0, 0, -2)));
            //Scene.Add(sceneObject.Clone(Matrix4x4.CreateTranslation(2, 0, 0)));
            //Scene.Add(sceneObject.Clone(Matrix4x4.CreateTranslation(-2, 0, 0)));
            //Scene.Add(sceneObject.Clone(Matrix4x4.CreateTranslation(-2, 0, 2)));
            //Scene.Add(sceneObject.Clone(Matrix4x4.CreateTranslation(-2, 0, -2)));
            //Scene.Add(sceneObject.Clone(Matrix4x4.CreateTranslation(2, 0, 2)));
            //Scene.Add(sceneObject.Clone(Matrix4x4.CreateTranslation(2, 0, -2)));
        }
    }
}