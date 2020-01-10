using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System.Runtime.InteropServices;

namespace HexaEngine.Core.Common
{
    public static class ConvertBitmap
    {
        public static Bitmap Convert(RenderTarget renderTarget, System.Drawing.Bitmap Bitmap, bool alhpa = false)
        {

            // Loads from file using System.Drawing.Image
            using (var bitmap = Bitmap)
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
                var size = new System.Drawing.Size(bitmap.Width, bitmap.Height);
                if (alhpa)
                {
                    bitmap.MakeTransparent();
                }
                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

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
                    return new Bitmap(renderTarget, new Size2(size.Width, size.Height), tempStream, stride, bitmapProperties);
                }
            }
        }
        public static Bitmap Convert(DeviceContext renderTarget, System.Drawing.Bitmap Bitmap, bool alhpa = false)
        {

            // Loads from file using System.Drawing.Image
            using (var bitmap = Bitmap)
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
                var size = new System.Drawing.Size(bitmap.Width, bitmap.Height);
                if (alhpa)
                {
                    bitmap.MakeTransparent();
                }
                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

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
                    return new Bitmap(renderTarget, new Size2(size.Width, size.Height), tempStream, stride, bitmapProperties);
                }
            }
        }
        public static bool IsAlphaBitmap(ref System.Drawing.Imaging.BitmapData BmpData)
        {
            byte[] Bytes = new byte[BmpData.Height * BmpData.Stride];
            Marshal.Copy(BmpData.Scan0, Bytes, 0, Bytes.Length);
            for (int p = 3; p < Bytes.Length; p += 4)
            {
                if (Bytes[p] != 255) return true;
            }
            return false;
        }
    }
}
