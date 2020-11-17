// <copyright file="RenderLoop.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Render
{
    using HexaEngine.Core.Windows;
    using SharpDX;
    using SharpDX.Direct2D1;
    using SharpDX.DXGI;
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
            Control control = (Control)Renderable;
            // Main loop
            RenderLoop.Run(control, () =>
            {
                stopwatch.Restart();
                using var brush = new SolidColorBrush(DriectXManager.D2DDeviceContext, Color.Red);
                if (Drawing)
                {
                    if (Engine.Current.Settings.AntialiasMode)
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

                    Engine.Current.SceneManager.RenderScene(DriectXManager.D2DDeviceContext);

                    DriectXManager.D2DDeviceContext.DrawImage(DriectXManager.ShadowMaskBitmap);
                    Engine.Current.UIManager.RenderUI(DriectXManager.D2DDeviceContext);
                    DriectXManager.D2DDeviceContext.EndDraw();

                    PostProcessingManager.PostProcess(input: DriectXManager.ObjectsBitmap, output: DriectXManager.TargetBitmap, Engine.Current.Camera.TranslationMatrix);

                    if (Engine.Current.Settings.DebugMode)
                    {
                        DriectXManager.D2DDeviceContext.BeginDraw();
                        DriectXManager.D2DDeviceContext.DrawText($"Render: {stopwatch.Elapsed.TotalMilliseconds} ms", DirectWrite.DefaultTextFormat, new RectangleF(0, 100, 200, 100), brush);
                        DriectXManager.D2DDeviceContext.DrawText($"Physics: {Engine.Current.PhysicsEngine.ThreadTiming.TotalMilliseconds} ms", DirectWrite.DefaultTextFormat, new RectangleF(0, 120, 200, 100), brush);
                        DriectXManager.D2DDeviceContext.DrawText($"Scence: {Engine.Current.SceneManager.SelectedScene?.Objects.Count ?? 0} objects", DirectWrite.DefaultTextFormat, new RectangleF(0, 140, 200, 100), brush);
                        DriectXManager.D2DDeviceContext.DrawLine(new Vector2(0, Renderable.Height / 2), new Vector2(Renderable.Width, Renderable.Height / 2), brush);
                        DriectXManager.D2DDeviceContext.DrawLine(new Vector2(Renderable.Width / 2, 0), new Vector2(Renderable.Width / 2, Renderable.Height), brush);
                        DriectXManager.D2DDeviceContext.EndDraw();
                    }

                    DriectXManager.SwapChain.Present(Engine.Current.Settings.VSync, PresentFlags.None);
                    Engine.Current.ThreadSyncTiming = stopwatch.ElapsedTicks;
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