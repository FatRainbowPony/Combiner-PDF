using System;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Media;

namespace Combiner_PDF.Support
{
    public class BitmapWorker
    {

        public static BitmapSource ToBitmapSource(Image image)
        {
            return ToBitmapSource(image as Bitmap);
        }

        public static BitmapSource ToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null) return null;

            using (System.Drawing.Bitmap source = (System.Drawing.Bitmap)bitmap.Clone())
            {
                IntPtr ptr = source.GetHbitmap();

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                NativeMethods.DeleteObject(ptr);
                bs.Freeze();
                return bs;
            }
        }

        public static BitmapSource ToBitmapSource(byte[] bytes, int width, int height, int dpiX, int dpiY)
        {
            var result = BitmapSource.Create(width, height, dpiX, dpiY, PixelFormats.Default, null, bytes,width * 4);
            result.Freeze();

            return result;
        }
    }
}
