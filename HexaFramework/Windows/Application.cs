using HexaFramework.Windows.Native;
using System;

namespace HexaFramework.Windows
{
    public static class Application
    {
        private static bool _exitRequested;

        public static RenderWindow MainWindow { get; private set; }

        public static void Run(RenderWindow window)
        {
            MainWindow = window;
            window.Show();
            PlatformRun();
        }

        public static void Exit()
        {
            User32.PostQuitMessage(0);
        }

        private static void PlatformRun()
        {
            while (!_exitRequested)
            {
                if (MainWindow.Focus)
                {
                    const uint PM_REMOVE = 1;
                    while (User32.PeekMessage(out var msg, IntPtr.Zero, 0, 0, PM_REMOVE))
                    {
                        User32.TranslateMessage(ref msg);
                        User32.DispatchMessage(ref msg);

                        if (msg.Value == (uint)WindowMessage.Quit)
                        {
                            _exitRequested = true;
                            break;
                        }
                    }
                }
                else if (!MainWindow.IsShown)
                {
                    _exitRequested = true;
                }
                else
                {
                    var ret = User32.GetMessage(out var msg, IntPtr.Zero, 0, 0);
                    if (ret == 0)
                    {
                        _exitRequested = true;
                        break;
                    }
                    else if (ret == -1)
                    {
                        _exitRequested = true;
                        break;
                    }
                    else
                    {
                        User32.TranslateMessage(ref msg);
                        User32.DispatchMessage(ref msg);
                    }
                }
            }
            MainWindow.Dispose();
        }
    }
}