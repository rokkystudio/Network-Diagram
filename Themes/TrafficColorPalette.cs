using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace NetworkDiagram
{
    internal sealed class TrafficColorOption
    {
        public TrafficColorOption(string name, string value)
        {
            Name = name;
            Value = value;
            RichBrush = TrafficColorPalette.CreateBrush(value);
            PastelBrush = TrafficColorPalette.CreateBrush(TrafficColorPalette.ToPastelHex(value));
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
        public Brush RichBrush { get; private set; }
        public Brush PastelBrush { get; private set; }
    }

    internal static class TrafficColorPalette
    {
        public static readonly IReadOnlyList<TrafficColorOption> Options = new[]
        {
            new TrafficColorOption("Red", "#EF4444"),
            new TrafficColorOption("Orange", "#F97316"),
            new TrafficColorOption("Amber", "#F59E0B"),
            new TrafficColorOption("Yellow", "#EAB308"),
            new TrafficColorOption("Lime", "#84CC16"),
            new TrafficColorOption("Green", "#22C55E"),
            new TrafficColorOption("Emerald", "#10B981"),
            new TrafficColorOption("Teal", "#14B8A6"),
            new TrafficColorOption("Cyan", "#06B6D4"),
            new TrafficColorOption("Sky", "#0EA5E9"),
            new TrafficColorOption("Blue", "#3B82F6"),
            new TrafficColorOption("Indigo", "#6366F1"),
            new TrafficColorOption("Violet", "#8B5CF6"),
            new TrafficColorOption("Purple", "#A855F7"),
            new TrafficColorOption("Fuchsia", "#D946EF"),
            new TrafficColorOption("Rose", "#F43F5E")
        };

        public static TrafficColorOption FindOrDefault(string value, int fallbackIndex)
        {
            TrafficColorOption exact = Options.FirstOrDefault(option =>
                string.Equals(option.Value, value, StringComparison.OrdinalIgnoreCase));
            if (exact != null) {
                return exact;
            }

            if (fallbackIndex < 0 || fallbackIndex >= Options.Count) {
                fallbackIndex = 0;
            }

            return Options[fallbackIndex];
        }

        public static Brush CreateGraphBrush(string value)
        {
            TrafficColorOption option = FindNearestOrDefault(value, 10);
            return ThemeService.IsDarkTheme(ThemeService.CurrentTheme)
                ? option.RichBrush
                : option.PastelBrush;
        }

        public static Brush CreateTextBrush(string value)
        {
            TrafficColorOption option = FindNearestOrDefault(value, 10);
            return option.RichBrush;
        }

        public static string Normalize(string value, int fallbackIndex)
        {
            return FindOrDefault(value, fallbackIndex).Value;
        }

        public static string ToPastelHex(string value)
        {
            Color color = ParseColor(value, Colors.SkyBlue);
            byte red = Blend(color.R, 255, 0.68);
            byte green = Blend(color.G, 255, 0.68);
            byte blue = Blend(color.B, 255, 0.68);
            return string.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
        }

        public static SolidColorBrush CreateBrush(string value)
        {
            SolidColorBrush brush = new SolidColorBrush(ParseColor(value, Colors.SkyBlue));
            brush.Freeze();
            return brush;
        }

        private static TrafficColorOption FindNearestOrDefault(string value, int fallbackIndex)
        {
            TrafficColorOption option = Options.FirstOrDefault(item =>
                string.Equals(item.Value, value, StringComparison.OrdinalIgnoreCase));
            if (option != null) {
                return option;
            }

            Color color = ParseColor(value, Colors.SkyBlue);
            return Options
                .OrderBy(item => ColorDistance(color, ParseColor(item.Value, Colors.SkyBlue)))
                .FirstOrDefault() ?? FindOrDefault(null, fallbackIndex);
        }

        private static Color ParseColor(string value, Color fallback)
        {
            try {
                object parsed = ColorConverter.ConvertFromString(value);
                return parsed is Color ? (Color)parsed : fallback;
            }
            catch {
                return fallback;
            }
        }

        private static int ColorDistance(Color left, Color right)
        {
            int red = left.R - right.R;
            int green = left.G - right.G;
            int blue = left.B - right.B;
            return (red * red) + (green * green) + (blue * blue);
        }

        private static byte Blend(byte source, byte target, double amount)
        {
            return (byte)Math.Round(source + ((target - source) * amount));
        }
    }
}
