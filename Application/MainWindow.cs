using App.Scripts;
using App.Shaders;
using HexaFramework.NvPhysX;
using HexaFramework.Resources;
using HexaFramework.Scenes;
using HexaFramework.Windows;
using PhysX;
using System.Numerics;

namespace App
{
    public class MainWindow : RenderWindow
    {
        private Camera Camera;
        private readonly SceneObject sceneObject = new();
        private readonly SceneObject sceneObject1 = new();
        private DefaultShader Shader;

        public MainWindow() : base("Test", 1280, 720)
        {
            LimitFPS = true;
        }

        protected override void InitializeComponent()
        {
            Camera = new Camera(DeviceManager)
            {
                Fov = 90,
                FarPlane = 1000f,
                PositionZ = -10,
                PositionY = 1
            };
            Camera.AttachScript<CameraScript>();
            var sound = Sound.Load(ResourceManager, "Resources/sound.wav");
            Camera.AddResource("Sound1", sound);
            Add(Camera);
            DebugShader = new DebugShader(DeviceManager, Camera);
            Shader = new DefaultShader(DeviceManager, Camera);
            Shader.Light = new LightDirectional()
            {
                DiffuseColour = new Vector4(1, 1, 1, 1),
                Direction = new Vector3(0, -1, 0),
                AmbientColor = new Vector4(0.15f, 0.15f, 0.15f, 1.0f),
                SpecularColor = new Vector4(1, 1, 1, 1),
                SpecularPower = 16
            };

            sceneObject.Shader = Shader;
            sceneObject.InitializeModelObj(ResourceManager, "Models/Plane.obj");
            sceneObject.InitializeTextures(ResourceManager, "Resources/d.png", "Resources/n.png", "Resources/s.png");
            var ground = Scene.CreateGroundPlane(sceneObject, Scene.Physics.CreateMaterial(0.1f, 0.5f, 0.5f));
            ground.Shapes[0].LocalPose = Matrix4x4.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)System.Math.PI / 2);
            sceneObject1.Shader = Shader;
            sceneObject1.InitializeModelObj(ResourceManager, "Models/Cube.obj");
            sceneObject1.InitializeTextures(ResourceManager, "Resources/d.png", "Resources/n.png", "Resources/s.png");
            var cube = Scene.CreateConvexMesh(sceneObject1, 100, Scene.Physics.CreateMaterial(0.1f, 0.5f, 0.5f));
            cube.GlobalPose = Matrix4x4.CreateTranslation(new(0, 10, 0));
            cube.Name = "Cube";
        }
    }
}