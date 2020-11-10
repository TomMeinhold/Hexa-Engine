// <copyright file="RenderSystem.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Render
{
    using HexaEngine.Core.Render.Components;
    using HexaEngine.Core.Ressources;
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
            Form = form ?? throw new ArgumentNullException(nameof(form));
            DefaultContextMenuStrip = form.ContextMenuStrip;
            Engine = engine;
            InitializeDirectX();
        }

        ~RenderSystem()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing)
            // weiter unten ein.
            Dispose(false);
        }

        public DeviceManager DeviceManager { get; set; }

        public DriectXManager DriectXManager { get; set; }

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
            DeviceManager = new DeviceManager(Form, featureLevels, 3, pixelFormat);
            DriectXManager = new DriectXManager(DeviceManager);
            PostProcessingManager = new PostProcessingManager(this);
        }

        public void Resize()
        {
            DriectXManager.Resize(Engine.Settings.Width, Engine.Settings.Height);
        }

        public void FlipFullscreen()
        {
            if (DeviceManager.SwapChain.IsFullScreen)
            {
                DeviceManager.SwapChain.SetFullscreenState(false, null);
            }
            else
            {
                DeviceManager.SwapChain.SetFullscreenState(true, null);
            }
        }

        public EventWaitHandle QueryReload()
        {
            ReloadDirectX = true;
            return WaitHandle;
        }

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
                    DeviceManager.Dispose();
                    DriectXManager.Dispose();
                    DirectWrite.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}