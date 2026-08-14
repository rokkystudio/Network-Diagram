using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkDiagram
{
    internal static class WpfLanguageMenuService
    {
        public static ContextMenu CreateLanguageMenu()
        {
            ContextMenu menu = new ContextMenu();
            menu.Items.Add(CreateLanguageMenuItem(
                LocalizationService.AutomaticLanguage,
                LocalizationService.SystemLanguageDisplayText(),
                GetLanguageFlagPath(LocalizationService.DetectedLanguage)));
            menu.Items.Add(new Separator());
            menu.Items.Add(CreateLanguageMenuItem(LocalizationService.EnglishLanguage, "English", AssetPaths.FlagEnglish));
            menu.Items.Add(CreateLanguageMenuItem(LocalizationService.RussianLanguage, "Русский", AssetPaths.FlagRussian));
            return menu;
        }

        public static string GetCurrentLanguageFlagPath()
        {
            return GetLanguageFlagPath(LocalizationService.CurrentLanguage);
        }

        private static string GetLanguageFlagPath(string language)
        {
            return LocalizationService.IsRussianLanguage(language)
                ? AssetPaths.FlagRussian
                : AssetPaths.FlagEnglish;
        }

        private static MenuItem CreateLanguageMenuItem(string language, string text, string flagPath)
        {
            MenuItem item = new MenuItem
            {
                Header = CreateLanguageHeader(flagPath, text),
                IsCheckable = true,
                IsChecked = string.Equals(LocalizationService.SelectedLanguage, language, StringComparison.Ordinal),
                Tag = language
            };
            item.Click += delegate
            {
                LocalizationService.CurrentLanguage = (string)item.Tag;
            };
            return item;
        }

        private static FrameworkElement CreateLanguageHeader(string flagPath, string text)
        {
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            panel.Children.Add(new Image
            {
                Width = 20,
                Height = 14,
                Margin = new Thickness(0, 0, 8, 0),
                Source = WpfAssetService.LoadImage(flagPath),
                Stretch = Stretch.Uniform
            });
            RenderOptions.SetBitmapScalingMode(panel.Children[0], BitmapScalingMode.Fant);

            panel.Children.Add(new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center
            });

            return panel;
        }
    }
}
