using System;
using System.IO;
using System.Windows;
using FastShot.Services;
using WinForms = System.Windows.Forms;

namespace FastShot.Views;

public partial class SettingsWindow : Window
{
    readonly ConfigService _cfg;

    public SettingsWindow(ConfigService cfg)
    {
        InitializeComponent();
        _cfg = cfg;
        LoadUI();
    }

    void LoadUI()
    {
        HotkeyBox.Text          = _cfg.Config.Hotkey;
        FolderBox.Text          = _cfg.Config.SavePath;
        SaveToDiskChk.IsChecked = _cfg.Config.SaveToDisk;
        ShowToastChk.IsChecked  = _cfg.Config.ShowToast;
    }

    void Browse_Click(object s, RoutedEventArgs e)
    {
        using var dlg = new WinForms.FolderBrowserDialog
        {
            SelectedPath = FolderBox.Text,
            Description  = "Select screenshot save folder"
        };
        if (dlg.ShowDialog() == WinForms.DialogResult.OK)
            FolderBox.Text = dlg.SelectedPath;
    }

    void Save_Click(object s, RoutedEventArgs e)
    {
        var hotkey = HotkeyBox.Text.Trim();
        var folder = FolderBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(hotkey))
        { MessageBox.Show("Hotkey cannot be empty."); return; }

        if (string.IsNullOrWhiteSpace(folder))
        { MessageBox.Show("Save folder cannot be empty."); return; }

        try   { Directory.CreateDirectory(folder); }
        catch (Exception ex)
        { MessageBox.Show($"Cannot create folder:\n{ex.Message}"); return; }

        _cfg.Config.Hotkey      = hotkey;
        _cfg.Config.SavePath    = folder;
        _cfg.Config.SaveToDisk  = SaveToDiskChk.IsChecked == true;
        _cfg.Config.ShowToast   = ShowToastChk.IsChecked  == true;
        _cfg.Save();

        DialogResult = true;
        Close();
    }

    void Cancel_Click(object s, RoutedEventArgs e) => Close();
}
