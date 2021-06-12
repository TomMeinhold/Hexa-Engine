using System.Numerics;

namespace HexaFramework.Scenes
{
    public class LightDirectional
    {
        public Vector4 AmbientColor { get; set; }
        public Vector4 DiffuseColour { get; set; }
        public Vector3 Direction { get; set; }

        public float SpecularPower { get; set; }

        public Vector4 SpecularColor { get; set; }

        // Methods
        public void SetDiffuseColour(float red, float green, float blue, float alpha)
        {
            DiffuseColour = new Vector4(red, green, blue, alpha);
        }

        public void SetDirection(float x, float y, float z)
        {
            Direction = new Vector3(x, y, z);
        }
    }
}