namespace NetworkDiagram
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mSpeedTimer = new System.Windows.Forms.Timer(this.components);
            this.mAdaptersTimer = new System.Windows.Forms.Timer(this.components);
            this.mNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mNotifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NotifyMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenuItemCompact = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenuItemReset = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mMainMenu = new System.Windows.Forms.MenuStrip();
            this.mMainMenuItemCompact = new System.Windows.Forms.ToolStripMenuItem();
            this.mMainMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mMainMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mLanguageMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mLanguageMenuItemEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.mLanguageMenuItemRussian = new System.Windows.Forms.ToolStripMenuItem();
            this.mTitleBarPanel = new System.Windows.Forms.Panel();
            this.mLanguageButton = new System.Windows.Forms.Button();
            this.mSettingsButton = new System.Windows.Forms.Button();
            this.mThemeButton = new System.Windows.Forms.Button();
            this.mCloseButton = new System.Windows.Forms.Button();
            this.mWindowSubtitleLabel = new System.Windows.Forms.Label();
            this.mWindowTitleLabel = new System.Windows.Forms.Label();
            this.mAppIconBox = new System.Windows.Forms.PictureBox();
            this.mBodyPanel = new System.Windows.Forms.Panel();
            this.mPanelBottom = new NetworkDiagram.SurfacePanel();
            this.mResizeGripPanel = new System.Windows.Forms.Panel();
            this.mDiagramBox = new NetworkDiagram.DiagramBox();
            this.mBodySpacerPanel = new System.Windows.Forms.Panel();
            this.Panel = new NetworkDiagram.SurfacePanel();
            this.mReceivedRateLabel = new System.Windows.Forms.Label();
            this.mSentRateLabel = new System.Windows.Forms.Label();
            this.mReceivedRateTitleLabel = new System.Windows.Forms.Label();
            this.mSentRateTitleLabel = new System.Windows.Forms.Label();
            this.mReceivedTitleLabel = new System.Windows.Forms.Label();
            this.mSentTitleLabel = new System.Windows.Forms.Label();
            this.mReceivedSpeedLabel = new System.Windows.Forms.Label();
            this.mSentSpeedLabel = new System.Windows.Forms.Label();
            this.mAdaptersComboBox = new System.Windows.Forms.ComboBox();
            this.mNotifyMenu.SuspendLayout();
            this.mMainMenu.SuspendLayout();
            this.mLanguageMenu.SuspendLayout();
            this.mTitleBarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAppIconBox)).BeginInit();
            this.mBodyPanel.SuspendLayout();
            this.mPanelBottom.SuspendLayout();
            this.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mSpeedTimer
            // 
            this.mSpeedTimer.Enabled = true;
            this.mSpeedTimer.Interval = 250;
            this.mSpeedTimer.Tick += new System.EventHandler(this.SpeedTimer_Tick);
            // 
            // mAdaptersTimer
            // 
            this.mAdaptersTimer.Enabled = true;
            this.mAdaptersTimer.Interval = 5000;
            this.mAdaptersTimer.Tick += new System.EventHandler(this.AdaptersTimer_Tick);
            // 
            // mNotifyIcon
            // 
            this.mNotifyIcon.ContextMenuStrip = this.mNotifyMenu;
            this.mNotifyIcon.Text = "Network Diagram";
            this.mNotifyIcon.Visible = true;
            this.mNotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            this.mNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseClick);
            // 
            // mNotifyMenu
            // 
            this.mNotifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NotifyMenuItemOpen,
            this.NotifyMenuItemCompact,
            this.NotifyMenuItemSettings,
            this.NotifyMenuItemReset,
            this.NotifyMenuItemExit});
            this.mNotifyMenu.Name = "mNotifyMenu";
            this.mNotifyMenu.Size = new System.Drawing.Size(181, 136);
            // 
            // NotifyMenuItemOpen
            // 
            this.NotifyMenuItemOpen.Name = "NotifyMenuItemOpen";
            this.NotifyMenuItemOpen.Size = new System.Drawing.Size(180, 22);
            this.NotifyMenuItemOpen.Text = "Open";
            this.NotifyMenuItemOpen.Click += new System.EventHandler(this.NotifyMenuItemOpen_Click);
            // 
            // NotifyMenuItemCompact
            // 
            this.NotifyMenuItemCompact.Name = "NotifyMenuItemCompact";
            this.NotifyMenuItemCompact.Size = new System.Drawing.Size(180, 22);
            this.NotifyMenuItemCompact.Text = "Compact Mode";
            this.NotifyMenuItemCompact.Click += new System.EventHandler(this.NotifyMenuItemCompact_Click);
            // 
            // NotifyMenuItemSettings
            // 
            this.NotifyMenuItemSettings.Name = "NotifyMenuItemSettings";
            this.NotifyMenuItemSettings.Size = new System.Drawing.Size(180, 22);
            this.NotifyMenuItemSettings.Text = "Settings";
            this.NotifyMenuItemSettings.Click += new System.EventHandler(this.NotifyMenuItemSettings_Click);
            // 
            // NotifyMenuItemReset
            // 
            this.NotifyMenuItemReset.Name = "NotifyMenuItemReset";
            this.NotifyMenuItemReset.Size = new System.Drawing.Size(180, 22);
            this.NotifyMenuItemReset.Text = "Reset";
            this.NotifyMenuItemReset.Click += new System.EventHandler(this.NotifyMenuItemReset_Click);
            // 
            // NotifyMenuItemExit
            // 
            this.NotifyMenuItemExit.Name = "NotifyMenuItemExit";
            this.NotifyMenuItemExit.Size = new System.Drawing.Size(180, 22);
            this.NotifyMenuItemExit.Text = "Exit";
            this.NotifyMenuItemExit.Click += new System.EventHandler(this.NotifyMenuItemExit_Click);
            // 
            // mMainMenu
            // 
            this.mMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMainMenuItemCompact,
            this.mMainMenuItemSettings,
            this.mMainMenuItemExit});
            this.mMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mMainMenu.Name = "mMainMenu";
            this.mMainMenu.Size = new System.Drawing.Size(420, 24);
            this.mMainMenu.TabIndex = 0;
            this.mMainMenu.Visible = false;
            // 
            // mMainMenuItemCompact
            // 
            this.mMainMenuItemCompact.Name = "mMainMenuItemCompact";
            this.mMainMenuItemCompact.Size = new System.Drawing.Size(102, 20);
            this.mMainMenuItemCompact.Text = "Compact Mode";
            this.mMainMenuItemCompact.Click += new System.EventHandler(this.MainMenuItemCompact_Click);
            // 
            // mMainMenuItemSettings
            // 
            this.mMainMenuItemSettings.Name = "mMainMenuItemSettings";
            this.mMainMenuItemSettings.Size = new System.Drawing.Size(61, 20);
            this.mMainMenuItemSettings.Text = "Settings";
            this.mMainMenuItemSettings.Click += new System.EventHandler(this.MainMenuItemSettings_Click);
            // 
            // mMainMenuItemExit
            // 
            this.mMainMenuItemExit.Name = "mMainMenuItemExit";
            this.mMainMenuItemExit.Size = new System.Drawing.Size(38, 20);
            this.mMainMenuItemExit.Text = "Exit";
            this.mMainMenuItemExit.Click += new System.EventHandler(this.NotifyMenuItemExit_Click);
            // 
            // mLanguageMenu
            // 
            this.mLanguageMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mLanguageMenuItemEnglish,
            this.mLanguageMenuItemRussian});
            this.mLanguageMenu.Name = "mLanguageMenu";
            this.mLanguageMenu.Size = new System.Drawing.Size(123, 48);
            // 
            // mLanguageMenuItemEnglish
            // 
            this.mLanguageMenuItemEnglish.Name = "mLanguageMenuItemEnglish";
            this.mLanguageMenuItemEnglish.Size = new System.Drawing.Size(122, 22);
            this.mLanguageMenuItemEnglish.Text = "English";
            this.mLanguageMenuItemEnglish.Click += new System.EventHandler(this.LanguageMenuItemEnglish_Click);
            // 
            // mLanguageMenuItemRussian
            // 
            this.mLanguageMenuItemRussian.Name = "mLanguageMenuItemRussian";
            this.mLanguageMenuItemRussian.Size = new System.Drawing.Size(122, 22);
            this.mLanguageMenuItemRussian.Text = "Русский";
            this.mLanguageMenuItemRussian.Click += new System.EventHandler(this.LanguageMenuItemRussian_Click);
            // 
            // mTitleBarPanel
            // 
            this.mTitleBarPanel.Controls.Add(this.mSettingsButton);
            this.mTitleBarPanel.Controls.Add(this.mCloseButton);
            this.mTitleBarPanel.Controls.Add(this.mWindowSubtitleLabel);
            this.mTitleBarPanel.Controls.Add(this.mWindowTitleLabel);
            this.mTitleBarPanel.Controls.Add(this.mAppIconBox);
            this.mTitleBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mTitleBarPanel.Location = new System.Drawing.Point(1, 1);
            this.mTitleBarPanel.Name = "mTitleBarPanel";
            this.mTitleBarPanel.Size = new System.Drawing.Size(418, 56);
            this.mTitleBarPanel.TabIndex = 1;
            this.mTitleBarPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            // 
            // mLanguageButton
            // 
            this.mLanguageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mLanguageButton.Location = new System.Drawing.Point(226, 4);
            this.mLanguageButton.Name = "mLanguageButton";
            this.mLanguageButton.Size = new System.Drawing.Size(48, 48);
            this.mLanguageButton.TabIndex = 3;
            this.mLanguageButton.UseVisualStyleBackColor = true;
            this.mLanguageButton.Click += new System.EventHandler(this.LanguageButton_Click);
            // 
            // mSettingsButton
            // 
            this.mSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mSettingsButton.Location = new System.Drawing.Point(322, 4);
            this.mSettingsButton.Name = "mSettingsButton";
            this.mSettingsButton.Size = new System.Drawing.Size(48, 48);
            this.mSettingsButton.TabIndex = 4;
            this.mSettingsButton.Text = "⚙";
            this.mSettingsButton.UseVisualStyleBackColor = true;
            this.mSettingsButton.Click += new System.EventHandler(this.MainMenuItemSettings_Click);
            // 
            // mThemeButton
            // 
            this.mThemeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mThemeButton.Location = new System.Drawing.Point(322, 4);
            this.mThemeButton.Name = "mThemeButton";
            this.mThemeButton.Size = new System.Drawing.Size(48, 48);
            this.mThemeButton.TabIndex = 5;
            this.mThemeButton.Text = "☾";
            this.mThemeButton.UseVisualStyleBackColor = true;
            this.mThemeButton.Click += new System.EventHandler(this.ThemeButton_Click);
            // 
            // mCloseButton
            // 
            this.mCloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mCloseButton.Location = new System.Drawing.Point(370, 4);
            this.mCloseButton.Name = "mCloseButton";
            this.mCloseButton.Size = new System.Drawing.Size(48, 48);
            this.mCloseButton.TabIndex = 6;
            this.mCloseButton.Text = "✕";
            this.mCloseButton.UseVisualStyleBackColor = true;
            this.mCloseButton.Click += new System.EventHandler(this.HideWindowButton_Click);
            // 
            // mWindowSubtitleLabel
            // 
            this.mWindowSubtitleLabel.AutoSize = true;
            this.mWindowSubtitleLabel.Location = new System.Drawing.Point(68, 31);
            this.mWindowSubtitleLabel.Name = "mWindowSubtitleLabel";
            this.mWindowSubtitleLabel.Size = new System.Drawing.Size(77, 15);
            this.mWindowSubtitleLabel.TabIndex = 2;
            this.mWindowSubtitleLabel.Text = "Traffic monitor";
            this.mWindowSubtitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            // 
            // mWindowTitleLabel
            // 
            this.mWindowTitleLabel.AutoSize = true;
            this.mWindowTitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mWindowTitleLabel.Location = new System.Drawing.Point(68, 10);
            this.mWindowTitleLabel.Name = "mWindowTitleLabel";
            this.mWindowTitleLabel.Size = new System.Drawing.Size(146, 21);
            this.mWindowTitleLabel.TabIndex = 1;
            this.mWindowTitleLabel.Text = "NETWORK DIAGRAM";
            this.mWindowTitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            // 
            // mAppIconBox
            // 
            this.mAppIconBox.Location = new System.Drawing.Point(8, 4);
            this.mAppIconBox.Name = "mAppIconBox";
            this.mAppIconBox.Size = new System.Drawing.Size(48, 48);
            this.mAppIconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mAppIconBox.TabIndex = 0;
            this.mAppIconBox.TabStop = false;
            this.mAppIconBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            // 
            // mBodyPanel
            // 
            this.mBodyPanel.Controls.Add(this.mPanelBottom);
            this.mBodyPanel.Controls.Add(this.mBodySpacerPanel);
            this.mBodyPanel.Controls.Add(this.Panel);
            this.mBodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mBodyPanel.Location = new System.Drawing.Point(1, 57);
            this.mBodyPanel.Name = "mBodyPanel";
            this.mBodyPanel.Padding = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.mBodyPanel.Size = new System.Drawing.Size(418, 262);
            this.mBodyPanel.TabIndex = 2;
            // 
            // mPanelBottom
            // 
            this.mPanelBottom.Controls.Add(this.mDiagramBox);
            this.mPanelBottom.CornerRadius = 0;
            this.mPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPanelBottom.Location = new System.Drawing.Point(12, 82);
            this.mPanelBottom.Name = "mPanelBottom";
            this.mPanelBottom.Padding = new System.Windows.Forms.Padding(16, 2, 16, 12);
            this.mPanelBottom.Size = new System.Drawing.Size(394, 168);
            this.mPanelBottom.TabIndex = 2;
            // 
            // mResizeGripPanel
            // 
            this.mResizeGripPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mResizeGripPanel.Location = new System.Drawing.Point(401, 301);
            this.mResizeGripPanel.Name = "mResizeGripPanel";
            this.mResizeGripPanel.Size = new System.Drawing.Size(18, 18);
            this.mResizeGripPanel.TabIndex = 1;
            this.mResizeGripPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ResizeGripPanel_Paint);
            this.mResizeGripPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResizeGripPanel_MouseDown);
            // 
            // mDiagramBox
            // 
            this.mDiagramBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mDiagramBox.Location = new System.Drawing.Point(16, 14);
            this.mDiagramBox.Name = "mDiagramBox";
            this.mDiagramBox.Size = new System.Drawing.Size(362, 102);
            this.mDiagramBox.TabIndex = 0;
            this.mDiagramBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GraphArea_MouseDown);
            this.mDiagramBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SpeedDiagram_MouseDoubleClick);
            this.mDiagramBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GraphArea_MouseMove);
            this.mDiagramBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GraphArea_MouseUp);
            // 
            // mBodySpacerPanel
            // 
            this.mBodySpacerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mBodySpacerPanel.Location = new System.Drawing.Point(12, 78);
            this.mBodySpacerPanel.Name = "mBodySpacerPanel";
            this.mBodySpacerPanel.Size = new System.Drawing.Size(394, 4);
            this.mBodySpacerPanel.TabIndex = 1;
            // 
            // Panel
            // 
            this.Panel.Controls.Add(this.mReceivedRateLabel);
            this.Panel.Controls.Add(this.mSentRateLabel);
            this.Panel.Controls.Add(this.mReceivedRateTitleLabel);
            this.Panel.Controls.Add(this.mSentRateTitleLabel);
            this.Panel.Controls.Add(this.mReceivedTitleLabel);
            this.Panel.Controls.Add(this.mSentTitleLabel);
            this.Panel.Controls.Add(this.mReceivedSpeedLabel);
            this.Panel.Controls.Add(this.mSentSpeedLabel);
            this.Panel.Controls.Add(this.mAdaptersComboBox);
            this.Panel.CornerRadius = 0;
            this.Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel.Location = new System.Drawing.Point(12, 0);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(394, 78);
            this.Panel.TabIndex = 0;
            // 
            // mReceivedRateLabel
            // 
            this.mReceivedRateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mReceivedRateLabel.Location = new System.Drawing.Point(304, 57);
            this.mReceivedRateLabel.Name = "mReceivedRateLabel";
            this.mReceivedRateLabel.Size = new System.Drawing.Size(74, 18);
            this.mReceivedRateLabel.TabIndex = 8;
            this.mReceivedRateLabel.Text = "0 KB/s";
            this.mReceivedRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mSentRateLabel
            // 
            this.mSentRateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mSentRateLabel.Location = new System.Drawing.Point(304, 39);
            this.mSentRateLabel.Name = "mSentRateLabel";
            this.mSentRateLabel.Size = new System.Drawing.Size(74, 18);
            this.mSentRateLabel.TabIndex = 7;
            this.mSentRateLabel.Text = "0 KB/s";
            this.mSentRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mReceivedRateTitleLabel
            // 
            this.mReceivedRateTitleLabel.AutoSize = true;
            this.mReceivedRateTitleLabel.Location = new System.Drawing.Point(208, 59);
            this.mReceivedRateTitleLabel.Name = "mReceivedRateTitleLabel";
            this.mReceivedRateTitleLabel.Size = new System.Drawing.Size(69, 15);
            this.mReceivedRateTitleLabel.TabIndex = 6;
            this.mReceivedRateTitleLabel.Text = "Down speed";
            // 
            // mSentRateTitleLabel
            // 
            this.mSentRateTitleLabel.AutoSize = true;
            this.mSentRateTitleLabel.Location = new System.Drawing.Point(208, 41);
            this.mSentRateTitleLabel.Name = "mSentRateTitleLabel";
            this.mSentRateTitleLabel.Size = new System.Drawing.Size(53, 15);
            this.mSentRateTitleLabel.TabIndex = 5;
            this.mSentRateTitleLabel.Text = "Up speed";
            // 
            // mReceivedTitleLabel
            // 
            this.mReceivedTitleLabel.AutoSize = true;
            this.mReceivedTitleLabel.Location = new System.Drawing.Point(16, 57);
            this.mReceivedTitleLabel.Name = "mReceivedTitleLabel";
            this.mReceivedTitleLabel.Size = new System.Drawing.Size(55, 15);
            this.mReceivedTitleLabel.TabIndex = 4;
            this.mReceivedTitleLabel.Text = "Download";
            // 
            // mSentTitleLabel
            // 
            this.mSentTitleLabel.AutoSize = true;
            this.mSentTitleLabel.Location = new System.Drawing.Point(16, 39);
            this.mSentTitleLabel.Name = "mSentTitleLabel";
            this.mSentTitleLabel.Size = new System.Drawing.Size(43, 15);
            this.mSentTitleLabel.TabIndex = 3;
            this.mSentTitleLabel.Text = "Upload";
            // 
            // mReceivedSpeedLabel
            // 
            this.mReceivedSpeedLabel.Location = new System.Drawing.Point(110, 57);
            this.mReceivedSpeedLabel.Name = "mReceivedSpeedLabel";
            this.mReceivedSpeedLabel.Size = new System.Drawing.Size(84, 18);
            this.mReceivedSpeedLabel.TabIndex = 2;
            this.mReceivedSpeedLabel.Text = "0 KB";
            this.mReceivedSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mSentSpeedLabel
            // 
            this.mSentSpeedLabel.Location = new System.Drawing.Point(110, 39);
            this.mSentSpeedLabel.Name = "mSentSpeedLabel";
            this.mSentSpeedLabel.Size = new System.Drawing.Size(84, 18);
            this.mSentSpeedLabel.TabIndex = 1;
            this.mSentSpeedLabel.Text = "0 KB";
            this.mSentSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mAdaptersComboBox
            // 
            this.mAdaptersComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mAdaptersComboBox.FormattingEnabled = true;
            this.mAdaptersComboBox.Location = new System.Drawing.Point(16, 12);
            this.mAdaptersComboBox.Name = "mAdaptersComboBox";
            this.mAdaptersComboBox.Size = new System.Drawing.Size(362, 23);
            this.mAdaptersComboBox.TabIndex = 0;
            this.mAdaptersComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 320);
            this.Controls.Add(this.mResizeGripPanel);
            this.Controls.Add(this.mBodyPanel);
            this.Controls.Add(this.mTitleBarPanel);
            this.Controls.Add(this.mMainMenu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mMainMenu;
            this.MinimumSize = new System.Drawing.Size(320, 180);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Network Diagram";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mNotifyMenu.ResumeLayout(false);
            this.mMainMenu.ResumeLayout(false);
            this.mMainMenu.PerformLayout();
            this.mLanguageMenu.ResumeLayout(false);
            this.mTitleBarPanel.ResumeLayout(false);
            this.mTitleBarPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAppIconBox)).EndInit();
            this.mBodyPanel.ResumeLayout(false);
            this.mPanelBottom.ResumeLayout(false);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer mSpeedTimer;
        private System.Windows.Forms.Timer mAdaptersTimer;
        private System.Windows.Forms.NotifyIcon mNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip mNotifyMenu;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemCompact;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemReset;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemExit;
        private System.Windows.Forms.MenuStrip mMainMenu;
        private System.Windows.Forms.ToolStripMenuItem mMainMenuItemCompact;
        private System.Windows.Forms.ToolStripMenuItem mMainMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem mMainMenuItemExit;
        private System.Windows.Forms.ToolTip mToolTip;
        private System.Windows.Forms.ContextMenuStrip mLanguageMenu;
        private System.Windows.Forms.ToolStripMenuItem mLanguageMenuItemEnglish;
        private System.Windows.Forms.ToolStripMenuItem mLanguageMenuItemRussian;
        private System.Windows.Forms.Panel mTitleBarPanel;
        private System.Windows.Forms.Button mLanguageButton;
        private System.Windows.Forms.Button mSettingsButton;
        private System.Windows.Forms.Button mThemeButton;
        private System.Windows.Forms.Button mCloseButton;
        private System.Windows.Forms.Label mWindowSubtitleLabel;
        private System.Windows.Forms.Label mWindowTitleLabel;
        private System.Windows.Forms.PictureBox mAppIconBox;
        private System.Windows.Forms.Panel mBodyPanel;
        private SurfacePanel mPanelBottom;
        private System.Windows.Forms.Panel mResizeGripPanel;
        private DiagramBox mDiagramBox;
        private System.Windows.Forms.Panel mBodySpacerPanel;
        private SurfacePanel Panel;
        private System.Windows.Forms.Label mReceivedRateLabel;
        private System.Windows.Forms.Label mSentRateLabel;
        private System.Windows.Forms.Label mReceivedRateTitleLabel;
        private System.Windows.Forms.Label mSentRateTitleLabel;
        private System.Windows.Forms.Label mReceivedTitleLabel;
        private System.Windows.Forms.Label mSentTitleLabel;
        private System.Windows.Forms.Label mReceivedSpeedLabel;
        private System.Windows.Forms.Label mSentSpeedLabel;
        private System.Windows.Forms.ComboBox mAdaptersComboBox;
    }
}
