using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace NetworkDiagram
{
    internal static class WpfAssetService
    {
        private static readonly Dictionary<string, BitmapImage> Cache = new Dictionary<string, BitmapImage>();

        public static BitmapImage LoadImage(string relativePath)
        {
            return LoadImage(relativePath, 0, 0);
        }

        public static BitmapImage LoadImage(string relativePath, int decodePixelWidth, int decodePixelHeight)
        {
            string fullPath = ResolvePath(relativePath);
            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath)) {
                return null;
            }

            string cacheKey = fullPath + "|" + decodePixelWidth + "x" + decodePixelHeight;
            BitmapImage image;
            if (Cache.TryGetValue(cacheKey, out image)) {
                return image;
            }

            image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            if (decodePixelWidth > 0) {
                image.DecodePixelWidth = decodePixelWidth;
            }

            if (decodePixelHeight > 0) {
                image.DecodePixelHeight = decodePixelHeight;
            }

            image.UriSource = new Uri(fullPath, UriKind.Absolute);
            image.EndInit();
            image.Freeze();

            Cache[cacheKey] = image;
            return image;
        }

        private static string ResolvePath(string relativePath)
        {
            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
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
