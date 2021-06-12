using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Mathematics;

namespace HexaFramework.Resources
{
    public class RenderTexture
    {
        private ID3D11Texture2D RenderTargetTexture { get; set; }
        private ID3D11RenderTargetView RenderTargetView { get; set; }
        public ID3D11ShaderResourceView ShaderResourceView { get; private set; }
        public ID3D11Texture2D DepthStencilBuffer { get; set; }
        public ID3D11DepthStencilView DepthStencilView { get; set; }
        public Viewport ViewPort { get; set; }

        public bool Initialize(ID3D11Device device)
        {
            try
            {
                // Initialize and set up the render target description.
                Texture2DDescription textureDesc = new()
                {
                    // Shadow Map Texture size as a 1024x1024 Square
                    Width = 1024,
                    Height = 1024,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.R32G32B32A32_Float,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = Vortice.Direct3D11.Usage.Default,
                    BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                };

                // Create the render target texture.
                RenderTargetTexture = device.CreateTexture2D(textureDesc);

                // Setup the description of the render target view.
                RenderTargetViewDescription renderTargetViewDesc = new()
                {
                    Format = textureDesc.Format,
                    ViewDimension = RenderTargetViewDimension.Texture2D,
                };
                renderTargetViewDesc.Texture2D.MipSlice = 0;

                // Create the render target view.
                RenderTargetView = device.CreateRenderTargetView(RenderTargetTexture, renderTargetViewDesc);

                // Setup the description of the shader resource view.
                ShaderResourceViewDescription shaderResourceViewDesc = new()
                {
                    Format = textureDesc.Format,
                    ViewDimension = ShaderResourceViewDimension.Texture2D,
                };
                shaderResourceViewDesc.Texture2D.MipLevels = 1;
                shaderResourceViewDesc.Texture2D.MostDetailedMip = 0;

                // Create the render target view.
                ShaderResourceView = device.CreateShaderResourceView(RenderTargetTexture, shaderResourceViewDesc);

                // Initialize and Set up the description of the depth buffer.
                Texture2DDescription depthStencilDesc = new()
                {
                    Width = 1024,
                    Height = 1024,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.D24_UNorm_S8_UInt,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = Vortice.Direct3D11.Usage.Default,
                    BindFlags = BindFlags.DepthStencil,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                };

                // Create the texture for the depth buffer using the filled out description.
                DepthStencilBuffer = device.CreateTexture2D(depthStencilDesc);

                // Initailze the depth stencil view description.
                DepthStencilViewDescription deothStencilViewDesc = new()
                {
                    Format = Format.D24_UNorm_S8_UInt,
                    ViewDimension = DepthStencilViewDimension.Texture2D
                };
                deothStencilViewDesc.Texture2D.MipSlice = 0;

                // Create the depth stencil view.
                DepthStencilView = device.CreateDepthStencilView(DepthStencilBuffer, deothStencilViewDesc);

                // Setup the viewport for rendering.
                ViewPort = new Viewport(0, 0, 1024.0f, 1024.0f, 0f, 1f);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetRenderTarget(ID3D11DeviceContext context)
        {
            // Bind the render target view and depth stencil buffer to the output pipeline.
            context.OMSetRenderTargets(RenderTargetView, DepthStencilView);

            // Set the viewport.
            context.RSSetViewport(ViewPort);
        }
    }
}