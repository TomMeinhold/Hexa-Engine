using System;

namespace Application
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            MainWindow window = new MainWindow();
            System.Windows.Forms.Application.Run(window);
            window.Dispose();
        }
    }
}