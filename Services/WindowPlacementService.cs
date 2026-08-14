using System.Drawing;
using System.Windows.Forms;

namespace NetworkDiagram
{
    internal static class WindowPlacementService
    {
        private const int TitleBarHeight = 56;
        private const int MinimumVisibleWidth = 140;

        public static void RestoreWindowPosition(Form form)
        {
            if (form == null) {
                return;
            }

            int storedX = Properties.Settings.Default.WindowPosX;
            int storedY = Properties.Settings.Default.WindowPosY;
            if (storedX < 0 || storedY < 0) {
                return;
            }

            Point location = EnsureVisible(new Point(storedX, storedY), form.Size);
            form.StartPosition = FormStartPosition.Manual;
            form.Left = location.X;
            form.Top = location.Y;
        }

        public static void SaveWindowPosition(Form form)
        {
            if (form == null || form.WindowState == FormWindowState.Minimized) {
                return;
            }

            Properties.Settings.Default.WindowPosX = form.Left;
            Properties.Settings.Default.WindowPosY = form.Top;
            Properties.Settings.Default.Save();
        }

        public static Point EnsureVisible(Point location, Size size)
        {
            Rectangle titleBarBounds = new Rectangle(
                location.X,
                location.Y,
                size.Width < MinimumVisibleWidth ? size.Width : MinimumVisibleWidth,
                TitleBarHeight);

            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(titleBarBounds)) {
                    return location;
                }
            }

            Rectangle workArea = Screen.PrimaryScreen.WorkingArea;
            int maxX = workArea.Right - MinimumVisibleWidth;
            if (maxX < workArea.Left) {
                maxX = workArea.Left;
            }

            int maxY = workArea.Bottom - TitleBarHeight;
            if (maxY < workArea.Top) {
                maxY = workArea.Top;
            }

            int x = location.X;
            if (x < workArea.Left) {
                x = workArea.Left;
            }
            if (x > maxX) {
                x = maxX;
            }

            int y = location.Y;
            if (y < workArea.Top) {
                y = workArea.Top;
            }
            if (y > maxY) {
                y = maxY;
            }

            return new Point(x, y);
        }
    }
}
