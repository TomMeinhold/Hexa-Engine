using System.Collections.Generic;
using System.IO;

namespace HexaEngine.Core.Plugins
{
    public class PluginManager
    {
        public PluginManager()
        {
            foreach (FileInfo file in Engine.PluginsPath.GetFiles("*.hpln"))
            {
                Plugins.Add(Plugin.Load(file));
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

        public List<Plugin> Plugins { get; } = new List<Plugin>();
    }
}