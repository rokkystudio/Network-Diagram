using NetworkDiagram.Properties;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkDiagram
{
    public partial class SettingsWindow : Window
    {
        private bool mInitializing = true;
        public SettingsWindow()
        {
            InitializeComponent();
            WpfThemeService.Apply(System.Windows.Application.Current);
            Icon = LoadIconSource();
            AppIcon.Source = Icon;

            ThemeService.ThemeChanged += ThemeService_ThemeChanged;
            LocalizationService.LanguageChanged += LocalizationService_LanguageChanged;
            Settings.Default.PropertyChanged += Settings_PropertyChanged;

            LoadSettings();
            ApplyLocalization();
            ApplyTheme();
            ApplyWindowSettings();
        }

        private void LoadSettings()
        {
            mInitializing = true;

            OpacitySlider.Value = Math.Round(Settings.Default.Opacity * 100);
            RunOnStartupCheckBox.IsChecked = Settings.Default.RunOnStartup;
            AlwaysOnTopCheckBox.IsChecked = Settings.Default.AlwaysOnTop;
            NormalizeStoredTrayIconStyle();
            NormalizeStoredPaletteColors();
            UpdateTrayIconStyleSelector();
            UpdateColorSelectors();
            UpdateOpacityText();

            mInitializing = false;
        }

        private void ApplyLocalization()
        {
            Title = LocalizationService.Text("settings");
            TitleText.Text = LocalizationService.Text("app_name");
            SubtitleText.Text = LocalizationService.Text("settings_subtitle");
            OpacityLabel.Text = LocalizationService.Text("opacity");
            RunOnStartupCheckBox.Content = LocalizationService.Text("run_on_startup");
            AlwaysOnTopCheckBox.Content = LocalizationService.Text("always_on_top");
            TrayIconStyleLabel.Text = LocalizationService.Text("tray_icon_style");
            SentColorLabel.Text = LocalizationService.Text("sent_color");
            ReceivedColorLabel.Text = LocalizationService.Text("received_color");
            UpdateHeaderButtons();
            UpdateTrayIconStyleSelector();
            UpdateColorSelectors();
        }

        private void ApplyTheme()
        {
            WpfThemeService.Apply(System.Windows.Application.Current);
            UpdateHeaderButtons();
            UpdateTrayIconStyleSelector();
            UpdateColorSelectors();
        }

        private void ApplyWindowSettings()
        {
            Opacity = 1D;
            WpfThemeService.Apply(System.Windows.Application.Current);
        }

        private void UpdateHeaderButtons()
        {
            if (LanguageButton == null || ThemeButton == null) {
                return;
            }

            LanguageButton.ToolTip = LocalizationService.Text("tooltip_language");
            ThemeButton.ToolTip = LocalizationService.Text("tooltip_theme");

            LanguageButtonImage.Source = WpfAssetService.LoadImage(WpfLanguageMenuService.GetCurrentLanguageFlagPath());

            string themePath = ThemeService.IsDarkTheme(ThemeService.CurrentTheme)
                ? AssetPaths.ThemeSun
                : AssetPaths.ThemeMoon;
            ThemeButtonImage.Source = WpfAssetService.LoadImage(themePath);
        }

        private void OpacitySlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mInitializing) {
                return;
            }

            Settings.Default.Opacity = (float)(OpacitySlider.Value / 100D);
            Settings.Default.Save();
            ApplyWindowSettings();
            UpdateOpacityText();
        }

        private void RunOnStartupCheckBox_OnChanged(object sender, RoutedEventArgs e)
        {
            if (mInitializing) {
                return;
            }

            Settings.Default.RunOnStartup = RunOnStartupCheckBox.IsChecked == true;
            Settings.Default.Save();
        }

        private void AlwaysOnTopCheckBox_OnChanged(object sender, RoutedEventArgs e)
        {
            if (mInitializing) {
                return;
            }

            Settings.Default.AlwaysOnTop = AlwaysOnTopCheckBox.IsChecked == true;
            Settings.Default.Save();
        }

        private void UpdateOpacityText()
        {
            if (OpacityValueText == null || OpacitySlider == null) {
                return;
            }

            OpacityValueText.Text = string.Format("{0:0}%", OpacitySlider.Value);
        }

        private void NormalizeStoredPaletteColors()
        {
            string sentColor = TrafficColorPalette.Normalize(Settings.Default.SentColor, 15);
            string receivedColor = TrafficColorPalette.Normalize(Settings.Default.ReceivedColor, 8);
            bool changed = false;

            if (!string.Equals(Settings.Default.SentColor, sentColor, StringComparison.OrdinalIgnoreCase)) {
                Settings.Default.SentColor = sentColor;
                changed = true;
            }

            if (!string.Equals(Settings.Default.ReceivedColor, receivedColor, StringComparison.OrdinalIgnoreCase)) {
                Settings.Default.ReceivedColor = receivedColor;
                changed = true;
            }

            if (changed) {
                Settings.Default.Save();
            }
        }

        private void NormalizeStoredTrayIconStyle()
        {
            int style = NotifyManager.NormalizeTrayArrowsStyle(Settings.Default.TrayIconStyle);
            if (Settings.Default.TrayIconStyle == style) {
                return;
            }

            Settings.Default.TrayIconStyle = style;
            Settings.Default.Save();
        }

        private void UpdateTrayIconStyleSelector()
        {
            if (TrayIconStylePreviewImage == null || TrayIconStyleValueText == null) {
                return;
            }

            int style = NotifyManager.NormalizeTrayArrowsStyle(Settings.Default.TrayIconStyle);
            TrayIconStylePreviewImage.Source = LoadTrayIconPreview(style);
            TrayIconStyleValueText.Text = FormatTrayIconStyleName(style);
            TrayIconStyleButton.ToolTip = FormatTrayIconStyleName(style);
        }

        private void UpdateColorSelectors()
        {
            UpdateColorSelector(true);
            UpdateColorSelector(false);
        }

        private void UpdateColorSelector(bool sent)
        {
            TrafficColorOption option = GetSelectedTrafficColor(sent);
            Border preview = sent ? SentColorPreview : ReceivedColorPreview;
            TextBlock text = sent ? SentColorValueText : ReceivedColorValueText;
            Button button = sent ? SentColorButton : ReceivedColorButton;

            if (preview == null || text == null || button == null) {
                return;
            }

            preview.Background = option.RichBrush;
            text.Text = option.Name;
            button.ToolTip = option.Name;
        }

        private TrafficColorOption GetSelectedTrafficColor(bool sent)
        {
            string value = sent
                ? TrafficColorPalette.Normalize(Settings.Default.SentColor, 15)
                : TrafficColorPalette.Normalize(Settings.Default.ReceivedColor, 8);
            return TrafficColorPalette.FindOrDefault(value, sent ? 15 : 8);
        }

        private ContextMenu CreateTrayIconStyleMenu()
        {
            ContextMenu menu = new ContextMenu();
            int selectedStyle = NotifyManager.NormalizeTrayArrowsStyle(Settings.Default.TrayIconStyle);

            for (int style = NotifyManager.MinTrayArrowsStyle; style <= NotifyManager.MaxTrayArrowsStyle; style++) {
                MenuItem item = new MenuItem
                {
                    Header = CreateTrayIconStyleHeader(style),
                    IsCheckable = true,
                    IsChecked = style == selectedStyle,
                    Tag = style
                };
                item.Click += delegate
                {
                    Settings.Default.TrayIconStyle = (int)item.Tag;
                    Settings.Default.Save();
                    UpdateTrayIconStyleSelector();
                };
                menu.Items.Add(item);
            }

            return menu;
        }

        private FrameworkElement CreateTrayIconStyleHeader(int style)
        {
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            panel.Children.Add(new Image
            {
                Width = 16,
                Height = 16,
                Margin = new Thickness(0, 0, 8, 0),
                Source = LoadTrayIconPreview(style),
                Stretch = Stretch.Fill,
                VerticalAlignment = VerticalAlignment.Center
            });
            RenderOptions.SetBitmapScalingMode(panel.Children[0], BitmapScalingMode.NearestNeighbor);

            panel.Children.Add(new TextBlock
            {
                Text = FormatTrayIconStyleName(style),
                VerticalAlignment = VerticalAlignment.Center
            });

            return panel;
        }

        private ContextMenu CreateColorMenu(bool sent)
        {
            ContextMenu menu = new ContextMenu();
            TrafficColorOption selected = GetSelectedTrafficColor(sent);

            foreach (TrafficColorOption option in TrafficColorPalette.Options) {
                MenuItem item = new MenuItem
                {
                    Header = CreateColorHeader(option),
                    IsCheckable = true,
                    IsChecked = string.Equals(selected.Value, option.Value, StringComparison.OrdinalIgnoreCase),
                    Tag = option
                };
                item.Click += delegate
                {
                    TrafficColorOption selectedOption = (TrafficColorOption)item.Tag;
                    if (sent) {
                        Settings.Default.SentColor = selectedOption.Value;
                    } else {
                        Settings.Default.ReceivedColor = selectedOption.Value;
                    }

                    Settings.Default.Save();
                    UpdateColorSelectors();
                };
                menu.Items.Add(item);
            }

            return menu;
        }

        private static FrameworkElement CreateColorHeader(TrafficColorOption option)
        {
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            panel.Children.Add(new Border
            {
                Width = 16,
                Height = 16,
                Margin = new Thickness(0, 0, 8, 0),
                Background = option.RichBrush,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                VerticalAlignment = VerticalAlignment.Center
            });

            panel.Children.Add(new TextBlock
            {
                Text = option.Name,
                VerticalAlignment = VerticalAlignment.Center
            });

            return panel;
        }

        private static ImageSource LoadTrayIconPreview(int style)
        {
            return WpfAssetService.LoadImage(AssetPaths.TrayArrowsPreview(style), 16, 16);
        }

        private static string FormatTrayIconStyleName(int style)
        {
            return string.Format(LocalizationService.Text("tray_icon_style_name"), style);
        }

        private void ThemeService_ThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity") {
                ApplyWindowSettings();
                if (OpacitySlider != null) {
                    mInitializing = true;
                    OpacitySlider.Value = Math.Round(Settings.Default.Opacity * 100);
                    mInitializing = false;
                }

                UpdateOpacityText();
            }

            if (e.PropertyName == "SentColor" || e.PropertyName == "ReceivedColor") {
                UpdateColorSelectors();
            }

            if (e.PropertyName == "TrayIconStyle") {
                NormalizeStoredTrayIconStyle();
                UpdateTrayIconStyleSelector();
            }
        }

        private void LocalizationService_LanguageChanged(object sender, EventArgs e)
        {
            ApplyLocalization();
        }

        private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                return;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed && WindowState == WindowState.Normal) {
                DragMove();
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TrayIconStyleButton_OnClick(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = CreateTrayIconStyleMenu();
            menu.PlacementTarget = TrayIconStyleButton;
            menu.IsOpen = true;
        }

        private void SentColorButton_OnClick(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = CreateColorMenu(true);
            menu.PlacementTarget = SentColorButton;
            menu.IsOpen = true;
        }

        private void ReceivedColorButton_OnClick(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = CreateColorMenu(false);
            menu.PlacementTarget = ReceivedColorButton;
            menu.IsOpen = true;
        }

        private void LanguageButton_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ContextMenu menu = WpfLanguageMenuService.CreateLanguageMenu();
            menu.PlacementTarget = LanguageButton;
            menu.IsOpen = true;
        }

        private void ThemeButton_OnClick(object sender, RoutedEventArgs e)
        {
            ThemeService.ToggleTheme();
        }

        private void Window_OnClosed(object sender, EventArgs e)
        {
            ThemeService.ThemeChanged -= ThemeService_ThemeChanged;
            LocalizationService.LanguageChanged -= LocalizationService_LanguageChanged;
            Settings.Default.PropertyChanged -= Settings_PropertyChanged;
        }

        private static ImageSource LoadIconSource()
        {
            return Imaging.CreateBitmapSourceFromHIcon(
                Properties.Resources.ApplicationIcon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(32, 32));
        }
    }
}
