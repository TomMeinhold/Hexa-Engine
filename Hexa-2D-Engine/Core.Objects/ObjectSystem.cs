using HexaEngine.Core.Game;
using HexaEngine.Core.Common;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Objects
{
    public class ObjectSystem
    {
        public ObjectSystem(Engine engineCore)
        {
            Grid = new Grid(engineCore);
            Player = new Player(engineCore, ConvertBitmap.Convert(engineCore.RenderTarget, Resource.Resource.player), new RawVector3() { X = 0, Y = 0, Z = 0 });
            Wall = new Wall(engineCore, ConvertBitmap.Convert(engineCore.RenderTarget, Resource.Resource.wall), new RawVector3() { X = 0, Y = -100, Z = 0 });
            Wall2 = new Wall(engineCore, ConvertBitmap.Convert(engineCore.RenderTarget, Resource.Resource.wall), new RawVector3() { X = -500, Y = -150, Z = 0 });
            ObjectList.Add(Player);
            ObjectList.Add(Wall);
            ObjectList.Add(Wall2);
        }

        public ArrayList ObjectList { get; } = new ArrayList();

        public void Dispose()
        {
            Grid.Dispose();
            Player.Dispose();
            Wall.Dispose();
            Wall2.Dispose();
        }

        readonly Grid Grid;

        public Player Player;

        public Wall Wall, Wall2;

        public void RenderObjects()
        {
            Grid.Draw();
            Player.Draw();
            Wall.Draw();
            Wall2.Draw();
        }
    }
}
