using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;

namespace NetworkDiagram
{
    internal static class NetworkInterfaceIconService
    {
        private static readonly Dictionary<string, Image> Cache = new Dictionary<string, Image>();

        public static Image GetIcon(NetworkAdapter adapter, Size size, Color strokeColor, Color accentColor)
        {
            NetworkInterfaceType interfaceType = adapter == null
                ? NetworkInterfaceType.Unknown
                : adapter.Interface.NetworkInterfaceType;

            string cacheKey = interfaceType + "|" + size.Width + "x" + size.Height + "|" + strokeColor.ToArgb() + "|" + accentColor.ToArgb();
            Image image;
            if (Cache.TryGetValue(cacheKey, out image)) {
                return image;
            }

            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (Pen strokePen = new Pen(strokeColor, 1.6f))
            using (Pen accentPen = new Pen(accentColor, 1.6f))
            using (SolidBrush accentBrush = new SolidBrush(accentColor))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);

                strokePen.StartCap = LineCap.Round;
                strokePen.EndCap = LineCap.Round;
                accentPen.StartCap = LineCap.Round;
                accentPen.EndCap = LineCap.Round;

                switch (interfaceType)
                {
                    case NetworkInterfaceType.Wireless80211:
                        DrawWireless(graphics, strokePen, accentBrush, size);
                        break;
                    case NetworkInterfaceType.Loopback:
                    case NetworkInterfaceType.Tunnel:
                    case NetworkInterfaceType.Ppp:
                        DrawLoop(graphics, strokePen, accentPen, accentBrush, size);
                        break;
                    default:
                        DrawEthernet(graphics, strokePen, accentBrush, size);
                        break;
                }
            }

            Cache[cacheKey] = bitmap;
            return bitmap;
        }

        private static void DrawWireless(Graphics graphics, Pen strokePen, SolidBrush accentBrush, Size size)
        {
            float centerX = size.Width / 2F;
            float bottom = size.Height - 3F;

            graphics.FillEllipse(accentBrush, centerX - 1.8F, bottom - 1.8F, 3.6F, 3.6F);
            graphics.DrawArc(strokePen, centerX - 4F, bottom - 6F, 8F, 8F, 215F, 110F);
            graphics.DrawArc(strokePen, centerX - 6.5F, bottom - 9F, 13F, 13F, 215F, 110F);
            graphics.DrawArc(strokePen, centerX - 9F, bottom - 12F, 18F, 18F, 215F, 110F);
        }

        private static void DrawEthernet(Graphics graphics, Pen strokePen, SolidBrush accentBrush, Size size)
        {
            RectangleF bodyBounds = new RectangleF(2.5F, 3F, size.Width - 5F, size.Height - 6F);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bodyBounds.X, bodyBounds.Y, 4F, 4F, 180F, 90F);
            path.AddArc(bodyBounds.Right - 4F, bodyBounds.Y, 4F, 4F, 270F, 90F);
            path.AddArc(bodyBounds.Right - 4F, bodyBounds.Bottom - 4F, 4F, 4F, 0F, 90F);
            path.AddArc(bodyBounds.X, bodyBounds.Bottom - 4F, 4F, 4F, 90F, 90F);
            path.CloseFigure();
            graphics.DrawPath(strokePen, path);
            path.Dispose();

            float portTop = bodyBounds.Y + 4F;
            float portLeft = bodyBounds.X + 4.5F;
            float portWidth = bodyBounds.Width - 9F;
            graphics.DrawLine(strokePen, portLeft, portTop, portLeft + portWidth, portTop);

            for (int index = 0; index < 3; index++)
            {
                float x = portLeft + 2F + (index * 3.4F);
                graphics.DrawLine(strokePen, x, portTop, x, portTop + 3.5F);
            }

            graphics.FillRectangle(accentBrush, bodyBounds.X + 4F, bodyBounds.Bottom - 4F, bodyBounds.Width - 8F, 2.4F);
        }

        private static void DrawLoop(Graphics graphics, Pen strokePen, Pen accentPen, SolidBrush accentBrush, Size size)
        {
            graphics.DrawArc(strokePen, 2.5F, 2.5F, size.Width - 5F, size.Height - 5F, 35F, 250F);
            graphics.DrawLine(accentPen, size.Width - 5F, 6F, size.Width - 2.5F, 6F);
            graphics.DrawLine(accentPen, size.Width - 5F, 6F, size.Width - 5F, 3.5F);
            graphics.FillEllipse(accentBrush, 2F, size.Height - 6F, 3.5F, 3.5F);
        }
    }
}
