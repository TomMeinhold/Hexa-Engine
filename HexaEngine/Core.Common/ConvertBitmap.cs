using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HexaEngine.Core.Common
{
    public static class ConvertBitmap
    {
        public static Bitmap1 Convert(SharpDX.Direct2D1.DeviceContext deviceContext, System.Drawing.Bitmap bitmap, bool alhpa = true)
        {
            // Loads from file using System.Drawing.Image
            var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapProperties = new BitmapProperties1(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            var size = new System.Drawing.Size(bitmap.Width, bitmap.Height);
            if (alhpa)
            {
                bitmap.MakeTransparent();
            }

            // Transform pixels from BGRA to RGBA
            int stride = bitmap.Width * sizeof(int);
            using var tempStream = new DataStream(bitmap.Height * stride, true, true);

            // Lock System.Drawing.Bitmap
            var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Convert all pixels
            for (int y = 0; y < bitmap.Height; y++)
            {
                int offset = bitmapData.Stride * y;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    // Not optimized
                    byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                    byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                    byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                    byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                    int rgba = R | (G << 8) | (B << 16) | (A << 24);
                    tempStream.Write(rgba);
                }
            }

            bitmap.UnlockBits(bitmapData);
            tempStream.Position = 0;
            return new Bitmap1(deviceContext, new Size2(size.Width, size.Height), tempStream, stride, bitmapProperties);
        }

        public static Texture2D Convert(SharpDX.Direct3D11.DeviceContext deviceContext, System.Drawing.Bitmap bitmap)
        {
            if (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                bitmap = bitmap.Clone(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var texture2DDescription = new Texture2DDescription
            {
                Width = bitmap.Size.Width,
                Height = bitmap.Size.Height,
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                Usage = ResourceUsage.Immutable,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
            };
            Texture2D texture2D = new Texture2D(deviceContext.Device, texture2DDescription, new DataRectangle(data.Scan0, data.Stride));
            bitmap.UnlockBits(data);
            return texture2D;
        }
    }
}