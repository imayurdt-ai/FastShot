using System.Windows.Forms;

namespace FastShot.Services;

public static class ToastService
{
    public static void Show(NotifyIcon icon, string title, string text)
    {
        icon.BalloonTipTitle = title;
        icon.BalloonTipText  = text;
        icon.BalloonTipIcon  = ToolTipIcon.Info;
        icon.ShowBalloonTip(2000);
    }
}
