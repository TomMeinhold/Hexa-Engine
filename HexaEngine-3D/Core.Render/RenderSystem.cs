// <copyright file="RenderSystem.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Render
{
    using HexaEngine.Core.Render.Components;
    using SharpDX.Direct2D1;
    using SharpDX.DXGI;
    using SharpDX.Windows;
    using System;
    using System.Threading;
    using AlphaMode = SharpDX.Direct2D1.AlphaMode;
    using FeatureLevel = SharpDX.Direct3D.FeatureLevel;

    /// <summary>
    /// Initial DirectX API.
    /// </summary>
    public partial class RenderSystem : IDisposable
    {
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        public RenderSystem(Engine engine, RenderForm form)
        {
            this.Form = form ?? throw new ArgumentNullException(nameof(form));
            this.DefaultContextMenuStrip = form.ContextMenuStrip;
            this.Engine = engine;
            this.InitializeDirectX();
        }

        ~RenderSystem()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing)
            // weiter unten ein.
            this.Dispose(false);
        }

        public DeviceManager DeviceManager { get; set; }

        public RessouceManager RessouceManager { get; set; }

        public DirectWriteManagement DirectWrite { get; set; } = new DirectWriteManagement();

        public PostProcessingManager PostProcessingManager { get; set; }

        public bool Drawing { get; set; } = true;

        public RenderForm Form { get; }

        public Engine Engine { get; set; }

        public bool ReloadDirectX { get; private set; }

        private EventWaitHandle WaitHandle { get; } = new EventWaitHandle(false, EventResetMode.AutoReset);

        public void InitializeDirectX()
        {
            var featureLevels = new FeatureLevel[] { FeatureLevel.Level_12_1, FeatureLevel.Level_12_0, FeatureLevel.Level_11_1, FeatureLevel.Level_11_0, FeatureLevel.Level_10_1, FeatureLevel.Level_10_0 };
            var pixelFormat = new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied);
            this.DeviceManager = new DeviceManager(this.Form, featureLevels, 3, pixelFormat);
            this.RessouceManager = new RessouceManager(this.DeviceManager);
            this.PostProcessingManager = new PostProcessingManager(this);
        }

        public void Resize()
        {
            this.RessouceManager.Resize(this.Engine.Settings.Width, this.Engine.Settings.Height);
        }

        public EventWaitHandle QueryReload()
        {
            this.ReloadDirectX = true;
            return this.WaitHandle;
        }

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
                    this.DeviceManager.Dispose();
                    this.RessouceManager.Dispose();
                    this.DirectWrite.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}