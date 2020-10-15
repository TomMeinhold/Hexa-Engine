using HexaEngine.Core.Plugins;
using System.IO;
using System.Text;

namespace HexaEngine.Core.Scripts
{
    public class Script
    {
        public Script(Plugin plugin, FileInfo file)
        {
            Plugin = plugin;
            File = file;
        }

        public Plugin Plugin { get; }

        public FileInfo File { get; }

        public string Code { get => GetCode(); }

        private string GetCode()
        {
            using var fs = File.OpenRead();
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            return Encoding.UTF8.GetString(buffer);
        }
    }
}