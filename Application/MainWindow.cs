using App.Scripts;
using App.Shaders;
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
                PositionZ = -10,
                PositionY = 1
            };
            Camera.AttachScript<CameraScript>();
            var sound = Sound.Load(ResourceManager, "Resources/sound.wav");
            Camera.AddResource("Sound1", sound);
            Add(Camera);
            Shader = new ShaderTessellation(DeviceManager, Camera,
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
            sceneObject.InitializeModelObj(ResourceManager, "Models/Plane.obj");
            sceneObject.InitializeTextures(ResourceManager, "Resources/d.png", "Resources/n.png", "Resources/s.png");
            CreateGroundPlane(sceneObject);
            sceneObject1.Shader = Shader;
            sceneObject1.InitializeModelObj(ResourceManager, "Models/Cube.obj");
            sceneObject1.InitializeTextures(ResourceManager, "Resources/d.png", "Resources/n.png", "Resources/s.png");
            CreateBox(sceneObject1);
        }

        private void CreateGroundPlane(SceneObject sceneObject)
        {
            var groundPlaneMaterial = Scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);
            var groundPlane = Scene.Physics.CreateRigidStatic();
            groundPlane.Name = "Ground Plane";
            var planeGeom = new PlaneGeometry();
            var shape = RigidActorExt.CreateExclusiveShape(groundPlane, planeGeom, groundPlaneMaterial, null);
            shape.LocalPose = Matrix4x4.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)System.Math.PI / 2);
            groundPlane.UserData = sceneObject;
            Scene.AddActor(groundPlane);
        }

        private void CreateBox(SceneObject sceneObject)
        {
            var material = Scene.Physics.CreateMaterial(0.1f, 0.5f, 0.5f);
            var rigidActor = Scene.Physics.CreateRigidDynamic();
            var boxGeometry = new BoxGeometry(1, 1, 1);
            var boxShape = RigidActorExt.CreateExclusiveShape(rigidActor, boxGeometry, material, null);
            rigidActor.GlobalPose = Matrix4x4.CreateTranslation(new Vector3(0, 3, 0));
            rigidActor.SetMassAndUpdateInertia(100);
            rigidActor.UserData = sceneObject;
            Scene.AddActor(rigidActor);
            rigidActor.AddForceAtLocalPosition(Vector3.UnitY * 100f, Vector3.Zero, ForceMode.Impulse, true);
        }
    }
}