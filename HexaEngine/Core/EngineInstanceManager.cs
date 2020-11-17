using HexaEngine.Core.Windows;
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

        public static void Create()
        {
            new Engine();
        }

        public static void Create(IRenderable renderable)
        {
            new Engine(renderable);
        }
    }
}