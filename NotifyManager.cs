using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public class NotifyManager
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        public const int STATIC_ICON   = 0;
        public const int DARK_ARROWS   = 1;
        public const int BIG_ARROWS    = 2;
        public const int SMALL_ARROWS  = 3;
        public const int BLUE_APP      = 4;
        public const int GREEN_APP     = 5;
        public const int PROGRESS_ICON = 6;

        private ResourceManager mResourceManager;
        private NotifyIcon mNotifyIcon;
        private bool mStaticMode = true;

        public NotifyManager(NotifyIcon icon) {
            mResourceManager = new ResourceManager(typeof(Properties.Resources));
            mNotifyIcon = icon;
        }

        public void DrawIcon(int style, int sent, int received)
        {
            if (style < STATIC_ICON && style > PROGRESS_ICON) style = DARK_ARROWS;

            if (style == STATIC_ICON)
            {
                if (!mStaticMode) {
                    DrawStaticIcon();
                    mStaticMode = true;
                }
                return;
            }
            mStaticMode = false;

            if (style == PROGRESS_ICON) {
                DrawProgressIcon(sent, received);
                return;
            }

            DrawArrowsIcon(style, sent != 0, received != 0);
        }

        private void DrawStaticIcon()
        {
            if (mNotifyIcon == null) return;
            Bitmap bitmap = (Bitmap) mResourceManager.GetObject("notify4_1");
            DrawBitmap(bitmap);
        }

        private void DrawArrowsIcon(int style, bool sent, bool received)
        {
            if (mNotifyIcon == null) return;

            int id = GetID(sent, received);
            String name = "notify" + style + "_" + id;
            Bitmap bitmap = (Bitmap) mResourceManager.GetObject(name);
            if (bitmap == null) return;

            DrawBitmap(bitmap);
        }

        private void DrawProgressIcon(int sent, int received)
        {
            if (mNotifyIcon == null) return;

            var resources = new ResourceManager(typeof(Properties.Resources));
            Bitmap bitmap = (Bitmap)resources.GetObject("notify1_1");
            if (bitmap == null) return;

            Bitmap bfull = (Bitmap)resources.GetObject("notify1_4");
            if (bfull == null) return;

            Graphics graphics = Graphics.FromImage(bitmap);

            // Draw sent arrow
            int height = (int)(16 * sent);
            if (height > 0)
            {
                Rectangle rect = new Rectangle(0, 0, 8, height);
                PixelFormat format = bfull.PixelFormat;
                Bitmap clone = bfull.Clone(rect, format);
                graphics.DrawImage(clone, 0, 0);
                clone.Dispose();
            }

            // Draw received arrow
            height = (int)(16 * received);
            if (height > 0)
            {
                Rectangle rect = new Rectangle(8, 0, 8, height);
                PixelFormat format = bfull.PixelFormat;
                Bitmap clone = bfull.Clone(rect, format);
                graphics.DrawImage(clone, 8, 0);
                clone.Dispose();
            }

            graphics.Dispose();
            bfull.Dispose();

            DrawBitmap(bitmap);
        }

        private void DrawBitmap(Bitmap bitmap)
        {
            if (mNotifyIcon == null) return;
            DestroyIcon(mNotifyIcon.Icon.Handle);

            Icon icon = Icon.FromHandle(bitmap.GetHicon());
            if (icon != null) mNotifyIcon.Icon = icon;
        }

        private int GetID(bool sent, bool received) {
            if (!sent && !received) return 1;
            if (!sent && received)  return 2;
            if (sent && !received)  return 3;
            return 4; 
        }
    }
}
