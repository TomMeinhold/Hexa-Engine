using HexaEngine.Core.Extentions;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;

namespace HexaEngine.Core.Objects.Components
{
    public class RayCastingModule
    {
        private Bitmap1 buffer;

        private SolidColorBrush raybrush;

        public RayCastingModule(IRayCasting rayCasting)
        {
            RayCasting = rayCasting;
        }

        public IRayCasting RayCasting { get; }

        public GaussianBlur Blur { get; private set; }

        public void Render(DeviceContext context)
        {
            if (buffer is null)
            {
                buffer = RayCasting.Engine.RenderSystem.RessouceManager.GetNewBitmap();
            }

            if (Blur is null)
            {
                Blur = new GaussianBlur(context) { Optimization = GaussianBlurOptimization.Quality, StandardDeviation = 10 };
            }

            if (raybrush is null)
            {
                raybrush = new SolidColorBrush(context, RayCasting.GlowColor);
            }
            else
            {
                raybrush.Color = RayCasting.GlowColor;
            }

            context.Target = buffer;
            context.Transform = (Matrix3x2)Matrix.Identity;
            context.Clear(Color.Transparent);
            if (!(RayCasting.Rays is null) && RayCasting.RaysEnabled)
            {
                lock (RayCasting.Rays)
                {
                    foreach (Ray ray in RayCasting.Rays)
                    {
                        context.DrawLine(ray.Position.Downgrade(), ray.Direction.Downgrade(), raybrush);
                    }
                }
            }

            Blur.SetInput(0, buffer, true);
            context.Target = RayCasting.Engine.RenderSystem.RessouceManager.RayBitmap;
            context.DrawImage(Blur, InterpolationMode.HighQualityCubic, CompositeMode.Plus);
        }
    }
}