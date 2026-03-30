using System.Drawing;
using System.Windows.Forms;

namespace FastShot.Services;

public static class ScreenshotService
{
    public static Bitmap CaptureAll()
    {
        var vs = SystemInformation.VirtualScreen;
        var bmp = new Bitmap(vs.Width, vs.Height);
        using var g = Graphics.FromImage(bmp);
        g.CopyFromScreen(vs.Left, vs.Top, 0, 0, vs.Size);
        return bmp;
    }

    public static Bitmap CapturePrimary()
    {
        var s = Screen.PrimaryScreen!.Bounds;
        var bmp = new Bitmap(s.Width, s.Height);
        using var g = Graphics.FromImage(bmp);
        g.CopyFromScreen(s.Left, s.Top, 0, 0, s.Size);
        return bmp;
    }
}
