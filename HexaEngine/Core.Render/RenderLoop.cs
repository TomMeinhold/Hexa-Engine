// <copyright file="RenderLoop.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Render
{
    using SharpDX;
    using SharpDX.Direct2D1;
    using SharpDX.DXGI;
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

        public event EventHandler EnterMainLoop;

        public void MainLoop()
        {
            EnterMainLoop?.Invoke(this, null);
            Stopwatch stopwatch = new Stopwatch();

            // Main loop
            RenderLoop.Run(this.Form, () =>
            {
                stopwatch.Restart();
                using var brush = new SolidColorBrush(this.DriectXManager.D2DDeviceContext, Color.Red);
                if (this.Drawing)
                {
                    if (Engine.Settings.AntialiasMode)
                    {
                        this.DriectXManager.D2DDeviceContext.AntialiasMode = AntialiasMode.PerPrimitive;
                    }
                    else
                    {
                        this.DriectXManager.D2DDeviceContext.AntialiasMode = AntialiasMode.Aliased;
                    }

                    this.DriectXManager.D2DDeviceContext.Target = this.DriectXManager.ObjectsBitmap;

                    this.DriectXManager.D2DDeviceContext.BeginDraw();
                    this.DriectXManager.D2DDeviceContext.Clear(Color.Transparent);
                    this.DrawRays();
                    this.Engine.SceneManager.RenderScene(this.DriectXManager.D2DDeviceContext);
                    this.Engine.SceneManager.RenderScene(this.DriectXManager.D3DDeviceContext);
                    this.Engine.UIManager.RenderUI(this.DriectXManager.D2DDeviceContext);
                    this.DriectXManager.D2DDeviceContext.EndDraw();

                    this.PostProcessingManager.PostProcess(input: this.DriectXManager.ObjectsBitmap, output: this.DriectXManager.TargetBitmap, this.Engine.Camera.TranslationMatrix);

                    if (Engine.Settings.DebugMode)
                    {
                        this.DriectXManager.D2DDeviceContext.BeginDraw();
                        this.DriectXManager.D2DDeviceContext.DrawText($"Physics: {this.Engine.PhysicsEngine.ThreadTiming.TotalMilliseconds} ms", this.DirectWrite.DefaultTextFormat, new RectangleF(0, 100, 200, 100), brush);
                        this.DriectXManager.D2DDeviceContext.EndDraw();
                    }

                    this.DriectXManager.SwapChain.Present(Engine.Settings.VSync, PresentFlags.None);
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
            this.DriectXManager.D2DDeviceContext.DrawImage(this.DriectXManager.RayBitmap);
            this.DriectXManager.D2DDeviceContext.Target = this.DriectXManager.RayBitmap;

            this.DriectXManager.D2DDeviceContext.Clear(Color.Transparent);
            this.DriectXManager.D2DDeviceContext.Target = this.DriectXManager.ObjectsBitmap;
        }
    }
}