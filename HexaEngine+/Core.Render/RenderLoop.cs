﻿// <copyright file="RenderLoop.cs" company="PlaceholderCompany">
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

        public SolidColorBrush Transparent;

        public SolidColorBrush Black;

        public void MainLoop()
        {
            EnterMainLoop?.Invoke(this, null);
            Transparent = new SolidColorBrush(DriectXManager.D2DDeviceContext, Color.Transparent);
            Black = new SolidColorBrush(DriectXManager.D2DDeviceContext, Color.Black);
            Stopwatch stopwatch = new Stopwatch();

            // Main loop
            RenderLoop.Run(Form, () =>
            {
                stopwatch.Restart();
                using var brush = new SolidColorBrush(DriectXManager.D2DDeviceContext, Color.Red);
                if (Drawing)
                {
                    if (Engine.Settings.AntialiasMode)
                    {
                        DriectXManager.D2DDeviceContext.AntialiasMode = AntialiasMode.PerPrimitive;
                    }
                    else
                    {
                        DriectXManager.D2DDeviceContext.AntialiasMode = AntialiasMode.Aliased;
                    }

                    DriectXManager.D2DDeviceContext.Target = DriectXManager.ObjectsBitmap;

                    DriectXManager.D2DDeviceContext.BeginDraw();
                    DriectXManager.D2DDeviceContext.Clear(Color.Transparent);
                    DrawRays();

                    Engine.SceneManager.RenderScene(DriectXManager.D2DDeviceContext);

                    DriectXManager.D2DDeviceContext.DrawImage(DriectXManager.ShadowMaskBitmap);
                    Engine.UIManager.RenderUI(DriectXManager.D2DDeviceContext);
                    DriectXManager.D2DDeviceContext.EndDraw();

                    PostProcessingManager.PostProcess(input: DriectXManager.ObjectsBitmap, output: DriectXManager.TargetBitmap, Engine.Camera.TranslationMatrix);

                    if (Engine.Settings.DebugMode)
                    {
                        DriectXManager.D2DDeviceContext.BeginDraw();
                        DriectXManager.D2DDeviceContext.DrawText($"Physics: {Engine.PhysicsEngine.ThreadTiming.TotalMilliseconds} ms", DirectWrite.DefaultTextFormat, new RectangleF(0, 100, 200, 100), brush);
                        DriectXManager.D2DDeviceContext.EndDraw();
                    }

                    DriectXManager.SwapChain.Present(Engine.Settings.VSync, PresentFlags.None);
                    Engine.ThreadSyncTiming = stopwatch.ElapsedTicks;
                }

                if (ReloadDirectX)
                {
                    ReloadDirectX = false;
                    Resize();
                    GC.Collect();
                    WaitHandle.Set();
                }
            });
        }

        private void DrawShadows()
        {
            DriectXManager.D2DDeviceContext.Target = DriectXManager.ShadowMaskBitmap;
            DriectXManager.D2DDeviceContext.Clear(Color.White);
            DriectXManager.D2DDeviceContext.Target = DriectXManager.ObjectsBitmap;
        }

        private void DrawRays()
        {
            DriectXManager.D2DDeviceContext.DrawImage(DriectXManager.RayBitmap);
            DriectXManager.D2DDeviceContext.Target = DriectXManager.RayBitmap;

            DriectXManager.D2DDeviceContext.Clear(Color.Transparent);
            DriectXManager.D2DDeviceContext.Target = DriectXManager.ObjectsBitmap;
        }
    }
}