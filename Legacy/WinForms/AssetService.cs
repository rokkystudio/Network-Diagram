using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NetworkDiagram
{
    internal static class AssetService
    {
        private static readonly Dictionary<string, Image> Cache = new Dictionary<string, Image>();
        private static readonly Dictionary<string, Image> ScaledCache = new Dictionary<string, Image>();

        public static Image GetImage(string relativePath)
        {
            string fullPath = ResolvePath(relativePath);
            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath)) {
                return null;
            }

            Image image;
            if (Cache.TryGetValue(fullPath, out image)) {
                return image;
            }

            using (Image fileImage = Image.FromFile(fullPath))
            {
                image = new Bitmap(fileImage);
            }

            Cache[fullPath] = image;
            return image;
        }

        public static Image GetScaledImage(string relativePath, int width, int height)
        {
            string fullPath = ResolvePath(relativePath);
            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath)) {
                return null;
            }

            string cacheKey = fullPath + "|" + width + "x" + height;
            Image image;
            if (ScaledCache.TryGetValue(cacheKey, out image)) {
                return image;
            }

            Image source = GetImage(relativePath);
            if (source == null) {
                return null;
            }

            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(source, new Rectangle(0, 0, width, height));
            }

            ScaledCache[cacheKey] = bitmap;
            return bitmap;
        }

        private static string ResolvePath(string relativePath)
        {
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(baseDirectory);

            while (directory != null) {
                string candidate = Path.Combine(directory.FullName, relativePath);
                if (File.Exists(candidate)) {
                    return candidate;
                }

                directory = directory.Parent;
            }

            return null;
        }
    }
}
