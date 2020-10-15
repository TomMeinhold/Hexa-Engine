// <copyright file="RessouceManager.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Ressources
{
    using HexaEngine.Core.Render.Components;
    using SharpDX.Direct2D1;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RessouceManager : IDisposable
    {
        public RessouceManager(Engine engine)
        {
            Engine = engine;
        }

        ~RessouceManager()
        {
            this.Dispose(false);
        }

        public bool IsDisposed { get; private set; } = false;

        public static List<Sound> Sounds { get; } = new List<Sound>();

        public static List<Sprite> Sprites { get; } = new List<Sprite>();

        public static List<Texture> Textures { get; } = new List<Texture>();

        public static List<Bitmap1> Bitmaps { get; } = new List<Bitmap1>();

        public Engine Engine { get; }

        public Bitmap1 GetNewBitmap()
        {
            var tmp = new Bitmap1(Engine.RenderSystem.DriectXManager.D2DDeviceContext, new SharpDX.Size2(Engine.RenderSystem.DriectXManager.RenderForm.ClientSize.Width, Engine.RenderSystem.DriectXManager.RenderForm.ClientSize.Height), Engine.RenderSystem.DriectXManager.DefaultBitmapProperties);
            Bitmaps.Add(tmp);
            return tmp;
        }

        public static Texture GetTexture(string name)
        {
            return Textures.FirstOrDefault(x => x.Name == name);
        }

        public static Sprite GetSprite(string name)
        {
            return Sprites.FirstOrDefault(x => x.Name == name);
        }

        public static Sound GetSound(string name)
        {
            return Sounds.FirstOrDefault(x => x.Name == name);
        }

        public Bitmap1 Convert(System.Drawing.Bitmap bitmap, bool hasAlpha = false)
        {
            return Common.ConvertBitmap.Convert(Engine.RenderSystem.DriectXManager.D2DDeviceContext, bitmap, hasAlpha);
        }

        public void DisposeRessource(string name, RessouceType type)
        {
            switch (type)
            {
                case RessouceType.Texture:
                    GetTexture(name)?.Dispose();
                    break;

                case RessouceType.Sprite:
                    GetSprite(name)?.Dispose();
                    break;

                case RessouceType.Sound:
                    GetSound(name)?.Dispose();
                    break;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    Bitmaps.ForEach(x => x.Dispose());
                    Textures.ForEach(x => x.Dispose());
                    Sprites.ForEach(x => x.Dispose());
                    Sounds.ForEach(x => x.Dispose());
                    foreach (Bitmap1 bitmap in Bitmaps)
                    {
                        bitmap.Dispose();
                    }

                    foreach (Texture bitmap in Textures)
                    {
                        bitmap.Dispose();
                    }
                }

                this.IsDisposed = true;
            }
        }
    }
}