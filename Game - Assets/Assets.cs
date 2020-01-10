using HexaEngine.Core.Common;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexaEngine.Core.Objects;
using GameAssets.Objects;

namespace GameAssets
{
    public static class Assets
    {
        public static void LoadAssets(HexaEngine.Core.Engine engine)
        {
            ObjectSystem objectSystem = engine.ObjectSystem;
            engine.ObjectSystem.Player = new Player(engine, ConvertBitmap.Convert(engine.RenderTarget, Resource.player), new RawVector3() { X = 0, Y = 0, Z = 0 });
            _ = new Wall(engine, ConvertBitmap.Convert(engine.RenderTarget, Resource.large_wall), new RawVector3() { X = 0, Y = -100, Z = 0 });
            _ = new Wall(engine, ConvertBitmap.Convert(engine.RenderTarget, Resource.large_wall), new RawVector3() { X = 128, Y = -36, Z = 0 });
            _ = new Wall(engine, ConvertBitmap.Convert(engine.RenderTarget, Resource.wall), new RawVector3() { X = -64, Y = 36, Z = 0 });
            _ = new Wall(engine, ConvertBitmap.Convert(engine.RenderTarget, Resource.wall), new RawVector3() { X = -64, Y = -36, Z = 0 });
            _ = new Background(engine, ConvertBitmap.Convert(engine.RenderTarget, Resource.background), new RawVector3() { X = -960, Y = 540, Z = -1 });
            objectSystem.Finishing();
        }
    }
}
