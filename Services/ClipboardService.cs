using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace FastShot.Services;

public static class ClipboardService
{
    [DllImport("gdi32.dll")] static extern bool DeleteObject(IntPtr hObj);

    public static void Copy(Bitmap bmp)
    {
        var hBmp = bmp.GetHbitmap();
        try
        {
            var src = Imaging.CreateBitmapSourceFromHBitmap(
                hBmp, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            Clipboard.SetImage(src);
        }
        finally { DeleteObject(hBmp); }
    }
}
