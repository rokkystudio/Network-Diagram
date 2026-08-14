namespace NetworkDiagram
{
    internal static class AssetPaths
    {
        public const string AppIcon = @"Assets\App\netspeed.ico";
        public const string ThemeMoon = @"Assets\Icons\ThemeMoon.png";
        public const string ThemeSun = @"Assets\Icons\ThemeSun.png";
        public const string FlagRussian = @"Assets\Flags\RU.png";
        public const string FlagEnglish = @"Assets\Flags\US.png";

        public static string TrayArrowsPreview(int style)
        {
            return string.Format(
                @"Assets\TrayIcons\tray-arrows-style-{0}-state-{1}.png",
                style,
                NotifyManager.PreviewTrayArrowsState);
        }
    }
}
