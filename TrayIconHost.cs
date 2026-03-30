using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FastShot.Services;
using FastShot.Views;

namespace FastShot;

public sealed class TrayIconHost : IDisposable
{
    readonly ConfigService _cfg = new();
    NotifyIcon? _icon;
    HotkeyService? _hotkey;
    HiddenWindow?  _hidden;

    public void Initialize()
    {
        BuildTrayIcon();
        RegisterHotkey();
    }

    void BuildTrayIcon()
    {
        _icon = new NotifyIcon
        {
            Text    = "FastShot",
            Visible = true,
            Icon    = SystemIcons.Application
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add("📸  Take Screenshot", null, (_, _) => CaptureAsync());
        menu.Items.Add("-");
        menu.Items.Add("⚙️  Settings",  null, (_, _) => OpenSettings());
        menu.Items.Add("❌  Exit",       null, (_, _) => ExitApp());
        _icon.ContextMenuStrip = menu;
        _icon.DoubleClick += (_, _) => CaptureAsync();
    }

    void RegisterHotkey()
    {
        _hotkey?.Dispose();
        _hidden ??= new HiddenWindow();
        _hidden.Show();
        var hwnd = new System.Windows.Interop.WindowInteropHelper(_hidden).EnsureHandle();
        try
        {
            var (mods, vk) = HotkeyParser.Parse(_cfg.Config.Hotkey);
            _hotkey = new HotkeyService(hwnd, mods, vk);
            _hotkey.Fired += () => CaptureAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "FastShot – Hotkey Error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    public async void CaptureAsync()
    {
        try
        {
            var bmp = await Task.Run(ScreenshotService.CaptureAll);
            ClipboardService.Copy(bmp);
            string? saved = null;
            if (_cfg.Config.SaveToDisk)
                saved = await Task.Run(() => FileStorageService.Save(bmp, _cfg.Config.SavePath));
            bmp.Dispose();
            if (_cfg.Config.ShowToast && _icon != null)
            {
                var msg = saved != null ? $"Saved to {saved}" : "Copied to clipboard";
                ToastService.Show(_icon, "Screenshot taken!", msg);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Screenshot failed:\n{ex.Message}", "FastShot",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    void OpenSettings()
    {
        var win = new SettingsWindow(_cfg);
        win.ShowDialog();
        RegisterHotkey();
    }

    void ExitApp()
    {
        Dispose();
        System.Windows.Application.Current.Shutdown();
    }

    public void Dispose()
    {
        _hotkey?.Dispose();
        if (_icon != null) { _icon.Visible = false; _icon.Dispose(); }
    }
}
