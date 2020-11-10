using HexaEngine.Core.Ressources;
using SharpDX.Direct2D1;
using System;
using System.IO;

namespace HexaEngine.Core.Render.Components
{
    public class Texture
    {
        private Texture()
        {
        }

        public string Name { get; private set; }

        public FileInfo File { get; private set; }

        public Bitmap1 Bitmap { get; private set; }

        public bool HasAlpha { get; private set; }

        public static void Load(string file)
        {
            var fileInfo = new FileInfo(Engine.TexturePath.FullName + file);
            var bitmap = new System.Drawing.Bitmap(fileInfo.FullName);

            Texture texture = new Texture()
            {
                Bitmap = Engine.Current.RessouceManager.Convert(bitmap, bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha),
                File = fileInfo,
                HasAlpha = bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha,
                Name = fileInfo.Name.Replace(fileInfo.Extension, "")
            };
            RessourceManager.Textures.Add(texture);
        }

        public static Texture LoadUnmanaged(string file)
        {
            var fileInfo = new FileInfo(Engine.TexturePath.FullName + file);
            var bitmap = new System.Drawing.Bitmap(fileInfo.FullName);

            Texture texture = new Texture()
            {
                Bitmap = Engine.Current.RessouceManager.Convert(bitmap, bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha),
                File = fileInfo,
                HasAlpha = bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha,
                Name = fileInfo.Name.Replace(fileInfo.Extension, "")
            };

            RessourceManager.Textures.Add(texture);

            return texture;
        }

        public static void Load(System.Drawing.Bitmap bitmap, string name)
        {
            Texture texture = new Texture()
            {
                Bitmap = Engine.Current.RessouceManager.Convert(bitmap, bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Alpha),
                File = null,
                HasAlpha = bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha,
                Name = name
            };
            RessourceManager.Textures.Add(texture);
        }

        public static Texture LoadUnmanaged(System.Drawing.Bitmap bitmap, string name)
        {
            Texture texture = new Texture()
            {
                Bitmap = Engine.Current.RessouceManager.Convert(bitmap, bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Alpha),
                File = null,
                HasAlpha = bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha,
                Name = name
            };

            RessourceManager.Textures.Add(texture);

            return texture;
        }

        public static Texture Get(string file)
        {
            return RessourceManager.GetTexture(file);
        }

        public void Unload()
        {
            RessourceManager.Unload(Name, RessourceType.Sprite);
        }

        public void Dispose()
        {
            ((IDisposable)Bitmap).Dispose();
        }

        public static implicit operator Bitmap1(Texture x) => x.Bitmap;
    }
}