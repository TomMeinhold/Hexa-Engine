// <copyright file="RenderLoop.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Render
{
    using SharpDX;
    using SharpDX.Direct2D1;
    using SharpDX.Direct2D1.Effects;
    using SharpDX.DXGI;
    using SharpDX.Mathematics.Interop;
    using SharpDX.Windows;
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    /// <summary>
    /// Initial DirectX API.
    /// </summary>
    public partial class RenderSystem
    {
        public ContextMenuStrip DefaultContextMenuStrip { get; internal set; }

        public void MainLoop()
        {
            Stopwatch stopwatch = new Stopwatch();

            // Main loop
            RenderLoop.Run(this.Form, () =>
            {
                stopwatch.Restart();
                using var brush = new SolidColorBrush(this.RessouceManager.D2DDeviceContext, Color.Red);
                if (this.Drawing)
                {
                    if (Engine.Settings.AntialiasMode)
                    {
                        this.RessouceManager.D2DDeviceContext.AntialiasMode = AntialiasMode.PerPrimitive;
                    }
                    else
                    {
                        this.RessouceManager.D2DDeviceContext.AntialiasMode = AntialiasMode.Aliased;
                    }

                    this.RessouceManager.D2DDeviceContext.Target = this.RessouceManager.ObjectsBitmap;

                    this.RessouceManager.D2DDeviceContext.BeginDraw();
                    this.RessouceManager.D2DDeviceContext.Clear(Color.Transparent);
                    this.DrawRays();
                    this.Engine.SceneManager.RenderScene(this.RessouceManager.D2DDeviceContext);
                    this.Engine.UIManager.RenderUI(this.RessouceManager.D2DDeviceContext);
                    this.RessouceManager.D2DDeviceContext.EndDraw();

                    this.PostProcessingManager.PostProcess(input: this.RessouceManager.ObjectsBitmap, output: this.RessouceManager.TargetBitmap, this.Engine.Camera.TranslationMatrix);

                    this.RessouceManager.SwapChain.Present(Engine.Settings.VSync, PresentFlags.None);
                    this.Engine.ThreadSyncTiming = stopwatch.ElapsedTicks;
                }

                if (this.ReloadDirectX)
                {
                    this.ReloadDirectX = false;
                    this.Resize();
                    GC.Collect();
                    this.WaitHandle.Set();
                }
            });
        }

        private void DrawRays()
        {
            this.RessouceManager.D2DDeviceContext.DrawImage(this.RessouceManager.RayBitmap);
            this.RessouceManager.D2DDeviceContext.Target = this.RessouceManager.RayBitmap;

            this.RessouceManager.D2DDeviceContext.Clear(Color.Transparent);
            this.RessouceManager.D2DDeviceContext.Target = this.RessouceManager.ObjectsBitmap;
        }
    }
}