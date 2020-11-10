using HexaEngine.Core.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace HexaEngine.Core.Plugins
{
    public class Plugin
    {
        public Plugin()
        {
        }

        public static Plugin Load(FileInfo file)
        {
            using var fs = file.OpenRead();
            XmlSerializer serializer = new XmlSerializer(typeof(Plugin));
            Plugin plugin = (Plugin)serializer.Deserialize(fs);
            plugin.File = file;
            fs.Close();
            return plugin;
        }

        public void Save(FileInfo file)
        {
            using var fs = file.Open(FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Plugin));
            serializer.Serialize(fs, this);
            fs.Close();
        }

        public void Compile()
        {
            Engine.Current.Compiler.Compile(this);
        }

        public void Execute()
        {
            try
            {
                Type program = Assembly.GetType(Name + ".Program");

                MethodInfo main = program.GetMethod("Main");
                if (main.GetParameters().Length == 0)
                {
                    main.Invoke(null, null);
                    return;
                }

                foreach (ParameterInfo p in main.GetParameters())
                {
                    if (p.ParameterType == typeof(Engine))
                    {
                        main.Invoke(null, null);
                    }
                }
            }
#pragma warning disable CA1031 // Keine allgemeinen Ausnahmetypen abfangen. Die Ausnahme kann nur von einem externen fehler verusacht werden. Und soll nur weitergeleitet werden.
            catch (Exception e)
#pragma warning restore CA1031 // Keine allgemeinen Ausnahmetypen abfangen. Die Ausnahme kann nur von einem externen fehler verusacht werden. Und soll nur weitergeleitet werden.
            {
                Console.WriteLine(e);
            }
        }

        public List<string> References { get; set; } = new List<string>();

        public List<string> Scripts { get; set; } = new List<string>();

        [XmlIgnore]
        public List<Script> GetScripts { get => Scripts.ConvertAll((x) => new Script(this, new FileInfo(Engine.ScriptSourcePath.FullName + "\\" + Name + "\\" + x + ".cs"))); }

        [XmlIgnore]
        public Assembly Assembly { get; set; }

        [XmlIgnore]
        public string Name { get => File.Name.Replace(".hpln", ""); }

        [XmlIgnore]
        public FileInfo File { get; set; }
    }
}