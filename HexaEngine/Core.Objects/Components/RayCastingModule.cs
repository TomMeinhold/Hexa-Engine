using HexaEngine.Core.Extensions;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
using SharpDX.Mathematics.Interop;
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

        private List<Ray> Rays { get; set; } = new List<Ray>();

        public void Render(DeviceContext context)
        {
            if (buffer is null)
            {
                buffer = Engine.Current.RessouceManager.GetNewBitmap();
            }

            if (raybrush is null)
            {
                raybrush = new SolidColorBrush(context, RayCasting.RayCastDiscription.RayColor);
            }
            else
            {
                raybrush.Color = RayCasting.RayCastDiscription.RayColor;
            }

            context.Target = Engine.Current.RenderSystem.DriectXManager.RayBitmap;
            context.Transform = (Matrix3x2)Matrix.Identity;
            if (!(Rays is null) && RayCasting.RayCastDiscription.RaysEnabled)
            {
                lock (Rays)
                {
                    if (Rays.Count > 0)
                    {
                        PathGeometry geometry = new PathGeometry(context.Factory);
                        var sink = geometry.Open();
                        sink.BeginFigure(Rays[0].Direction.Downgrade(), FigureBegin.Filled);
                        sink.AddLines(Rays.ConvertAll(x => (RawVector2)x.Direction.Downgrade()).ToArray());
                        sink.EndFigure(FigureEnd.Closed);
                        sink.Close();
                        context.FillGeometry(geometry, raybrush);
                        geometry.Dispose();
                    }
                }
            }
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