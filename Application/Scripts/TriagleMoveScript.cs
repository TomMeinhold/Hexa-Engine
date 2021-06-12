using HexaFramework.Scenes;
using HexaFramework.Scripts;
using System.Numerics;

namespace App.Scripts
{
    public class TriagleMoveScript : Script
    {
        private const float DegToRadFactor = 0.0174532925f;
        private float angle = 0;

        public override void Update()
        {
            var model = GetComponent<SceneObject>();
            angle = NormalizeEulerAngle(angle + 0.1f);
            model.Transform = Matrix4x4.CreateRotationY(angle * DegToRadFactor, Vector3.Zero);
        }

        public static float NormalizeEulerAngle(float angle)
        {
            var normalized = angle % 360;
            if (normalized < 0)
                normalized += 360;
            return normalized;
        }
    }
}