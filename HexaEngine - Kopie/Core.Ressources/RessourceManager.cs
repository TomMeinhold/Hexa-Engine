﻿// <copyright file="RessouceManager.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Ressources
{
    using HexaEngine.Core.Render.Components;
    using SharpDX.Direct2D1;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RessourceManager : IDisposable
    {
        public RessourceManager(Engine engine)
        {
            Engine = engine;
        }

        ~RessourceManager()
        {
            Dispose(false);
        }

        public bool IsDisposed { get; private set; } = false;

        public static List<Sound> Sounds { get; } = new List<Sound>();

        public static List<Sprite> Sprites { get; } = new List<Sprite>();

        public static List<Texture> Textures { get; } = new List<Texture>();

        public static List<Bitmap1> Bitmaps { get; } = new List<Bitmap1>();

        public static List<Archive> Archives { get; } = new List<Archive>();

        public Engine Engine { get; }

        public Bitmap1 GetNewBitmap()
        {
            var tmp = new Bitmap1(Engine.RenderSystem.DriectXManager.D2DDeviceContext, new SharpDX.Size2(Engine.RenderSystem.DriectXManager.RenderForm.ClientSize.Width, Engine.RenderSystem.DriectXManager.RenderForm.ClientSize.Height), Engine.RenderSystem.DriectXManager.DefaultBitmapProperties);
            Bitmaps.Add(tmp);
            return tmp;
        }

        public static Texture GetTexture(string name)
        {
            return Textures.FirstOrDefault(x => x.Name == name) ?? Archives.FirstOrDefault(x => x.Contains(RessourceType.Texture, name)).GetTexture(name);
        }

        public static Sprite GetSprite(string name)
        {
            return Sprites.FirstOrDefault(x => x.Name == name) ?? Archives.FirstOrDefault(x => x.Contains(RessourceType.Sprite, name)).GetSprite(name);
        }

        public static Sound GetSound(string name)
        {
            return Sounds.FirstOrDefault(x => x.Name == name) ?? Archives.FirstOrDefault(x => x.Contains(RessourceType.Sound, name)).GetSound(name);
        }

        public Bitmap1 Convert(System.Drawing.Bitmap bitmap, bool hasAlpha = false)
        {
            return Common.ConvertBitmap.Convert(Engine.RenderSystem.DriectXManager.D2DDeviceContext, bitmap, hasAlpha);
        }

        public static void Unload(string name, RessourceType type)
        {
            switch (type)
            {
                case RessourceType.Texture:
                    GetTexture(name)?.Dispose();
                    Archives.FirstOrDefault(x => x.Contains(type, name))?.GetTexture(name)?.Dispose();
                    break;

                case RessourceType.Sprite:
                    GetSprite(name)?.Dispose();
                    Archives.FirstOrDefault(x => x.Contains(type, name))?.GetSprite(name)?.Dispose();
                    break;

                case RessourceType.Sound:
                    GetSound(name)?.Dispose();
                    Archives.FirstOrDefault(x => x.Contains(type, name))?.GetSound(name)?.Dispose();
                    break;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
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

                IsDisposed = true;
            }
        }
    }
}