using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace FastShot.Services;

public sealed class HotkeyService : IDisposable
{
    [DllImport("user32.dll")] static extern bool RegisterHotKey(IntPtr h, int id, uint mod, uint vk);
    [DllImport("user32.dll")] static extern bool UnregisterHotKey(IntPtr h, int id);

    const int WM_HOTKEY = 0x0312;
    const int ID = 9001;

    readonly IntPtr _hwnd;
    bool _disposed;
    public event Action? Fired;

    public HotkeyService(IntPtr hwnd, uint modifiers, uint vk)
    {
        _hwnd = hwnd;
        if (!RegisterHotKey(_hwnd, ID, modifiers, vk))
            throw new InvalidOperationException(
                "Could not register hotkey — it may already be in use by another app.");
        var src = HwndSource.FromHwnd(_hwnd);
        src?.AddHook(WndProc);
    }

    IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_HOTKEY && wParam.ToInt32() == ID)
        {
            Fired?.Invoke();
            handled = true;
        }
        return IntPtr.Zero;
    }

    public void Dispose()
    {
        if (_disposed) return;
        UnregisterHotKey(_hwnd, ID);
        var src = HwndSource.FromHwnd(_hwnd);
        src?.RemoveHook(WndProc);
        _disposed = true;
    }
}
