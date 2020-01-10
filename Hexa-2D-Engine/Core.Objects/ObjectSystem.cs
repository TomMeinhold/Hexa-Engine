using HexaEngine.Core.Common;
using HexaEngine.Core.Game;
using SharpDX.Mathematics.Interop;
using System.Collections;
using System.Collections.Generic;

namespace HexaEngine.Core.Objects
{
    public class ObjectSystem
    {
        public ObjectSystem(Engine engineCore)
        {
            Grid = new Grid(engineCore);
            State = ObjectSystemState.Initialized;
        }

        public ArrayList ObjectList { get; } = new ArrayList();

        public ObjectSystemState State;

        public void Dispose()
        {
            Grid.Dispose();
            foreach (dynamic s in ObjectList)
            {
                s.Dispose();
            }
        }

        public void SetState(ObjectSystemState state)
        {
            State = state;
        }

        public void Finishing()
        {
            ObjectList.Sort(new SortObjectList());
            State = ObjectSystemState.Active;
        }

        readonly Grid Grid;

        public PlayerBase Player;


        public void RenderObjects()
        {
            Grid.Draw();
            foreach (dynamic s in ObjectList)
            {
                s.Draw();
            }
        }

        public class SortObjectList : IComparer
        {
            public int Compare(dynamic _x, dynamic _y)
            {
                return _x.Position.Z.CompareTo(_y.Position.Z);
            }
        }
    }
}
