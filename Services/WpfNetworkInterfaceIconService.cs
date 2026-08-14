using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkDiagram
{
    internal static class WpfNetworkInterfaceIconService
    {
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr handle);

        public static ImageSource GetIcon(NetworkAdapter adapter, int width, int height)
        {
            ThemePalette palette = ThemeService.CurrentPalette;
            System.Drawing.Color strokeColor = palette.SoftTextColor;
            System.Drawing.Color accentColor = palette.PrimaryColor;

            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(
                NetworkInterfaceIconService.GetIcon(
                    adapter,
                    new System.Drawing.Size(width, height),
                    strokeColor,
                    accentColor)))
            {
                IntPtr handle = bitmap.GetHbitmap();
                try {
                    BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(
                        handle,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromWidthAndHeight(width, height));
                    source.Freeze();
                    return source;
                }
                finally {
                    DeleteObject(handle);
                }
            }
        }
    }
}
