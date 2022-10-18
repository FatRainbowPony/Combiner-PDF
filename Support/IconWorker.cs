using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Combiner_PDF.Support
{
    public static class IconWorker
    {
        public static ImageSource FileToImageIconConverter(string filePath)
        {
            ImageSource icon = default;

            if (File.Exists(filePath))
            {
                using (Icon sysIcon = Icon.ExtractAssociatedIcon(filePath))
                {
                    icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        sysIcon.Handle, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                }
            }

            return icon;
        }
    }
}
