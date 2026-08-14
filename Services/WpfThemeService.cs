using System.Windows;
using System.Windows.Media;
using NetworkDiagram.Properties;

namespace NetworkDiagram
{
    internal static class WpfThemeService
    {
        public static void Apply(Application application)
        {
            if (application == null) {
                return;
            }

            ThemePalette palette = ThemeService.CurrentPalette;
            ResourceDictionary resources = application.Resources;
            byte alpha = GetSurfaceAlpha(Settings.Default.Opacity);
            resources["BackgroundBrush"] = ToBrush(palette.BackgroundColor, alpha);
            resources["SurfaceBrush"] = ToBrush(palette.SurfaceColor, alpha);
            resources["RaisedSurfaceBrush"] = ToBrush(palette.SurfaceRaisedColor, alpha);
            resources["TextBrush"] = ToBrush(palette.TextColor);
            resources["MutedTextBrush"] = ToBrush(palette.MutedTextColor);
            resources["BorderBrush"] = ToBrush(palette.BorderColor, alpha);
            resources["PrimaryBrush"] = ToBrush(palette.PrimaryColor);
            resources["DangerBrush"] = ToBrush(palette.ErrorColor);
            resources["TitleBarBackgroundBrush"] = ToBrush(palette.TitleBarBackgroundColor, alpha);
            resources["TitleBarBorderBrush"] = ToBrush(palette.TitleBarBorderColor, alpha);
            resources["ButtonHoverBackgroundBrush"] = ToBrush(Blend(palette.PrimaryColor, palette.SurfaceRaisedColor, 0.14F), alpha);
            resources["ButtonPressedBackgroundBrush"] = ToBrush(Blend(palette.PrimaryColor, palette.SurfaceRaisedColor, 0.24F), alpha);
        }

        public static Color ToWpfColor(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static SolidColorBrush ToBrush(System.Drawing.Color color)
        {
            return ToBrush(color, color.A);
        }

        public static SolidColorBrush ToBrush(System.Drawing.Color color, byte alpha)
        {
            SolidColorBrush brush = new SolidColorBrush(ToWpfColor(color));
            brush.Color = Color.FromArgb(alpha, brush.Color.R, brush.Color.G, brush.Color.B);
            brush.Freeze();
            return brush;
        }

        private static byte GetSurfaceAlpha(float opacity)
        {
            if (opacity <= 0F) {
                opacity = 1F;
            }

            opacity = System.Math.Max(0.2F, System.Math.Min(1F, opacity));
            return (byte)System.Math.Round(255 * opacity);
        }

        private static System.Drawing.Color Blend(System.Drawing.Color overlay, System.Drawing.Color background, float amount)
        {
            amount = System.Math.Max(0F, System.Math.Min(1F, amount));
            int red = (int)(background.R + ((overlay.R - background.R) * amount));
            int green = (int)(background.G + ((overlay.G - background.G) * amount));
            int blue = (int)(background.B + ((overlay.B - background.B) * amount));
            return System.Drawing.Color.FromArgb(red, green, blue);
        }
    }
}
