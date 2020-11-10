using HexaEngine.Core.Windows;
using System;

namespace EngineApplication
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using var app = new MainApplication();
            app.Run();
        }
    }

    internal class MainApplication : Application
    {
    }
}