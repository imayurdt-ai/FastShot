using System.Windows;

namespace FastShot;

public class HiddenWindow : Window
{
    public HiddenWindow()
    {
        WindowStyle   = WindowStyle.None;
        ShowInTaskbar = false;
        Width = Height = 1;
        Left  = Top = -9999;
        Opacity = 0;
    }
}
