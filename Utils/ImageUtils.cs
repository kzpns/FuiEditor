using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FuiEditor.Utils
{
    static class ImageUtils
    {
        //CreateThumbnail method, thx!
        //https://www.atmarkit.co.jp/ait/articles/0508/12/news091.html
        public static Image CreateThumbnail(Image original, Size size)
        {
            int w = size.Width;
            int h = size.Height;

            Bitmap canvas = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);

            float fw = (float)w / (float)original.Width;
            float fh = (float)h / (float)original.Height;

            float scale = Math.Min(fw, fh);
            fw = original.Width * scale;
            fh = original.Height * scale;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g.DrawImage(original,
                new RectangleF((w - fw) / 2, (h - fh) / 2, fw, fh),
                new RectangleF(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
            g.Dispose();

            return canvas;
        }

        public static void ReverseColorRB(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int stride = data.Stride;
            IntPtr pixelData = data.Scan0;

            for(int x=0; x<width; x++)
            {
                for(int y=0; y<height; y++)
                {
                    IntPtr pixelOffset = pixelData + 4 * x + stride * y;
                    byte[] color = new byte[4];
                    Marshal.Copy(pixelOffset, color, 0, 4);

                    byte red = color[0];
                    byte blue = color[2];

                    color[0] = blue;
                    color[2] = red;

                    Marshal.Copy(color, 0, pixelOffset, 4);
                }
            }

            bitmap.UnlockBits(data);
        }
    }
}
