using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Render.Components
{
    public class PostProcessingManager
    {
        public PostProcessingManager(RenderSystem renderSystem)
        {
            RenderSystem = renderSystem ?? throw new ArgumentNullException(nameof(renderSystem));
        }

        private List<Effect> Effects { get; } = new List<Effect>();

        private RenderSystem RenderSystem { get; }

        public void AddEffect(Effect effect)
        {
            Effects.Add(effect);
        }

        public void PostProcess(Bitmap1 input, Bitmap1 output)
        {
            Effect lastEffect = null;

            foreach (Effect effect in this.Effects)
            {
                effect.SetInput(0, this.RenderSystem.DriectXManager.ObjectsBitmap, true);
                lastEffect?.SetInputEffect(0, effect);
                lastEffect = effect;
            }

            this.RenderSystem.DriectXManager.D2DDeviceContext.BeginDraw();
            this.RenderSystem.DriectXManager.D2DDeviceContext.Target = output;
            this.RenderSystem.DriectXManager.D2DDeviceContext.Clear(Color.Transparent);
            if (lastEffect is null)
            {
                this.RenderSystem.DriectXManager.D2DDeviceContext.DrawBitmap(input, 1, BitmapInterpolationMode.Linear);
            }
            else
            {
                this.RenderSystem.DriectXManager.D2DDeviceContext.DrawImage(lastEffect);
            }

            this.RenderSystem.DriectXManager.D2DDeviceContext.EndDraw();
        }

        public void PostProcess(Bitmap1 input, Bitmap1 output, Matrix3x2 matrix)
        {
            Effect lastEffect = null;

            foreach (Effect effect in this.Effects)
            {
                effect.SetInput(0, this.RenderSystem.DriectXManager.ObjectsBitmap, true);
                lastEffect?.SetInputEffect(0, effect);
                lastEffect = effect;
            }

            this.RenderSystem.DriectXManager.D2DDeviceContext.BeginDraw();
            this.RenderSystem.DriectXManager.D2DDeviceContext.Target = output;
            this.RenderSystem.DriectXManager.D2DDeviceContext.Clear(Color.Transparent);
            if (lastEffect is null)
            {
                this.RenderSystem.DriectXManager.D2DDeviceContext.Transform = matrix;
                this.RenderSystem.DriectXManager.D2DDeviceContext.DrawBitmap(input, 1, BitmapInterpolationMode.Linear);
                this.RenderSystem.DriectXManager.D2DDeviceContext.Transform = (Matrix3x2)Matrix.Identity;
            }
            else
            {
                this.RenderSystem.DriectXManager.D2DDeviceContext.Transform = matrix;
                this.RenderSystem.DriectXManager.D2DDeviceContext.DrawImage(lastEffect);
                this.RenderSystem.DriectXManager.D2DDeviceContext.Transform = (Matrix3x2)Matrix.Identity;
            }

            this.RenderSystem.DriectXManager.D2DDeviceContext.EndDraw();
        }
    }
}