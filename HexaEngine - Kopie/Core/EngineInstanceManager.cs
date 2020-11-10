using System;
using System.Collections.Generic;

namespace HexaEngine.Core
{
    public partial class Engine
    {
        private static Dictionary<int, Engine> Instances { get; } = new Dictionary<int, Engine>();

        public static Engine Current { get => GetCurrentEngine(); }

        private static Engine GetCurrentEngine()
        {
            if (Instances.ContainsKey(AppDomain.CurrentDomain.Id))
            {
                return Instances[AppDomain.CurrentDomain.Id];
            }
            else
            {
                return null;
            }
        }
    }
}