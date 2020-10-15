﻿using HexaEngine.Core.Render.Components;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace HexaEngine.Core.Ressources
{
    public class Sprite : IDisposable
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private bool disposedValue;

        private Sprite()
        {
            stopwatch.Start();
        }

        ~Sprite()
        {
            Dispose(disposing: false);
        }

        public static Sprite Load(Engine engine, TimeSpan time, params string[] names)
        {
            List<Texture> textures = new List<Texture>();
            foreach (string name in names)
            {
                textures.Add(Texture.Load(engine, name, true));
            }

            Sprite sprite = new Sprite
            {
                Engine = engine,
                Name = Path.GetFileNameWithoutExtension(names[0]),
                Textures = textures,
                TimeSpan = time
            };
            RessouceManager.Sprites.Add(sprite);
            return sprite;
        }

        public string Name { get; internal set; }

        public List<Texture> Textures { get; private set; } = new List<Texture>();

        public TimeSpan TimeSpan { get; set; }

        public Engine Engine { get; private set; }

        public Size2F Size { get => Textures[SpriteIndex].Bitmap.Size; }

        public int SpriteIndex { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Textures.ForEach(x => x.Dispose());
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Texture GetCurrentTexture()
        {
            if (stopwatch.ElapsedTicks > TimeSpan.Ticks && TimeSpan.Ticks != 0)
            {
                SpriteIndex++;
                stopwatch.Restart();
                if (SpriteIndex == Textures.Count)
                {
                    SpriteIndex = 0;
                }
            }

            return Textures[SpriteIndex];
        }

        public static implicit operator Bitmap1(Sprite x) => x.GetCurrentTexture();
    }
}