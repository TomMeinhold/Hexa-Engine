using HexaEngine.Core.Ressources;
using SharpDX.Direct2D1;
using System;
using System.IO;

namespace HexaEngine.Core.Render.Components
{
    public class Texture : IDisposable
    {
        private Texture()
        {
        }

        public string Name { get; private set; }

        public FileInfo File { get; private set; }

        public Bitmap1 Bitmap { get; private set; }

        public bool HasAlpha { get; private set; }

        public Engine Engine { get; private set; }

        public static void Load(Engine engine, string file)
        {
            var fileInfo = new FileInfo(Engine.TexturePath.FullName + file);
            var bitmap = new System.Drawing.Bitmap(fileInfo.FullName);

            Texture texture = new Texture()
            {
                Bitmap = engine.RessouceManager.Convert(bitmap, bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha),
                Engine = engine,
                File = fileInfo,
                HasAlpha = bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha,
                Name = fileInfo.Name.Replace(fileInfo.Extension, "")
            };
            RessouceManager.Textures.Add(texture);
        }

        internal static Texture Load(Engine engine, string file, bool unmanaged)
        {
            var fileInfo = new FileInfo(Engine.TexturePath.FullName + file);
            var bitmap = new System.Drawing.Bitmap(fileInfo.FullName);

            Texture texture = new Texture()
            {
                Bitmap = engine.RessouceManager.Convert(bitmap, bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha),
                Engine = engine,
                File = fileInfo,
                HasAlpha = bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.PAlpha,
                Name = fileInfo.Name.Replace(fileInfo.Extension, "")
            };

            if (!unmanaged)
            {
                RessouceManager.Textures.Add(texture);
            }

            return texture;
        }

        public void Dispose()
        {
            ((IDisposable)Bitmap).Dispose();
        }

        public static implicit operator Bitmap1(Texture x) => x.Bitmap;
    }
}