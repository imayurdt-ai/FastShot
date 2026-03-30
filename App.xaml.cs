using System.Windows;

namespace FastShot;

public partial class App : Application
{
    private TrayIconHost? _tray;

    private void OnStartup(object sender, StartupEventArgs e)
    {
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        _tray = new TrayIconHost();
        _tray.Initialize();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        _tray?.Dispose();
    }
}
