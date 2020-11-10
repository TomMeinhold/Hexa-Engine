using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace HexaEngine.Core.Objects.Components
{
    public class ObjectPostProcessManager : IDisposable
    {
        private bool disposedValue;

        ~ObjectPostProcessManager()
        {
            Dispose(disposing: false);
        }

        private List<Effect> Effects { get; } = new List<Effect>();

        public void AddEffect(Effect effect)
        {
            Effects.Add(effect);
        }

        public void PostProcess(DeviceContext deviceContext, Bitmap1 input, Bitmap1 output)
        {
            Effect lastEffect = null;
            foreach (Effect effect in Effects)
            {
                if (effect is null)
                {
                    effect.SetInput(0, input, true);
                }
                else
                {
                    effect.SetInputEffect(0, lastEffect, true);
                }

                lastEffect = effect;
            }

            deviceContext.BeginDraw();
            deviceContext.Target = output;
            deviceContext.Clear(Color.Transparent);
            if (lastEffect is null)
            {
                deviceContext.DrawBitmap(input, 1, BitmapInterpolationMode.Linear);
            }
            else
            {
                deviceContext.DrawImage(lastEffect);
            }

            deviceContext.EndDraw();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (Effect effect in Effects)
                    {
                        effect.Dispose();
                    }
                }

                disposedValue = true;
            }
        }
    }
}