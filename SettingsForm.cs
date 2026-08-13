using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public partial class SettingsForm : Form
    {
        private readonly ToolStripMenuItem mLanguageMenuItemAutomatic = new ToolStripMenuItem();
        private readonly ToolStripSeparator mLanguageMenuSeparator = new ToolStripSeparator();

        private string mSentColor
        {
            get { return mSentColorLabel.Text; }
            set
            {
                mSentColorLabel.TextChanged -= mSentColorLabel_TextChanged;
                UpdateColorPreview(mSentColorLabel, value);
                mSentColorLabel.TextChanged += mSentColorLabel_TextChanged;
            }
        }

        private string mReceivedColor
        {
            get { return mReceivedColorLabel.Text; }
            set
            {
                mReceivedColorLabel.TextChanged -= mReceivedColorLabel_TextChanged;
                UpdateColorPreview(mReceivedColorLabel, value);
                mReceivedColorLabel.TextChanged += mReceivedColorLabel_TextChanged;
            }
        }

        public SettingsForm()
        {
            InitializeComponent();
            InitializeLanguageMenu();

            Icon = Properties.Resources.ApplicationIcon;
            mAppIconBox.Image = new Bitmap(Properties.Resources.ApplicationIcon.ToBitmap(), 32, 32);

            setTrackBarOpacity(Properties.Settings.Default.Opacity);
            Opacity = Properties.Settings.Default.Opacity;

            SetCheckBoxState(mCheckBoxRunOnStartup, Properties.Settings.Default.RunOnStartup, mCheckBoxRunOnStartup_CheckedChanged);
            SetCheckBoxState(mCheckBoxAlwaysOnTop, Properties.Settings.Default.AlwaysOnTop, mCheckBoxAlwaysOnTop_CheckedChanged);
            TopMost = Properties.Settings.Default.AlwaysOnTop;

            mSentColor = Properties.Settings.Default.SentColor;
            mReceivedColor = Properties.Settings.Default.ReceivedColor;

            Properties.Settings.Default.PropertyChanged += PropertyChanged;
            ThemeService.ThemeChanged += ThemeService_ThemeChanged;
            LocalizationService.LanguageChanged += LocalizationService_LanguageChanged;

            ApplyLocalization();
            ApplyTheme();
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Theme" || e.PropertyName == "Language") {
                ApplyLocalization();
                ApplyTheme();
            }
        }

        private void ThemeService_ThemeChanged(object sender, EventArgs e)
        {
            ApplyLocalization();
            ApplyTheme();
        }

        private void LocalizationService_LanguageChanged(object sender, EventArgs e)
        {
            ApplyLocalization();
            ApplyTheme();
        }

        private void TrackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            float opacity = getTrackBarOpacity();
            Properties.Settings.Default.Opacity = opacity;
            Properties.Settings.Default.Save();
            Opacity = opacity;
            mOpacityValueLabel.Text = ((int)(opacity * 100)).ToString() + "%";
        }

        private float getTrackBarOpacity()
        {
            float result = TrackBarOpacity.Value / 100F;
            if (result < 0.1F) {
                result = 0.1F;
            }

            if (result > 1F) {
                result = 1F;
            }

            return result;
        }

        private void setTrackBarOpacity(float value)
        {
            if (value < 0.1F) {
                value = 0.1F;
            }

            if (value > 1F) {
                value = 1F;
            }

            TrackBarOpacity.Value = (int)(value * 100);
            mOpacityValueLabel.Text = ((int)(value * 100)).ToString() + "%";
        }

        private void mCheckBoxRunOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RunOnStartup = mCheckBoxRunOnStartup.Checked;
            Properties.Settings.Default.Save();
        }

        private void mCheckBoxAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = mCheckBoxAlwaysOnTop.Checked;
            Properties.Settings.Default.AlwaysOnTop = mCheckBoxAlwaysOnTop.Checked;
            Properties.Settings.Default.Save();
        }

        private void mSentColorPickButton_Click(object sender, EventArgs e)
        {
            if (mColorDialog.ShowDialog() == DialogResult.OK) {
                mSentColorLabel.Text = Tools.ColorToHex(mColorDialog.Color);
            }
        }

        private void mReceivedColorPickButton_Click(object sender, EventArgs e)
        {
            if (mColorDialog.ShowDialog() == DialogResult.OK) {
                mReceivedColorLabel.Text = Tools.ColorToHex(mColorDialog.Color);
            }
        }

        private void mSentColorLabel_TextChanged(object sender, EventArgs e)
        {
            Color color = Tools.HexToColor(mSentColorLabel.Text);
            if (color == Color.Empty) {
                return;
            }

            UpdateColorPreview(mSentColorLabel, mSentColorLabel.Text);
            Properties.Settings.Default.SentColor = mSentColorLabel.Text;
            Properties.Settings.Default.Save();
        }

        private void mReceivedColorLabel_TextChanged(object sender, EventArgs e)
        {
            Color color = Tools.HexToColor(mReceivedColorLabel.Text);
            if (color == Color.Empty) {
                return;
            }

            UpdateColorPreview(mReceivedColorLabel, mReceivedColorLabel.Text);
            Properties.Settings.Default.ReceivedColor = mReceivedColorLabel.Text;
            Properties.Settings.Default.Save();
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

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                ThemeService.BeginWindowDrag(this);
            }
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

        private void SetCheckBoxState(CheckBox checkBox, bool value, EventHandler changedHandler)
        {
            checkBox.CheckedChanged -= changedHandler;
            checkBox.Checked = value;
            checkBox.CheckedChanged += changedHandler;
        }

        private void UpdateColorPreview(Label label, string value)
        {
            Color color = Tools.HexToColor(value);
            if (color == Color.Empty) {
                return;
            }

            label.Text = value;
            label.BackColor = color;
            label.ForeColor = ThemeService.GetContrastTextColor(color);
        }

        private void ApplyLocalization()
        {
            Text = LocalizationService.Text("settings");
            mWindowTitleLabel.Text = LocalizationService.Text("app_name");
            mWindowSubtitleLabel.Text = LocalizationService.Text("settings_subtitle");
            mCheckBoxRunOnStartup.Text = LocalizationService.Text("run_on_startup");
            mCheckBoxAlwaysOnTop.Text = LocalizationService.Text("always_on_top");
            label2.Text = LocalizationService.Text("opacity");
            label3.Text = LocalizationService.Text("sent_color");
            label5.Text = LocalizationService.Text("received_color");
            mSentColorPickButton.Text = LocalizationService.Text("pick");
            mReceivedColorPickButton.Text = LocalizationService.Text("pick");
            mLanguageMenuItemAutomatic.Text = LocalizationService.SystemLanguageDisplayText();
            mLanguageMenuItemEnglish.Text = "English";
            mLanguageMenuItemRussian.Text = "Русский";

            mToolTip.SetToolTip(mLanguageButton, LocalizationService.Text("tooltip_language"));
            mToolTip.SetToolTip(mThemeButton, LocalizationService.Text("tooltip_theme"));
            mToolTip.SetToolTip(mCloseButton, LocalizationService.Text("tooltip_hide"));
        }

        private void ApplyTheme()
        {
            ThemePalette palette = ThemeService.CurrentPalette;

            ThemeService.ApplyFormStyle(this);
            ThemeService.StyleTitleBarButton(mLanguageButton);
            ThemeService.StyleTitleBarButton(mThemeButton);
            ThemeService.StyleTitleBarButton(mCloseButton, true);
            ThemeService.StyleSecondaryButton(mSentColorPickButton);
            ThemeService.StyleSecondaryButton(mReceivedColorPickButton);
            ThemeService.StyleCheckBox(mCheckBoxRunOnStartup);
            ThemeService.StyleCheckBox(mCheckBoxAlwaysOnTop);

            mTitleBarPanel.BackColor = palette.TitleBarBackgroundColor;
            mAppIconBox.BackColor = palette.TitleBarBackgroundColor;
            mWindowTitleLabel.ForeColor = palette.TextColor;
            mWindowSubtitleLabel.ForeColor = palette.MutedTextColor;

            mGeneralPanel.FillColor = palette.SurfaceRaisedColor;
            mGeneralPanel.BorderColor = palette.BorderColor;
            mAppearancePanel.FillColor = palette.SurfaceRaisedColor;
            mAppearancePanel.BorderColor = palette.BorderColor;

            label2.ForeColor = palette.MutedTextColor;
            label3.ForeColor = palette.MutedTextColor;
            label5.ForeColor = palette.MutedTextColor;
            mOpacityValueLabel.ForeColor = palette.TextColor;

            TrackBarOpacity.BackColor = palette.SurfaceRaisedColor;

            mLanguageButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mThemeButton.Font = new Font("Segoe UI Symbol", 11F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mCloseButton.Font = new Font("Segoe UI Symbol", 13F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mCloseButton.ForeColor = palette.MutedTextColor;
            ApplyButtonAssets();

            UpdateColorPreview(mSentColorLabel, mSentColorLabel.Text);
            UpdateColorPreview(mReceivedColorLabel, mReceivedColorLabel.Text);
            Invalidate(true);
        }

        private void ApplyButtonAssets()
        {
            Image languageImage = LocalizationService.IsRussianLanguage(LocalizationService.CurrentLanguage)
                ? AssetService.GetScaledImage(@"Assets\Flags\RU.png", 24, 18)
                : AssetService.GetScaledImage(@"Assets\Flags\GB.png", 24, 18);
            Image themeImage = ThemeService.IsDarkTheme(ThemeService.CurrentTheme)
                ? AssetService.GetScaledImage(@"Assets\ThemeSun.png", 20, 20)
                : AssetService.GetScaledImage(@"Assets\ThemeMoon.png", 20, 20);

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

        private void SettingsForm_Move(object sender, EventArgs e)
        {
            WindowPlacementService.SaveWindowPosition(this);
        }
    }
}
