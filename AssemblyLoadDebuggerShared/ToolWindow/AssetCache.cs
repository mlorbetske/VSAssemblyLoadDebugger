using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ImageAttributes = Microsoft.VisualStudio.Imaging.Interop.ImageAttributes;

namespace ToolWindow
{
    public static class AssetCache
    {
        private static readonly ConcurrentDictionary<string, BitmapSource> Cache = new ConcurrentDictionary<string, BitmapSource>(StringComparer.Ordinal);

        public static ImageSource GetIconForImageMoniker(ImageMoniker? imageMoniker, int sizeX, int sizeY)
        {
            if (imageMoniker == null)
            {
                return null;
            }

            IVsImageService2 vsIconService = ServiceProvider.GlobalProvider.GetService(typeof(SVsImageService)) as IVsImageService2;

            if (vsIconService == null)
            {
                return null;
            }

            try
            {
                ImageAttributes imageAttributes = new ImageAttributes
                {
                    Flags = (uint) _ImageAttributesFlags.IAF_RequiredFlags,
                    ImageType = (uint) _UIImageType.IT_Bitmap,
                    Format = (uint) _UIDataFormat.DF_WPF,
                    LogicalHeight = sizeY,
                    LogicalWidth = sizeX,
                    StructSize = Marshal.SizeOf(typeof(ImageAttributes))
                };

                IVsUIObject result = vsIconService.GetImage(imageMoniker.Value, imageAttributes);

                object data;
                result.get_Data(out data);
                ImageSource glyph = data as ImageSource;

                if (glyph != null)
                {
                    glyph.Freeze();
                }

                return glyph;
            }
            catch 
            {
                Debug.Fail("Unable to get image.");
            }

            return null;
        }
    }
}