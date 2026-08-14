using Microsoft.Win32;
using NetworkDiagram.Properties;
using NetworkDiagram.ViewModels;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace NetworkDiagram
{
    public partial class MainWindow : Window
    {
        private bool mCloseApplication;
        private bool mApplicationCleanupComplete;
        private bool mCompactMode;
        private bool mGraphDragPending;
        private System.Windows.Point mGraphDragOrigin;
        private MainWindowViewModel mViewModel;
        private NotifyIcon mNotifyIcon;
        private NotifyManager mNotifyManager;
        private SettingsWindow mSettingsWindow;

        public MainWindow()
        {
            InitializeComponent();
            WpfThemeService.Apply(System.Windows.Application.Current);

            mViewModel = new MainWindowViewModel();
            DataContext = mViewModel;
            mViewModel.SpeedsUpdated += ViewModel_SpeedsUpdated;
            mViewModel.PropertyChanged += ViewModel_PropertyChanged;

            ConfigureWindow();
            ConfigureNotifyIcon();
            ApplyTheme();
            ApplyGraphColors();
            ApplyWindowSettings();

            Settings.Default.PropertyChanged += Settings_PropertyChanged;
            ThemeService.ThemeChanged += ThemeService_ThemeChanged;
            LocalizationService.LanguageChanged += LocalizationService_LanguageChanged;
        }

        public void RestoreWindowPlacementAndVisibility()
        {
            RestoreWindowPosition();

            if (Settings.Default.MainWindowVisible) {
                ShowDiagram();
            }
        }

        private void ConfigureWindow()
        {
            Icon = LoadIconSource();
            AppIcon.Source = Icon;
            StartupEnableClassic(Settings.Default.RunOnStartup);
        }

        private void ConfigureNotifyIcon()
        {
            mNotifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.ApplicationIcon,
                Text = LocalizationService.Text("notify_text"),
                Visible = true
            };

            mNotifyIcon.DoubleClick += NotifyIcon_OnDoubleClick;
            mNotifyIcon.ContextMenuStrip = CreateNotifyMenu();
            mNotifyManager = new NotifyManager(mNotifyIcon);
        }

        private ContextMenuStrip CreateNotifyMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(CreateMenuItem(LocalizationService.Text("open"), delegate { ShowDiagram(); }));
            menu.Items.Add(CreateMenuItem(LocalizationService.Text("compact_mode"), delegate { SetCompactMode(!mCompactMode); }));
            menu.Items.Add(CreateMenuItem(LocalizationService.Text("settings"), delegate { ShowSettings(); }));
            menu.Items.Add(CreateMenuItem(LocalizationService.Text("reset"), delegate { ResetWindow(); }));
            menu.Items.Add(CreateMenuItem(LocalizationService.Text("exit"), delegate { CloseApplication(); }));
            ThemeService.ApplyToolStrip(menu);
            return menu;
        }

        private static ToolStripMenuItem CreateMenuItem(string text, EventHandler click)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text);
            item.Click += click;
            return item;
        }

        private void ViewModel_SpeedsUpdated(int sentSpeed, int receivedSpeed)
        {
            TrafficGraph.AddValues(sentSpeed, receivedSpeed);

            int maxValue = TrafficGraph.MaxValue;
            int sentThroughput = maxValue <= 0 ? 0 : sentSpeed * 100 / maxValue;
            int receivedThroughput = maxValue <= 0 ? 0 : receivedSpeed * 100 / maxValue;
            if (mNotifyManager != null) {
                mNotifyManager.DrawIcon(Settings.Default.TrayIconStyle, sentThroughput, receivedThroughput);
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAdapter") {
                UpdateAdapterButtonIcon();
            }
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity" || e.PropertyName == "AlwaysOnTop") {
                ApplyWindowSettings();
            }

            if (e.PropertyName == "SentColor" || e.PropertyName == "ReceivedColor") {
                ApplyGraphColors();
            }

            if (e.PropertyName == "RunOnStartup") {
                StartupEnableClassic(Settings.Default.RunOnStartup);
            }

            if (e.PropertyName == "Language") {
                RebuildNotifyMenu();
                UpdateHeaderButtons();
            }

            if (e.PropertyName == "Theme") {
                ApplyTheme();
            }
        }

        private void ThemeService_ThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void LocalizationService_LanguageChanged(object sender, EventArgs e)
        {
            RebuildNotifyMenu();
            UpdateHeaderButtons();
        }

        private void ApplyTheme()
        {
            WpfThemeService.Apply(System.Windows.Application.Current);
            UpdateHeaderButtons();
            UpdateAdapterButtonIcon();
            ApplyGraphColors();
            RebuildNotifyMenu();
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

        private void RebuildNotifyMenu()
        {
            if (mNotifyIcon == null) {
                return;
            }

            if (mNotifyIcon.ContextMenuStrip != null) {
                mNotifyIcon.ContextMenuStrip.Dispose();
            }

            mNotifyIcon.Text = LocalizationService.Text("notify_text");
            mNotifyIcon.ContextMenuStrip = CreateNotifyMenu();
        }

        private void ApplyWindowSettings()
        {
            Topmost = Settings.Default.AlwaysOnTop;
            Opacity = 1D;
            WpfThemeService.Apply(System.Windows.Application.Current);
        }

        private void ApplyGraphColors()
        {
            string sentColor = TrafficColorPalette.Normalize(Settings.Default.SentColor, 15);
            string receivedColor = TrafficColorPalette.Normalize(Settings.Default.ReceivedColor, 8);

            System.Windows.Media.Brush sentGraphBrush = TrafficColorPalette.CreateGraphBrush(sentColor);
            System.Windows.Media.Brush receivedGraphBrush = TrafficColorPalette.CreateGraphBrush(receivedColor);
            System.Windows.Media.Brush sentTextBrush = TrafficColorPalette.CreateTextBrush(sentColor);
            System.Windows.Media.Brush receivedTextBrush = TrafficColorPalette.CreateTextBrush(receivedColor);

            TrafficGraph.SentBrush = sentGraphBrush;
            TrafficGraph.ReceivedBrush = receivedGraphBrush;

            UploadTotalValue.Foreground = sentTextBrush;
            UploadSpeedValue.Foreground = sentTextBrush;
            DownloadTotalValue.Foreground = receivedTextBrush;
            DownloadSpeedValue.Foreground = receivedTextBrush;
        }

        private void UpdateAdapterButtonIcon()
        {
            if (AdapterButtonImage == null || mViewModel == null) {
                return;
            }

            AdapterButtonImage.Source = WpfNetworkInterfaceIconService.GetIcon(mViewModel.SelectedAdapter, 16, 16);
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }

        private void AdapterButton_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ContextMenu menu = CreateAdapterMenu();
            menu.PlacementTarget = AdapterButton;
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

        private System.Windows.Controls.ContextMenu CreateAdapterMenu()
        {
            System.Windows.Controls.ContextMenu menu = new System.Windows.Controls.ContextMenu();
            if (mViewModel == null) {
                return menu;
            }

            foreach (NetworkAdapter adapter in mViewModel.Adapters) {
                System.Windows.Controls.MenuItem item = new System.Windows.Controls.MenuItem
                {
                    Header = CreateAdapterMenuHeader(adapter),
                    IsCheckable = true,
                    IsChecked = ReferenceEquals(adapter, mViewModel.SelectedAdapter),
                    Tag = adapter
                };
                item.Click += delegate
                {
                    mViewModel.SelectedAdapter = (NetworkAdapter)item.Tag;
                };
                menu.Items.Add(item);
            }

            return menu;
        }

        private static FrameworkElement CreateAdapterMenuHeader(NetworkAdapter adapter)
        {
            StackPanel panel = new StackPanel
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal
            };

            panel.Children.Add(new System.Windows.Controls.Image
            {
                Width = 16,
                Height = 16,
                Margin = new Thickness(0, 0, 8, 0),
                Source = WpfNetworkInterfaceIconService.GetIcon(adapter, 16, 16),
                Stretch = Stretch.Uniform
            });

            panel.Children.Add(new TextBlock
            {
                Text = adapter == null ? string.Empty : adapter.DisplayName,
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                MaxWidth = 320
            });

            return panel;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            HideToTray();
        }

        private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                return;
            }

            BeginDragMove();
        }

        private void TrafficGraph_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SetCompactMode(!mCompactMode);
        }

        private void TrafficGraph_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mGraphDragPending = true;
            mGraphDragOrigin = e.GetPosition(this);
            TrafficGraph.CaptureMouse();
        }

        private void TrafficGraph_OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!mGraphDragPending || e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            System.Windows.Point position = e.GetPosition(this);
            if (Math.Abs(position.X - mGraphDragOrigin.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(position.Y - mGraphDragOrigin.Y) < SystemParameters.MinimumVerticalDragDistance) {
                return;
            }

            mGraphDragPending = false;
            TrafficGraph.ReleaseMouseCapture();
            BeginDragMove();
        }

        private void TrafficGraph_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mGraphDragPending = false;
            TrafficGraph.ReleaseMouseCapture();
        }

        private void BeginDragMove()
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed || WindowState != WindowState.Normal) {
                return;
            }

            try {
                DragMove();
            }
            catch (InvalidOperationException) {
            }
        }

        private void SetCompactMode(bool enable)
        {
            mCompactMode = enable;
            TitleRow.Height = enable ? new GridLength(0) : GridLength.Auto;
            StatsRow.Height = enable ? new GridLength(0) : new GridLength(150);
            SpacerRow.Height = enable ? new GridLength(0) : new GridLength(8);
            TitleBar.Visibility = enable ? Visibility.Collapsed : Visibility.Visible;
            StatsPanel.Visibility = enable ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ShowSettings()
        {
            if (mSettingsWindow != null && mSettingsWindow.IsVisible) {
                mSettingsWindow.Activate();
                return;
            }

            mSettingsWindow = new SettingsWindow();
            mSettingsWindow.Owner = this;
            mSettingsWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            mSettingsWindow.Left = Left;
            mSettingsWindow.Top = Top;
            mSettingsWindow.Closed += SettingsWindow_OnClosed;

            Hide();
            mSettingsWindow.Show();
            mSettingsWindow.Activate();
        }

        private void SettingsWindow_OnClosed(object sender, EventArgs e)
        {
            if (mSettingsWindow != null) {
                Left = mSettingsWindow.Left;
                Top = mSettingsWindow.Top;
                mSettingsWindow.Closed -= SettingsWindow_OnClosed;
                mSettingsWindow = null;
            }

            if (!mCloseApplication) {
                ShowDiagram();
            }
        }

        private void ShowDiagram()
        {
            WindowState = WindowState.Normal;
            Show();
            Activate();
            SaveMainWindowVisibility(true);
        }

        private void ResetWindow()
        {
            Left = 0;
            Top = 0;
            Settings.Default.Opacity = 1;
            Settings.Default.Save();
            ShowDiagram();
        }

        private void NotifyIcon_OnDoubleClick(object sender, EventArgs e)
        {
            if (IsVisible && WindowState == WindowState.Normal) {
                HideToTray();
                return;
            }

            ShowDiagram();
        }

        private void Window_OnClosing(object sender, CancelEventArgs e)
        {
            if (!mCloseApplication) {
                e.Cancel = true;
                HideToTray();
            }
        }

        private void Window_OnClosed(object sender, EventArgs e)
        {
            CleanupApplicationResources();
        }

        private void Window_OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) {
                HideToTray();
            }
        }

        private void Window_OnLocationChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) {
                return;
            }

            Settings.Default.WindowPosX = (int)Left;
            Settings.Default.WindowPosY = (int)Top;
            Settings.Default.Save();
        }

        private void HideToTray()
        {
            Hide();
            SaveMainWindowVisibility(false);
        }

        private static void SaveMainWindowVisibility(bool visible)
        {
            if (Settings.Default.MainWindowVisible == visible) {
                return;
            }

            Settings.Default.MainWindowVisible = visible;
            Settings.Default.Save();
        }

        private void RestoreWindowPosition()
        {
            int x = Settings.Default.WindowPosX;
            int y = Settings.Default.WindowPosY;
            if (x < 0 || y < 0) {
                return;
            }

            System.Drawing.Point point = WindowPlacementService.EnsureVisible(
                new System.Drawing.Point(x, y),
                new System.Drawing.Size((int)Width, (int)Height));
            Left = point.X;
            Top = point.Y;
        }

        private static ImageSource LoadIconSource()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AssetPaths.AppIcon);
            if (!File.Exists(path)) {
                return Imaging.CreateBitmapSourceFromHIcon(
                    Properties.Resources.ApplicationIcon.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(32, 32));
            }

            return new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        private static void StartupEnableClassic(bool enabled)
        {
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true))
            {
                if (key == null) {
                    return;
                }

                if (enabled) {
                    key.SetValue(name, "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
                } else {
                    key.DeleteValue(name, false);
                }
            }
        }

        private void CloseApplication()
        {
            if (mCloseApplication) {
                return;
            }

            mCloseApplication = true;

            if (mSettingsWindow != null) {
                mSettingsWindow.Closed -= SettingsWindow_OnClosed;
                mSettingsWindow.Close();
                mSettingsWindow = null;
            }

            Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void CleanupApplicationResources()
        {
            if (mApplicationCleanupComplete) {
                return;
            }

            mApplicationCleanupComplete = true;
            Settings.Default.PropertyChanged -= Settings_PropertyChanged;
            ThemeService.ThemeChanged -= ThemeService_ThemeChanged;
            LocalizationService.LanguageChanged -= LocalizationService_LanguageChanged;

            if (mViewModel != null) {
                mViewModel.SpeedsUpdated -= ViewModel_SpeedsUpdated;
                mViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                mViewModel.Dispose();
                mViewModel = null;
            }

            if (mNotifyIcon != null) {
                mNotifyIcon.Visible = false;
                mNotifyIcon.Dispose();
                mNotifyIcon = null;
            }
        }
    }
}
