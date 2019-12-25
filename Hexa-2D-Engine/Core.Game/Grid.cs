using HexaEngine.Core.Common;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Game
{
    public class Grid
    {
        readonly Engine Engine;

        readonly float Width;

        readonly float Height;

        public RawVector2Buffer Buffer = new RawVector2Buffer();

        public Thread Thread;

        public Grid(Engine engine)
        {
            Engine = engine;
            Width = Engine.RenderTarget.Size.Width;
            Height = Engine.RenderTarget.Size.Height;
            Thread = new Thread(Worker);
            Thread.Start();
        }

        public void Dispose()
        {
            Thread.Abort();
        }

        private void Worker()
        {
            while (true)
            {
                RawVector2[] list = new RawVector2[]
            {
                Engine.PhysicsEngine.InsertRelativePosition(new RawVector2() { X = 0, Y = Height / 2 }),
                Engine.PhysicsEngine.InsertRelativePosition(new RawVector2() { X = Width, Y = Height / 2 }),
                Engine.PhysicsEngine.InsertRelativePosition(new RawVector2() { X = Width / 2, Y = 0 }),
                Engine.PhysicsEngine.InsertRelativePosition(new RawVector2() { X = Width / 2, Y = Height })
            };
                Buffer.AddBuffer(list);
            }
        }


        public void Draw()
        {
            RenderTarget render = Engine.RenderTarget;
            Brushpalette brushpalette = Engine.Brushpalette;
            RawVector2[] arrayList = Buffer.GetBuffer();
            render.DrawLine(arrayList[0], arrayList[1], brushpalette.BrushRed);
            render.DrawLine(arrayList[2], arrayList[3], brushpalette.BrushRed);
        }
    }
}
