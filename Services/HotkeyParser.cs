using System;
using System.Windows.Forms;

namespace FastShot.Services;

public static class HotkeyParser
{
    public static (uint mods, uint vk) Parse(string hotkey)
    {
        uint mods = 0;
        var parts = hotkey.ToUpperInvariant().Split('+');
        string keyStr = "S";
        foreach (var p in parts)
        {
            switch (p.Trim())
            {
                case "CTRL":  mods |= 0x0002; break;
                case "SHIFT": mods |= 0x0004; break;
                case "ALT":   mods |= 0x0001; break;
                case "WIN":   mods |= 0x0008; break;
                default: keyStr = p.Trim(); break;
            }
        }
        Enum.TryParse<Keys>(keyStr, true, out var key);
        return (mods, (uint)key);
    }
}
