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
    // Менеджер значка в трее: отображает иконки сети в зависимости от активности.
    public class NotifyManager
    {
        // Удаляет дескриптор иконки из user32.dll (GDI очистка)
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        // Типы иконок
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

        // Конструктор. Передаётся NotifyIcon, с которым будем работать.
        public NotifyManager(NotifyIcon icon) {
            mResourceManager = Properties.Resources.ResourceManager;
            mNotifyIcon = icon;
        }

        // Отображает иконку в трее в зависимости от стиля и активности.
        public void DrawIcon(int style, int sent, int received)
        {
            if (style < STATIC_ICON || style > PROGRESS_ICON) {
                style = DARK_ARROWS;
            }

            if (style == STATIC_ICON) {
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

        // Отображает статичную иконку (например, при простое).
        private void DrawStaticIcon()
        {
            if (mNotifyIcon == null) return;

            Bitmap bitmap = (Bitmap) mResourceManager.GetObject("notify4_1");
            if (bitmap != null) DrawBitmap(bitmap);
        }

        // Отображает стрелки при активности (отправка/приём).
        private void DrawArrowsIcon(int style, bool sent, bool received)
        {
            if (mNotifyIcon == null) return;

            int id = GetID(sent, received);
            String name = "notify" + style + "_" + id;

            Bitmap bitmap = (Bitmap) mResourceManager.GetObject(name);
            if (bitmap == null) {
                System.Diagnostics.Debug.WriteLine($"[DrawArrowsIcon] Resource '{name}' not found.");
                return;
            }

            DrawBitmap(bitmap);
        }

        // Отображает иконку-прогресс (уровень активности как заливка).
        private void DrawProgressIcon(int sent, int received)
        {
            if (mNotifyIcon == null) return;

            Bitmap bitmap = (Bitmap)mResourceManager.GetObject("notify1_1");
            if (bitmap == null) return;

            Bitmap bfull = (Bitmap)mResourceManager.GetObject("notify1_4");
            if (bfull == null) {
                bitmap.Dispose();
                return;
            }

            try {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Clamp значения до 1.0f
                    float clampedSent = Math.Min(sent / 1f, 1f);
                    float clampedReceived = Math.Min(received / 1f, 1f);

                    // Draw sent arrow
                    int height = (int)(16 * clampedSent);
                    if (height > 0) {
                        Rectangle rect = new Rectangle(0, 0, 8, height);
                        using (Bitmap clone = bfull.Clone(rect, bfull.PixelFormat)) {
                            graphics.DrawImage(clone, 0, 0);
                        }
                    }

                    // Draw received arrow
                    height = (int)(16 * clampedReceived);
                    if (height > 0) {
                        Rectangle rect = new Rectangle(8, 0, 8, height);
                        using (Bitmap clone = bfull.Clone(rect, bfull.PixelFormat)) {
                            graphics.DrawImage(clone, 8, 0);
                        }
                    }
                }

                DrawBitmap(bitmap);
            }
            finally {
                bfull.Dispose();
                bitmap.Dispose();
            }
        }

        // Устанавливает bitmap как иконку в NotifyIcon.
        private void DrawBitmap(Bitmap bitmap)
        {
            if (mNotifyIcon == null || bitmap == null) return;

            // Получаем HICON из Bitmap
            IntPtr hIcon = bitmap.GetHicon();
            using (Icon tmpIcon = Icon.FromHandle(hIcon)) {
                // Клонируем, чтобы NotifyIcon владел копией и был безопасен от GC
                Icon clone = (Icon)tmpIcon.Clone();
                mNotifyIcon.Icon = clone;
            }

            // Уничтожаем оригинальный дескриптор (не влияет на клонированный)
            DestroyIcon(hIcon);
        }

        // Получает ID иконки по активности
        private int GetID(bool sent, bool received)
        {
            if (!sent && !received) return 1;
            if (!sent && received) return 2;
            if (sent && !received) return 3;
            return 4;
        }
    }
}
