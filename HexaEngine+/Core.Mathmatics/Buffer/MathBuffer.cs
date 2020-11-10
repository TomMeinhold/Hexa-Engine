using System;
using System.Collections.Generic;

namespace HexaEngine.Core.Mathmatics.Buffer
{
    public class MathBuffer<T1, T2>
    {
        public MathBuffer(Func<T1, T2> func)
        {
            Func = func;
        }

        private Dictionary<T1, T2> BufferData { get; } = new Dictionary<T1, T2>();

        private Func<T1, T2> Func { get; }

        public T2 GetValue(T1 t1)
        {
            if (BufferData.ContainsKey(t1))
            {
                return BufferData[t1];
            }
            else
            {
                BufferData.Add(t1, Func(t1));
                return BufferData[t1];
            }
        }
    }
}