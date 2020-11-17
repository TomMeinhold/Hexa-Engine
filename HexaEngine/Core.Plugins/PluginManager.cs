using HexaEngine.Core.IO;
using HexaEngine.Core.Ressources;
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
                Plugin plugin = Plugin.Load(file);
                Plugins.Add(plugin);
                Archives.AddRange(plugin.Archives.ConvertAll(x => new Archive(HexaEngineArchive.Load(new FileInfo(Engine.ArchivesPath.FullName + x)))));
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

        public List<Archive> Archives { get; } = new List<Archive>();
    }
}