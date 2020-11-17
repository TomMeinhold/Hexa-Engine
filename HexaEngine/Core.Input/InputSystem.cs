// <copyright file="InputSystem.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input
{
    using HexaEngine.Core.Input.Component;
    using HexaEngine.Core.Windows;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Input Management.
    /// </summary>
    public partial class InputSystem : IDisposable
    {
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        public InputSystem(IRenderable renderable)
        {
            Control = renderable as Control ?? throw new ArgumentNullException(nameof(renderable));
            Control.MouseWheel += Form_MouseWheel;
            Control.MouseMove += Form_MouseMove;
            Control.MouseDown += Form_MouseDown;
            Control.MouseUp += Form_MouseUp;
            Control.KeyDown += Form_KeyDown;
            Control.KeyUp += Form_KeyUp;
        }

        ~InputSystem()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing)
            // weiter oben ein.
            Dispose(false);
        }

        public bool Active { get; set; }

        public static MouseState MouseState { get; } = new MouseState();

        public static KeyboardState KeyboardState { get; } = new KeyboardState();

        public static event EventHandler<KeyboardUpdatePackage> KeyboardUpdate;

        public static event EventHandler<MouseUpdatePackage> MouseUpdate;

        public Control Control { get; }

        /// <summary>
        /// Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        /// </summary>
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing)
            // weiter oben ein.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            Active = true;
        }

        private void MainWindow_Deactivate(object sender, EventArgs e)
        {
            Active = false;
        }
    }
}