using HexaEngine.Core.Plugins;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace HexaEngine.Core.Scripts
{
    public class ScriptCompiler
    {
        public ScriptCompiler(Engine engine)
        {
            Engine = engine;
        }

        public static CSharpCodeProvider CSharpCodeProvider { get; set; } = new CSharpCodeProvider();

        public static CompilerParameters Parameters { get; set; } = new CompilerParameters();

        public Engine Engine { get; }

        public void Compile(Plugin plugin)
        {
            if (!Engine.ScriptCache.Exists)
            {
                Engine.ScriptCache.Create();
            }

            FileInfo cacheFile = new FileInfo(Engine.ScriptCache.FullName + "\\" + plugin.Name + ".dll");

            if (cacheFile.Exists && Engine.UseScriptCache)
            {
                plugin.Assembly = Assembly.LoadFrom(cacheFile.FullName);
            }
            else
            {
                CreateCompilerParameters(plugin);
                BindReferences();
                BuildReferences(plugin);
                (bool, Assembly) compilerResults = Compile(plugin.GetScripts);
                if (!compilerResults.Item1)
                {
                    plugin.Assembly = compilerResults.Item2;
                }
            }
        }

        private void CreateCompilerParameters(Plugin plugin)
        {
            List<string> compilerOptions = new List<string>
            {
                "-optimize"
            };

            Parameters = new CompilerParameters
            {
                WarningLevel = 3,
                GenerateExecutable = false,
                GenerateInMemory = false,
                IncludeDebugInformation = false,
                CompilerOptions = string.Join(" ", compilerOptions),
                MainClass = "Program",
                OutputAssembly = Engine.ScriptCache.FullName + "\\" + plugin.Name + ".dll"
            };
        }

        private void BuildReferences(Plugin plugin)
        {
            foreach (string reference in plugin.References)
            {
                Parameters.ReferencedAssemblies.Add(reference + ".dll");
            }
        }

        private void BindReferences()
        {
            foreach (FileInfo file in new DirectoryInfo(Application.StartupPath).GetFiles("*.dll"))
            {
                Parameters.ReferencedAssemblies.Add(file.Name);
            }
            Parameters.ReferencedAssemblies.Add("System.dll");
            Parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            Parameters.ReferencedAssemblies.Add("System.Core.dll");
            Parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            Parameters.ReferencedAssemblies.Add("mscorlib.dll");
        }

        private (bool, Assembly) Compile(List<Script> scripts)
        {
            string[] code = scripts.ConvertAll(x => x.Code).ToArray();
            CompilerResults compilerResults = CSharpCodeProvider.CompileAssemblyFromSource(Parameters, code);

            if (compilerResults.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in compilerResults.Errors)
                {
                    sb.AppendLine($"Error ({error.ErrorNumber}): {error.ErrorText}, {error.Line} : {error.Column}");
                }

                Console.WriteLine(sb.ToString());
                return (true, null);
            }

            return (false, compilerResults.CompiledAssembly);
        }
    }
}