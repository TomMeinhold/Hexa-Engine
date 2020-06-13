using System;
using System.Windows.Forms;

namespace Game
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt f�r die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow window = new MainWindow();
            window.Dispose();
        }
    }
}
