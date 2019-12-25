using SharpDX.Mathematics.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Common
{
    public class RawVector2Buffer
    {
        private readonly Array[] buffer = new Array[32];

        int Tail = 0;

        int Head = 0;

        public RawVector2[] GetBuffer()
        {
            while (Head == Tail) { Thread.Sleep(1); }
            if (Head == buffer.Length)
            {
                Head = 0;
            }
            RawVector2[] list = (RawVector2[])buffer.GetValue(Head);
            Head++;
            return list;
        }
        public void AddBuffer(RawVector2[] list)
        {
            while (Tail < Head) { Thread.Sleep(1); }
            if (Tail == buffer.Length)
            {
                Tail = 0;
            }
            buffer.SetValue(list, Tail);
            Tail++;
        }
    }
}
