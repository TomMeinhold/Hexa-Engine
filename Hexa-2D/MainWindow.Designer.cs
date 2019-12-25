using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.DXGI.Factory;

namespace HexaMain
{
    partial class MainWindow
    {
        private Device Device;

        private SwapChain SwapChain;

        private RenderTarget D2DRenderTarget;

        private RenderTargetView RenderView;

        private Factory D3DFactory;

        private SharpDX.Direct2D1.Factory D2DFactory;

        private Texture2D BackBuffer;

        private SwapChainDescription SwapChainDescription;

        public bool LockRenderTarget = false;

        public bool LockModifys = true;


        private void InitializeDirectX()
        {
            // SwapChain description
            SwapChainDescription = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(ClientSize.Width, ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                Flags = SwapChainFlags.None,
                IsWindowed = !IsFullscreen,
                OutputHandle = Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };
            // Create Device and SwapChain
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new SharpDX.Direct3D.FeatureLevel[] { SharpDX.Direct3D.FeatureLevel.Level_11_0 }, SwapChainDescription, out Device, out SwapChain);

            D2DFactory = new SharpDX.Direct2D1.Factory();

            // Ignore all windows events
            D3DFactory = SwapChain.GetParent<Factory>();
            D3DFactory.MakeWindowAssociation(Handle, WindowAssociationFlags.None);

            // New RenderTargetView from the backbuffer
            BackBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(SwapChain, 0);
            RenderView = new RenderTargetView(Device, BackBuffer);

            Surface surface = BackBuffer.QueryInterface<Surface>();

            D2DRenderTarget = new RenderTarget(D2DFactory, surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            D2DRenderTarget.AntialiasMode = AntialiasMode.Aliased;
        }

        public void ReloadDirectX()
        {
            while (LockModifys)
            D2DFactory.Dispose();
            D2DRenderTarget.Dispose();
            RenderView.Dispose();
            BackBuffer.Dispose();
            Device.Dispose();
            SwapChain.Dispose();
            D3DFactory.Dispose();
            InitializeDirectX();
            Engine.Brushpalette.Dispose();
            Engine.Initial(D2DRenderTarget);
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        
        protected override void Dispose(bool disposing)
        {
            Engine.Dispose();
            D2DFactory.Dispose();
            D2DRenderTarget.Dispose();
            RenderView.Dispose();
            BackBuffer.Dispose();
            Device.Dispose();
            SwapChain.Dispose();
            D3DFactory.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainWindow
            // 
            this.AllowUserResizing = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}