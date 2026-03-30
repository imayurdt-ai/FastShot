using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FastShot.Services;

public static class FileStorageService
{
    public static string? Save(Bitmap bmp, string folder)
    {
        try
        {
            Directory.CreateDirectory(folder);
            var name = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var path = Path.Combine(folder, name);
            bmp.Save(path, ImageFormat.Png);
            return path;
        }
        catch { return null; }
    }
}
