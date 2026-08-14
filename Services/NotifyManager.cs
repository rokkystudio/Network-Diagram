using System;
using System.Drawing;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetworkDiagram
{
    // Менеджер значка в трее: отображает иконки сети в зависимости от активности.
    public class NotifyManager
    {
        public const int DefaultTrayArrowsStyle = 1;
        public const int MinTrayArrowsStyle = 1;
        public const int MaxTrayArrowsStyle = 5;
        public const int PreviewTrayArrowsState = 4;

        // Удаляет дескриптор иконки из user32.dll (GDI очистка)
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        private ResourceManager mResourceManager;
        private NotifyIcon mNotifyIcon;

        // Конструктор. Передаётся NotifyIcon, с которым будем работать.
        public NotifyManager(NotifyIcon icon) {
            mResourceManager = Properties.Resources.ResourceManager;
            mNotifyIcon = icon;
        }

        // Отображает иконку в трее в зависимости от стиля и активности.
        public void DrawIcon(int style, int sent, int received)
        {
            style = NormalizeTrayArrowsStyle(style);
            DrawArrowsIcon(style, sent != 0, received != 0);
        }

        public static int NormalizeTrayArrowsStyle(int style)
        {
            if (style < MinTrayArrowsStyle || style > MaxTrayArrowsStyle) {
                return DefaultTrayArrowsStyle;
            }

            return style;
        }

        // Отображает стрелки при активности (отправка/приём).
        private void DrawArrowsIcon(int style, bool sent, bool received)
        {
            if (mNotifyIcon == null) return;

            int state = GetState(sent, received);
            string name = CreateTrayArrowsResourceName(style, state);

            Bitmap bitmap = (Bitmap) mResourceManager.GetObject(name);
            if (bitmap == null) {
                System.Diagnostics.Debug.WriteLine($"[DrawArrowsIcon] Resource '{name}' not found.");
                return;
            }

            DrawBitmap(bitmap);
        }

        private static string CreateTrayArrowsResourceName(int style, int state)
        {
            return string.Format("TrayArrowsStyle{0}State{1}", style, state);
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
        private int GetState(bool sent, bool received)
        {
            if (!sent && !received) return 1;
            if (!sent && received) return 2;
            if (sent && !received) return 3;
            return 4;
        }
    }
}
