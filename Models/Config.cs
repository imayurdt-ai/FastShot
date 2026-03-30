using System.IO;
using System;

namespace FastShot.Models;

public class Config
{
    public string Hotkey     { get; set; } = "Ctrl+Shift+S";
    public string SavePath   { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Screenshots");
    public bool   SaveToDisk { get; set; } = true;
    public bool   ShowToast  { get; set; } = true;
}
