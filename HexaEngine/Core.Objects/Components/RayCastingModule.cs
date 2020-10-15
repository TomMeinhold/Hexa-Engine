using HexaEngine.Core.Extensions;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
using System.Collections.Generic;

namespace HexaEngine.Core.Objects.Components
{
    public class RayCastingModule
    {
        private Bitmap1 buffer;

        private SolidColorBrush raybrush;

        private int pointer;

        private int lastPointer;

        public RayCastingModule(IRayCasting rayCasting)
        {
            RayCasting = rayCasting;
        }

        public IRayCasting RayCasting { get; }

        public GaussianBlur Blur { get; private set; }

        private List<Ray> Rays { get; set; } = new List<Ray>();

        public void Render(DeviceContext context)
        {
            if (buffer is null)
            {
                buffer = RayCasting.Engine.RessouceManager.GetNewBitmap();
            }

            if (Blur is null)
            {
                Blur = new GaussianBlur(context) { Optimization = GaussianBlurOptimization.Quality, StandardDeviation = 10 };
            }

            if (raybrush is null)
            {
                raybrush = new SolidColorBrush(context, RayCasting.RayCastDiscription.RayColor);
            }
            else
            {
                raybrush.Color = RayCasting.RayCastDiscription.RayColor;
            }

            context.Target = buffer;
            context.Transform = (Matrix3x2)Matrix.Identity;
            context.Clear(Color.Transparent);
            if (!(Rays is null) && RayCasting.RayCastDiscription.RaysEnabled)
            {
                lock (Rays)
                {
                    int i = 0;
                    foreach (Ray ray in Rays)
                    {
                        context.DrawLine(ray.Position.Downgrade(), ray.Direction.Downgrade(), raybrush);
                        i++;
                        if (i == lastPointer)
                        {
                            break;
                        }
                    }
                }
            }

            Blur.SetInput(0, buffer, true);
            context.Target = RayCasting.Engine.RenderSystem.DriectXManager.RayBitmap;
            context.DrawImage(Blur, InterpolationMode.HighQualityCubic, CompositeMode.Plus);
        }

        public void AddRay(Vector3 pos, Vector3 direct)
        {
            if (Rays.Count < pointer + 1)
            {
                Ray ray = new Ray(pos, direct);
                lock (Rays)
                {
                    Rays.Add(ray);
                }
            }
            else
            {
                Ray ray = Rays[pointer];
                ray.Position = pos;
                ray.Direction = direct;
                lock (Rays)
                {
                    Rays[pointer] = ray;
                }
            }

            pointer++;
        }

        public void ResetPointer()
        {
            lock (Rays)
            {
                lastPointer = pointer;
                pointer = 0;
            }
        }
    }
}