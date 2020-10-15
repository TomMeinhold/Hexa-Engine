﻿// <copyright file="InputSystem.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Input
{
    using HexaEngine.Core.Input.Component;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Input Management.
    /// </summary>
    public partial class InputSystem : IDisposable
    {
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        public InputSystem(Form form)
        {
            this.Form = form ?? throw new ArgumentNullException(nameof(form));
            this.Form.Activated += this.MainWindow_Activated;
            this.Form.Deactivate += this.MainWindow_Deactivate;
            this.Form.MouseWheel += this.Form_MouseWheel;
            this.Form.MouseMove += this.Form_MouseMove;
            this.Form.MouseDown += this.Form_MouseDown;
            this.Form.MouseUp += this.Form_MouseUp;
            this.Form.KeyDown += this.Form_KeyDown;
            this.Form.KeyUp += this.Form_KeyUp;
        }

        ~InputSystem()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing)
            // weiter oben ein.
            this.Dispose(false);
        }

        public bool Active { get; set; }

        public static MouseState MouseState { get; } = new MouseState();

        public static KeyboardState KeyboardState { get; } = new KeyboardState();

        public static event EventHandler<KeyboardUpdatePackage> KeyboardUpdate;

        public static event EventHandler<MouseUpdatePackage> MouseUpdate;

        public Form Form { get; }

        /// <summary>
        /// Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        /// </summary>
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing)
            // weiter oben ein.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }

                this.disposedValue = true;
            }
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            this.Active = true;
        }

        private void MainWindow_Deactivate(object sender, EventArgs e)
        {
            this.Active = false;
        }
    }
}