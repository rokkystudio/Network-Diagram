using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public partial class MainForm : HiddenForm
    {
        private const int DiagramReceived = 0;
        private const int DiagramSent = 1;
        private const int WmNcHitTest = 0x84;
        private const int WmNcLButtonDown = 0xA1;
        private const int HtClient = 0x1;
        private const int HtLeft = 0xA;
        private const int HtRight = 0xB;
        private const int HtTop = 0xC;
        private const int HtTopLeft = 0xD;
        private const int HtTopRight = 0xE;
        private const int HtBottom = 0xF;
        private const int HtBottomLeft = 0x10;
        private const int HtBottomRight = 0x11;
        private const int ResizeBorderSize = 6;
        private const int BottomRightResizeGripSize = 18;

        private bool mCloseApplication;
        private bool mApplicationCleanupComplete;
        private bool mCompactMode;
        private bool mGraphDragPending;
        private Point mGraphDragOrigin;
        private NotifyManager mNotifyManager;
        private SettingsForm mSettingsForm;
        private readonly ToolStripMenuItem mLanguageMenuItemAutomatic = new ToolStripMenuItem();
        private readonly ToolStripSeparator mLanguageMenuSeparator = new ToolStripSeparator();

        private NetworkAdapter mSelectedAdapter
        {
            get { return (NetworkAdapter)mAdaptersComboBox.SelectedItem; }
            set
            {
                mAdaptersComboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                mAdaptersComboBox.SelectedItem = value;
                mAdaptersComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        private int mSelectedAdapterIndex
        {
            get { return mAdaptersComboBox.SelectedIndex; }
            set
            {
                mAdaptersComboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                mAdaptersComboBox.SelectedIndex = value;
                mAdaptersComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            InitializeLanguageMenu();
            ConfigureMainWindowControls();

            Icon = Properties.Resources.ApplicationIcon;
            mNotifyIcon.Icon = Properties.Resources.ApplicationIcon;
            mNotifyManager = new NotifyManager(mNotifyIcon);
            mAppIconBox.Image = new Bitmap(Properties.Resources.ApplicationIcon.ToBitmap(), 32, 32);
            WindowPlacementService.RestoreWindowPosition(this);

            TopMost = Properties.Settings.Default.AlwaysOnTop;
            setOpacity(Properties.Settings.Default.Opacity);
            StartupEnableClassic(Properties.Settings.Default.RunOnStartup);

            Properties.Settings.Default.PropertyChanged += PropertyChanged;
            ThemeService.ThemeChanged += ThemeService_ThemeChanged;
            LocalizationService.LanguageChanged += LocalizationService_LanguageChanged;

            NetworkAdapter.UpdateAdapters(mAdaptersComboBox);
            SelectAdapterByID(Properties.Settings.Default.ActiveAdapter);

            if (mAdaptersComboBox.Items.Count > 0 && mSelectedAdapterIndex == -1) {
                mSelectedAdapterIndex = 0;
            }

            UpdateSentColor();
            UpdateReceivedColor();
            ApplyLocalization();
            ApplyTheme();
            setCompactMode(false);
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr handle, int message, IntPtr wParam, IntPtr lParam);

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == WmNcHitTest && WindowState == FormWindowState.Normal) {
                base.WndProc(ref message);
                if ((int)message.Result == HtClient) {
                    Point clientPoint = PointToClient(new Point((int)message.LParam));
                    message.Result = (IntPtr)GetResizeHandle(clientPoint);
                    return;
                }

                return;
            }

            base.WndProc(ref message);
        }

        private int GetResizeHandle(Point point)
        {
            bool bottomRightCorner = point.X >= ClientSize.Width - BottomRightResizeGripSize &&
                                     point.Y >= ClientSize.Height - BottomRightResizeGripSize;
            if (bottomRightCorner) {
                return HtBottomRight;
            }

            bool left = point.X <= ResizeBorderSize;
            bool right = point.X >= ClientSize.Width - ResizeBorderSize;
            bool top = point.Y <= ResizeBorderSize;
            bool bottom = point.Y >= ClientSize.Height - ResizeBorderSize;

            if (left && top) return HtTopLeft;
            if (right && top) return HtTopRight;
            if (left && bottom) return HtBottomLeft;
            if (right && bottom) return HtBottomRight;
            if (left) return HtLeft;
            if (right) return HtRight;
            if (top) return HtTop;
            if (bottom) return HtBottom;

            return HtClient;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!mCloseApplication) {
                e.Cancel = true;
                Hide();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CleanupApplicationResources();
            Application.ExitThread();
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mSelectedAdapter == null) {
                return;
            }

            Properties.Settings.Default.ActiveAdapter = mSelectedAdapter.Id;
            Properties.Settings.Default.Save();
        }

        public void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity") {
                setOpacity(Properties.Settings.Default.Opacity);
            }

            if (e.PropertyName == "ActiveAdapter") {
                SelectAdapterByID(Properties.Settings.Default.ActiveAdapter);
            }

            if (e.PropertyName == "RunOnStartup") {
                StartupEnableClassic(Properties.Settings.Default.RunOnStartup);
            }

            if (e.PropertyName == "AlwaysOnTop") {
                TopMost = Properties.Settings.Default.AlwaysOnTop;
            }

            if (e.PropertyName == "SentColor") {
                UpdateSentColor();
            }

            if (e.PropertyName == "ReceivedColor") {
                UpdateReceivedColor();
            }

            if (e.PropertyName == "Theme") {
                ApplyTheme();
            }

            if (e.PropertyName == "Language") {
                ApplyLocalization();
            }
        }

        private void ThemeService_ThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void LocalizationService_LanguageChanged(object sender, EventArgs e)
        {
            ApplyLocalization();
        }

        private void UpdateSentColor()
        {
            Color color = Tools.HexToColor(Properties.Settings.Default.SentColor);
            if (color != Color.Empty) {
                mDiagramBox.SetColor(DiagramSent, color);
            }
        }

        private void UpdateReceivedColor()
        {
            Color color = Tools.HexToColor(Properties.Settings.Default.ReceivedColor);
            if (color != Color.Empty) {
                mDiagramBox.SetColor(DiagramReceived, color);
            }
        }

        private void SelectAdapterByID(string id)
        {
            foreach (NetworkAdapter adapter in mAdaptersComboBox.Items)
            {
                if (adapter.Id == id) {
                    mSelectedAdapter = adapter;
                    break;
                }
            }
        }

        private void SpeedTimer_Tick(object sender, EventArgs e)
        {
            if (mSelectedAdapter == null) {
                return;
            }

            int sentSpeed = mSelectedAdapter.GetSentCount() / mSpeedTimer.Interval * 1000;
            mSentSpeedLabel.Text = FormatBytes(mSelectedAdapter.GetSentBytesAll());
            mSentRateLabel.Text = FormatRate(sentSpeed);
            mDiagramBox.AddValue(DiagramSent, sentSpeed);

            int receivedSpeed = mSelectedAdapter.GetReceivedCount() / mSpeedTimer.Interval * 1000;
            mReceivedSpeedLabel.Text = FormatBytes(mSelectedAdapter.GetReceivedBytesAll());
            mReceivedRateLabel.Text = FormatRate(receivedSpeed);
            mDiagramBox.AddValue(DiagramReceived, receivedSpeed);

            int maxValue = mDiagramBox.GetMaxValue();
            if (maxValue < 1) {
                maxValue = 1;
            }

            int sentThroughput = sentSpeed * 100 / maxValue;
            int receivedThroughput = receivedSpeed * 100 / maxValue;
            mNotifyManager.DrawIcon(NotifyManager.DefaultTrayArrowsStyle, sentThroughput, receivedThroughput);
        }

        private string FormatBytes(long bytes)
        {
            if (bytes > 0 && bytes < 1024) {
                bytes = 1024;
            }

            if (bytes == 0) {
                return "0 KB";
            }

            string[] sizes = { "", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1) {
                order++;
                bytes /= 1024;
            }

            return string.Format("{0:0.##} {1}", bytes, sizes[order]);
        }

        private string FormatRate(long bytesPerSecond)
        {
            return FormatBytes(bytesPerSecond) + "/s";
        }

        private void AdaptersTimer_Tick(object sender, EventArgs e)
        {
            NetworkAdapter.UpdateAdapters(mAdaptersComboBox);
        }

        public void OnAnimationStart()
        {
            throw new NotImplementedException();
        }

        public void OnAnimationFinish()
        {
            throw new NotImplementedException();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (Visible && WindowState == FormWindowState.Normal) {
                Hide();
                return;
            }

            showDiagram();
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Visible) {
                ClearHeaderButtonFocus();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) {
                Hide();
            }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            WindowPlacementService.SaveWindowPosition(this);
        }

        private void MainMenuItemSettings_Click(object sender, EventArgs e)
        {
            showSettings();
        }

        private void MainMenuItemCompact_Click(object sender, EventArgs e)
        {
            setCompactMode(!mCompactMode);
        }

        private void NotifyMenuItemOpen_Click(object sender, EventArgs e)
        {
            showDiagram();
        }

        private void NotifyMenuItemCompact_Click(object sender, EventArgs e)
        {
            setCompactMode(!mCompactMode);
        }

        private void NotifyMenuItemSettings_Click(object sender, EventArgs e)
        {
            showSettings();
        }

        private void NotifyMenuItemReset_Click(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;
            showDiagram();

            Properties.Settings.Default.Opacity = 1;
            Properties.Settings.Default.Save();
        }

        private void NotifyMenuItemExit_Click(object sender, EventArgs e)
        {
            closeApplication();
        }

        private void showDiagram()
        {
            WindowState = FormWindowState.Normal;
            Show();
            BringToFront();
            Activate();
            ClearHeaderButtonFocus();
        }

        private void showSettings()
        {
            if (mSettingsForm != null && !mSettingsForm.IsDisposed) {
                mSettingsForm.BringToFront();
                mSettingsForm.Activate();
                return;
            }

            mSettingsForm = new SettingsForm();
            mSettingsForm.StartPosition = FormStartPosition.Manual;
            Point settingsLocation = WindowPlacementService.EnsureVisible(new Point(Left, Top), mSettingsForm.Size);
            mSettingsForm.Left = settingsLocation.X;
            mSettingsForm.Top = settingsLocation.Y;
            mSettingsForm.FormClosed += SettingsForm_FormClosed;

            Hide();
            mSettingsForm.Show();
            mSettingsForm.BringToFront();
            mSettingsForm.Activate();
        }

        private void closeApplication()
        {
            if (mCloseApplication) {
                return;
            }

            mCloseApplication = true;

            if (mSettingsForm != null && !mSettingsForm.IsDisposed) {
                mSettingsForm.FormClosed -= SettingsForm_FormClosed;
                mSettingsForm.Close();
                mSettingsForm = null;
            }

            if (mNotifyIcon != null) {
                mNotifyIcon.Visible = false;
            }

            Application.Exit();
        }

        private void setOpacity(float value)
        {
            if (value > 0 && value <= 1F) {
                Opacity = value;
            }
        }

        private void SpeedDiagram_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            setCompactMode(!mCompactMode);
        }

        private void setCompactMode(bool enable)
        {
            mCompactMode = enable;

            mMainMenuItemCompact.Checked = enable;

            mTitleBarPanel.Visible = !enable;
            Panel.Visible = !enable;
            mBodySpacerPanel.Visible = !enable;

            mBodyPanel.Padding = enable ? Padding.Empty : new Padding(12, 0, 12, 12);
            mPanelBottom.Padding = enable ? Padding.Empty : new Padding(16, 2, 16, 12);

            ApplyTheme();
        }

        public void StartupEnableClassic(bool enabled)
        {
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true)) {
                if (key != null) {
                    if (enabled) {
                        key.SetValue(name, '"' + Application.ExecutablePath + '"');
                    } else {
                        key.DeleteValue(name, false);
                    }
                }
            }
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                ThemeService.BeginWindowDrag(this);
            }
        }

        private void ThemeButton_Click(object sender, EventArgs e)
        {
            ThemeService.ToggleTheme();
        }

        private void LanguageMenuItemAutomatic_Click(object sender, EventArgs e)
        {
            LocalizationService.CurrentLanguage = LocalizationService.AutomaticLanguage;
        }

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            ThemeService.ApplySelectionMenu(mLanguageMenu);
            mLanguageMenu.Show(mLanguageButton, new Point(0, mLanguageButton.Height));
        }

        private void LanguageMenuItemEnglish_Click(object sender, EventArgs e)
        {
            LocalizationService.CurrentLanguage = LocalizationService.EnglishLanguage;
        }

        private void LanguageMenuItemRussian_Click(object sender, EventArgs e)
        {
            LocalizationService.CurrentLanguage = LocalizationService.RussianLanguage;
        }

        private void HideWindowButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void GraphArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) {
                return;
            }

            mGraphDragPending = true;
            mGraphDragOrigin = Control.MousePosition;
        }

        private void GraphArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mGraphDragPending || e.Button != MouseButtons.Left) {
                return;
            }

            Size dragSize = SystemInformation.DragSize;
            int deltaX = System.Math.Abs(Control.MousePosition.X - mGraphDragOrigin.X);
            int deltaY = System.Math.Abs(Control.MousePosition.Y - mGraphDragOrigin.Y);
            if (deltaX < dragSize.Width / 2 && deltaY < dragSize.Height / 2) {
                return;
            }

            mGraphDragPending = false;
            ThemeService.BeginWindowDrag(this);
        }

        private void GraphArea_MouseUp(object sender, MouseEventArgs e)
        {
            mGraphDragPending = false;
        }

        private void InitializeLanguageMenu()
        {
            mLanguageMenuItemAutomatic.Click += LanguageMenuItemAutomatic_Click;
            mLanguageMenu.Items.Clear();
            mLanguageMenu.Items.AddRange(new ToolStripItem[]
            {
                mLanguageMenuItemAutomatic,
                mLanguageMenuSeparator,
                mLanguageMenuItemEnglish,
                mLanguageMenuItemRussian
            });
        }

        private void ConfigureMainWindowControls()
        {
            mAdaptersComboBox.TabStop = false;
            mLanguageButton.TabStop = false;
            mSettingsButton.TabStop = false;
            mThemeButton.TabStop = false;
            mCloseButton.TabStop = false;
            mNotifyMenu.ShowCheckMargin = false;
            mResizeGripPanel.Cursor = Cursors.SizeNWSE;
            mResizeGripPanel.BringToFront();
            Panel.SizeChanged += StatsPanel_SizeChanged;
            mSettingsButton.Enter += HeaderButton_Enter;
            mCloseButton.Enter += HeaderButton_Enter;
            mLanguageButton.Visible = false;
            mThemeButton.Visible = false;
            mSettingsButton.Visible = true;
            mCloseButton.Visible = true;
            mSettingsButton.Location = new Point(322, 4);
            mSettingsButton.BringToFront();
            mCloseButton.BringToFront();
            mWindowTitleLabel.MaximumSize = new Size(250, 0);
            mWindowSubtitleLabel.MaximumSize = new Size(250, 0);

            mAdaptersComboBox.Location = new Point(16, 14);
            mAdaptersComboBox.Size = new Size(Math.Max(220, Panel.ClientSize.Width - 32), 24);
            mSentTitleLabel.AutoSize = false;
            mSentTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

            mReceivedTitleLabel.AutoSize = false;
            mReceivedTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            mSentRateTitleLabel.AutoSize = false;
            mSentRateTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            mReceivedRateTitleLabel.AutoSize = false;
            mReceivedRateTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            mSentSpeedLabel.AutoEllipsis = true;
            mReceivedSpeedLabel.AutoEllipsis = true;
            mSentRateLabel.AutoEllipsis = true;
            mReceivedRateLabel.AutoEllipsis = true;
            mSentSpeedLabel.TextAlign = ContentAlignment.MiddleRight;
            mReceivedSpeedLabel.TextAlign = ContentAlignment.MiddleRight;
            mSentRateLabel.TextAlign = ContentAlignment.MiddleRight;
            mReceivedRateLabel.TextAlign = ContentAlignment.MiddleRight;

            Panel.Height = 82;
            mBodySpacerPanel.Height = 4;
            mPanelBottom.Padding = new Padding(16, 2, 16, 12);
            ApplyNotifyMenuLayout();
            UpdateStatsLayout();
        }

        private void ApplyLocalization()
        {
            Text = LocalizationService.Text("app_name");
            mNotifyIcon.Text = LocalizationService.Text("notify_text");

            mWindowTitleLabel.Text = LocalizationService.Text("app_name");
            mWindowSubtitleLabel.Text = LocalizationService.Text("app_subtitle");
            mSentTitleLabel.Text = LocalizationService.Text("upload");
            mReceivedTitleLabel.Text = LocalizationService.Text("download");
            mSentRateTitleLabel.Text = LocalizationService.Text("upload_speed");
            mReceivedRateTitleLabel.Text = LocalizationService.Text("download_speed");

            NotifyMenuItemOpen.Text = LocalizationService.Text("open");
            NotifyMenuItemCompact.Text = LocalizationService.Text("compact_mode");
            NotifyMenuItemSettings.Text = LocalizationService.Text("settings");
            NotifyMenuItemReset.Text = LocalizationService.Text("reset");
            NotifyMenuItemExit.Text = LocalizationService.Text("exit");

            mMainMenuItemCompact.Text = LocalizationService.Text("compact_mode");
            mMainMenuItemSettings.Text = LocalizationService.Text("settings");
            mMainMenuItemExit.Text = LocalizationService.Text("exit");
            mLanguageMenuItemAutomatic.Text = LocalizationService.SystemLanguageDisplayText();
            mLanguageMenuItemEnglish.Text = "English";
            mLanguageMenuItemRussian.Text = "Русский";

            mToolTip.SetToolTip(mLanguageButton, LocalizationService.Text("tooltip_language"));
            mToolTip.SetToolTip(mSettingsButton, LocalizationService.Text("tooltip_settings"));
            mToolTip.SetToolTip(mThemeButton, LocalizationService.Text("tooltip_theme"));
            mToolTip.SetToolTip(mCloseButton, LocalizationService.Text("tooltip_hide"));
            ApplyButtonAssets();
            UpdateStatsLayout();
        }

        private void ApplyTheme()
        {
            ThemePalette palette = ThemeService.CurrentPalette;

            ThemeService.ApplyFormStyle(this);
            ThemeService.ApplyToolStrip(mNotifyMenu);
            ApplyNotifyMenuLayout();
            ThemeService.StyleComboBox(mAdaptersComboBox);
            ThemeService.StyleTitleBarButton(mLanguageButton);
            ThemeService.StyleTitleBarButton(mSettingsButton);
            ThemeService.StyleTitleBarButton(mThemeButton);
            ThemeService.StyleTitleBarButton(mCloseButton, true);

            mTitleBarPanel.BackColor = palette.TitleBarBackgroundColor;
            mTitleBarPanel.ForeColor = palette.TextColor;
            mAppIconBox.BackColor = palette.TitleBarBackgroundColor;
            mWindowTitleLabel.ForeColor = palette.TextColor;
            mWindowSubtitleLabel.ForeColor = palette.MutedTextColor;

            mBodyPanel.BackColor = palette.BackgroundColor;
            mBodySpacerPanel.BackColor = palette.BackgroundColor;
            mResizeGripPanel.BackColor = palette.BackgroundColor;

            Panel.FillColor = palette.BackgroundColor;
            Panel.BorderColor = palette.BackgroundColor;

            mPanelBottom.FillColor = palette.BackgroundColor;
            mPanelBottom.BorderColor = palette.BackgroundColor;

            mSentTitleLabel.ForeColor = palette.MutedTextColor;
            mReceivedTitleLabel.ForeColor = palette.MutedTextColor;
            mSentRateTitleLabel.ForeColor = palette.MutedTextColor;
            mReceivedRateTitleLabel.ForeColor = palette.MutedTextColor;
            mSentSpeedLabel.ForeColor = palette.TextColor;
            mReceivedSpeedLabel.ForeColor = palette.TextColor;
            mSentRateLabel.ForeColor = palette.TextColor;
            mReceivedRateLabel.ForeColor = palette.TextColor;
            mSentTitleLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mReceivedTitleLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mSentRateTitleLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mReceivedRateTitleLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mSentSpeedLabel.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mReceivedSpeedLabel.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mSentRateLabel.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mReceivedRateLabel.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Regular, GraphicsUnit.Point, 204);

            mLanguageButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mSettingsButton.Font = new Font("Segoe UI Symbol", 11F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mThemeButton.Font = new Font("Segoe UI Symbol", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mCloseButton.Font = new Font("Segoe UI Symbol", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mCloseButton.ForeColor = Color.White;

            ApplyButtonAssets();
            mDiagramBox.SetTheme(palette);
            mResizeGripPanel.Invalidate();
            Invalidate(true);
        }

        private void ApplyButtonAssets()
        {
            Image languageImage = LocalizationService.IsRussianLanguage(LocalizationService.CurrentLanguage)
                ? AssetService.GetScaledImage(@"Assets\Flags\RU.png", 24, 18)
                : AssetService.GetScaledImage(@"Assets\Flags\GB.png", 24, 18);
            Image themeImage = ThemeService.IsDarkTheme(ThemeService.CurrentTheme)
                ? AssetService.GetScaledImage(@"Assets\Icons\ThemeSun.png", 20, 20)
                : AssetService.GetScaledImage(@"Assets\Icons\ThemeMoon.png", 20, 20);

            mLanguageButton.Image = languageImage;
            mLanguageButton.Text = languageImage == null
                ? (LocalizationService.IsRussianLanguage(LocalizationService.CurrentLanguage) ? "RU" : "EN")
                : string.Empty;
            mLanguageButton.ImageAlign = ContentAlignment.MiddleCenter;
            mLanguageButton.Padding = Padding.Empty;

            mThemeButton.Image = themeImage;
            mThemeButton.Text = themeImage == null
                ? (ThemeService.IsDarkTheme(ThemeService.CurrentTheme) ? "☀" : "☾")
                : string.Empty;
            mThemeButton.ImageAlign = ContentAlignment.MiddleCenter;
            mThemeButton.Padding = Padding.Empty;

            mSettingsButton.Image = null;
            mSettingsButton.Text = "⚙";
            mSettingsButton.TextAlign = ContentAlignment.MiddleCenter;
            mCloseButton.Image = null;
            mCloseButton.Text = "✕";
            mCloseButton.TextAlign = ContentAlignment.MiddleCenter;
            mLanguageMenuItemAutomatic.Image = LocalizationService.IsRussianLanguage(LocalizationService.DetectedLanguage)
                ? AssetService.GetScaledImage(@"Assets\Flags\RU.png", 20, 14)
                : AssetService.GetScaledImage(@"Assets\Flags\GB.png", 20, 14);
            mLanguageMenuItemEnglish.Image = AssetService.GetScaledImage(@"Assets\Flags\GB.png", 20, 14);
            mLanguageMenuItemRussian.Image = AssetService.GetScaledImage(@"Assets\Flags\RU.png", 20, 14);
            mLanguageMenuItemAutomatic.Checked = LocalizationService.IsAutomaticLanguageSelection;
            mLanguageMenuItemEnglish.Checked = !LocalizationService.IsAutomaticLanguageSelection &&
                !LocalizationService.IsRussianLanguage(LocalizationService.CurrentLanguage);
            mLanguageMenuItemRussian.Checked = !LocalizationService.IsAutomaticLanguageSelection &&
                LocalizationService.IsRussianLanguage(LocalizationService.CurrentLanguage);
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mSettingsForm != null) {
                Left = mSettingsForm.Left;
                Top = mSettingsForm.Top;
                mSettingsForm.FormClosed -= SettingsForm_FormClosed;
                mSettingsForm = null;
            }

            if (!mCloseApplication) {
                showDiagram();
            }
        }

        private void StatsPanel_SizeChanged(object sender, EventArgs e)
        {
            UpdateStatsLayout();
        }

        private void UpdateStatsLayout()
        {
            const int leftPadding = 16;
            const int rightPadding = 16;
            const int topPadding = 10;
            const int comboHeight = 24;
            const int statsTop = 44;
            const int rowHeight = 18;
            const int groupGap = 16;
            const int labelGap = 8;

            int contentWidth = Math.Max(220, Panel.ClientSize.Width - leftPadding - rightPadding);
            mAdaptersComboBox.Location = new Point(leftPadding, topPadding);
            mAdaptersComboBox.Size = new Size(contentWidth, comboHeight);

            int groupWidth = Math.Max(110, (contentWidth - groupGap) / 2);
            int leftGroupX = leftPadding;
            int rightGroupX = leftPadding + groupWidth + groupGap;

            int sentTitleWidth = Math.Max(48, TextRenderer.MeasureText(mSentTitleLabel.Text ?? string.Empty, mSentTitleLabel.Font).Width + 4);
            int receivedTitleWidth = Math.Max(68, TextRenderer.MeasureText(mReceivedTitleLabel.Text ?? string.Empty, mReceivedTitleLabel.Font).Width + 4);
            int sentRateTitleWidth = Math.Max(60, TextRenderer.MeasureText(mSentRateTitleLabel.Text ?? string.Empty, mSentRateTitleLabel.Font).Width + 4);
            int receivedRateTitleWidth = Math.Max(60, TextRenderer.MeasureText(mReceivedRateTitleLabel.Text ?? string.Empty, mReceivedRateTitleLabel.Font).Width + 4);

            int sentValueWidth = Math.Max(36, groupWidth - sentTitleWidth - labelGap);
            int receivedValueWidth = Math.Max(36, groupWidth - receivedTitleWidth - labelGap);
            int sentRateValueWidth = Math.Max(36, groupWidth - sentRateTitleWidth - labelGap);
            int receivedRateValueWidth = Math.Max(36, groupWidth - receivedRateTitleWidth - labelGap);

            mSentTitleLabel.Location = new Point(leftGroupX, statsTop);
            mSentTitleLabel.Size = new Size(sentTitleWidth, rowHeight);
            mSentSpeedLabel.Location = new Point(leftGroupX + sentTitleWidth + labelGap, statsTop);
            mSentSpeedLabel.Size = new Size(sentValueWidth, rowHeight);

            mSentRateTitleLabel.Location = new Point(rightGroupX, statsTop);
            mSentRateTitleLabel.Size = new Size(sentRateTitleWidth, rowHeight);
            mSentRateLabel.Location = new Point(rightGroupX + sentRateTitleWidth + labelGap, statsTop);
            mSentRateLabel.Size = new Size(sentRateValueWidth, rowHeight);

            int secondRowTop = statsTop + rowHeight;
            mReceivedTitleLabel.Location = new Point(leftGroupX, secondRowTop);
            mReceivedTitleLabel.Size = new Size(receivedTitleWidth, rowHeight);
            mReceivedSpeedLabel.Location = new Point(leftGroupX + receivedTitleWidth + labelGap, secondRowTop);
            mReceivedSpeedLabel.Size = new Size(receivedValueWidth, rowHeight);

            mReceivedRateTitleLabel.Location = new Point(rightGroupX, secondRowTop);
            mReceivedRateTitleLabel.Size = new Size(receivedRateTitleWidth, rowHeight);
            mReceivedRateLabel.Location = new Point(rightGroupX + receivedRateTitleWidth + labelGap, secondRowTop);
            mReceivedRateLabel.Size = new Size(receivedRateValueWidth, rowHeight);
        }

        private void ApplyNotifyMenuLayout()
        {
            mNotifyMenu.Padding = new Padding(2);
            mNotifyMenu.ShowImageMargin = false;
            mNotifyMenu.ShowCheckMargin = false;

            foreach (ToolStripItem item in mNotifyMenu.Items) {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem == null) {
                    continue;
                }

                menuItem.AutoSize = true;
                menuItem.Margin = Padding.Empty;
                menuItem.Padding = new Padding(10, 2, 10, 2);
            }
        }

        private void HeaderButton_Enter(object sender, EventArgs e)
        {
            ClearHeaderButtonFocus();
        }

        private void ClearHeaderButtonFocus()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                if (ActiveControl == mSettingsButton || ActiveControl == mCloseButton ||
                    ActiveControl == mLanguageButton || ActiveControl == mThemeButton) {
                    ActiveControl = null;
                }
            });
        }

        private void ResizeGripPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || WindowState != FormWindowState.Normal) {
                return;
            }

            ReleaseCapture();
            SendMessage(Handle, WmNcLButtonDown, (IntPtr)HtBottomRight, IntPtr.Zero);
        }

        private void ResizeGripPanel_Paint(object sender, PaintEventArgs e)
        {
            ThemePalette palette = ThemeService.CurrentPalette;
            e.Graphics.Clear(palette.BackgroundColor);

            using (Pen pen = new Pen(palette.BorderColor)) {
                int size = mResizeGripPanel.Width - 1;
                e.Graphics.DrawLine(pen, size - 10, size, size, size - 10);
                e.Graphics.DrawLine(pen, size - 6, size, size, size - 6);
                e.Graphics.DrawLine(pen, size - 2, size, size, size - 2);
            }
        }

        private void CleanupApplicationResources()
        {
            if (mApplicationCleanupComplete) {
                return;
            }

            mApplicationCleanupComplete = true;

            mSpeedTimer.Stop();
            mAdaptersTimer.Stop();
            mDiagramBox.Stop();

            Properties.Settings.Default.PropertyChanged -= PropertyChanged;
            ThemeService.ThemeChanged -= ThemeService_ThemeChanged;
            LocalizationService.LanguageChanged -= LocalizationService_LanguageChanged;

            if (mNotifyIcon != null) {
                mNotifyIcon.Visible = false;
                mNotifyIcon.Dispose();
            }
        }
    }
}
