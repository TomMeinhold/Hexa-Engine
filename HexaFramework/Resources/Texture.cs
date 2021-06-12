using System.Drawing;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace HexaFramework.Resources
{
    public class Texture : Resource
    {
        // Propertues
        public ID3D11ShaderResourceView TextureResource { get; private set; }

        public static implicit operator ID3D11ShaderResourceView(Texture texture)
        {
            return texture.TextureResource;
        }

        // Methods.
        public bool Load(ID3D11Device device, string fileName)
        {
            try
            {
                using ID3D11Texture2D texture = CreateTexture2DFromBitmap(device, fileName);
                ShaderResourceViewDescription srvDesc = new()
                {
                    Format = texture.Description.Format,
                    ViewDimension = Vortice.Direct3D.ShaderResourceViewDimension.Texture2D
                };
                srvDesc.Texture2D.MostDetailedMip = 0;
                srvDesc.Texture2D.MipLevels = -1;

                TextureResource = device.CreateShaderResourceView(texture, srvDesc);
                device.ImmediateContext.GenerateMips(TextureResource);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            TextureResource?.Dispose();
            TextureResource = null;
        }

        private ID3D11Texture2D CreateTexture2DFromBitmap(ID3D11Device device, string bitmapSource, float resolutionScale)
        {
            using var bitmap = new Bitmap(bitmapSource);
            return CreateTexture2DFromBitmap(device, bitmap, resolutionScale);
        }

        private ID3D11Texture2D CreateTexture2DFromBitmap(ID3D11Device device, string bitmapSource)
        {
            using var bitmap = new Bitmap(bitmapSource);
            return CreateTexture2DFromBitmap(device, bitmap);
        }

        private static ID3D11Texture2D CreateTexture2DFromBitmap(ID3D11Device device, Bitmap bitmapSource)
        {
            var data = bitmapSource.LockBits(new Rectangle(0, 0, bitmapSource.Width, bitmapSource.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var texture = device.CreateTexture2D
                (new Texture2DDescription()
                {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                    Usage = Vortice.Direct3D11.Usage.Default,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Format = Format.B8G8R8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.GenerateMips,
                    SampleDescription = new SampleDescription(1, 0),
                }, new SubresourceData[] { new SubresourceData(data.Scan0, data.Stride) });
            bitmapSource.UnlockBits(data);
            bitmapSource.Dispose();
            return texture;
        }

        private static ID3D11Texture2D CreateTexture2DFromBitmap(ID3D11Device device, Bitmap bitmapSource, float resolutionScale)
        {
            var scaledBitmap = new Bitmap((int)(bitmapSource.Width * resolutionScale), (int)(bitmapSource.Height * resolutionScale));
            var g = Graphics.FromImage(scaledBitmap);
            g.DrawImage(bitmapSource, 0, 0, (int)(bitmapSource.Width * resolutionScale), (int)(bitmapSource.Height * resolutionScale));
            g.Save();
            g.Dispose();
            var data = scaledBitmap.LockBits(new Rectangle(0, 0, scaledBitmap.Width, scaledBitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var texture = device.CreateTexture2D
                (new Texture2DDescription()
                {
                    Width = scaledBitmap.Size.Width,
                    Height = scaledBitmap.Size.Height,
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                    Usage = Vortice.Direct3D11.Usage.Default,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Format = Format.B8G8R8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.GenerateMips,
                    SampleDescription = new SampleDescription(1, 0),
                }, new SubresourceData[] { new SubresourceData(data.Scan0, data.Stride) });
            scaledBitmap.UnlockBits(data);
            scaledBitmap.Dispose();
            return texture;
        }
    }
}