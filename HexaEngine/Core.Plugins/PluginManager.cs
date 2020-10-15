using System.Collections.Generic;
using System.IO;

namespace HexaEngine.Core.Plugins
{
    public class PluginManager
    {
        public PluginManager(Engine engine)
        {
            Engine = engine;
            foreach (FileInfo file in Engine.PluginsPath.GetFiles("*.hpln"))
            {
                Plugins.Add(Plugin.Load(Engine, file));
            }

            foreach (Plugin plugin in Plugins)
            {
                plugin.Compile();
            }

            foreach (Plugin plugin in Plugins)
            {
                plugin.Execute();
            }
        }

        public Engine Engine { get; }

        public List<Plugin> Plugins { get; } = new List<Plugin>();
    }
}