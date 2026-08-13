namespace NetworkDiagram
{
    partial class SettingsForm
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
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mLanguageMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mLanguageMenuItemEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.mLanguageMenuItemRussian = new System.Windows.Forms.ToolStripMenuItem();
            this.mTitleBarPanel = new System.Windows.Forms.Panel();
            this.mLanguageButton = new System.Windows.Forms.Button();
            this.mThemeButton = new System.Windows.Forms.Button();
            this.mCloseButton = new System.Windows.Forms.Button();
            this.mWindowSubtitleLabel = new System.Windows.Forms.Label();
            this.mWindowTitleLabel = new System.Windows.Forms.Label();
            this.mAppIconBox = new System.Windows.Forms.PictureBox();
            this.mGeneralPanel = new NetworkDiagram.SurfacePanel();
            this.mCheckBoxRunOnStartup = new System.Windows.Forms.CheckBox();
            this.mCheckBoxAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.mAppearancePanel = new NetworkDiagram.SurfacePanel();
            this.label2 = new System.Windows.Forms.Label();
            this.mOpacityValueLabel = new System.Windows.Forms.Label();
            this.TrackBarOpacity = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.mSentColorLabel = new System.Windows.Forms.Label();
            this.mSentColorPickButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.mReceivedColorLabel = new System.Windows.Forms.Label();
            this.mReceivedColorPickButton = new System.Windows.Forms.Button();
            this.mColorDialog = new System.Windows.Forms.ColorDialog();
            this.mLanguageMenu.SuspendLayout();
            this.mTitleBarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAppIconBox)).BeginInit();
            this.mGeneralPanel.SuspendLayout();
            this.mAppearancePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarOpacity)).BeginInit();
            this.SuspendLayout();
            // 
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
            this.mTitleBarPanel.Controls.Add(this.mLanguageButton);
            this.mTitleBarPanel.Controls.Add(this.mThemeButton);
            this.mTitleBarPanel.Controls.Add(this.mCloseButton);
            this.mTitleBarPanel.Controls.Add(this.mWindowSubtitleLabel);
            this.mTitleBarPanel.Controls.Add(this.mWindowTitleLabel);
            this.mTitleBarPanel.Controls.Add(this.mAppIconBox);
            this.mTitleBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mTitleBarPanel.Location = new System.Drawing.Point(1, 1);
            this.mTitleBarPanel.Name = "mTitleBarPanel";
            this.mTitleBarPanel.Size = new System.Drawing.Size(418, 56);
            this.mTitleBarPanel.TabIndex = 0;
            this.mTitleBarPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            // 
            // mLanguageButton
            // 
            this.mLanguageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mLanguageButton.Location = new System.Drawing.Point(274, 4);
            this.mLanguageButton.Name = "mLanguageButton";
            this.mLanguageButton.Size = new System.Drawing.Size(48, 48);
            this.mLanguageButton.TabIndex = 3;
            this.mLanguageButton.UseVisualStyleBackColor = true;
            this.mLanguageButton.Click += new System.EventHandler(this.LanguageButton_Click);
            // 
            // mThemeButton
            // 
            this.mThemeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mThemeButton.Location = new System.Drawing.Point(322, 4);
            this.mThemeButton.Name = "mThemeButton";
            this.mThemeButton.Size = new System.Drawing.Size(48, 48);
            this.mThemeButton.TabIndex = 4;
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
            this.mCloseButton.TabIndex = 5;
            this.mCloseButton.Text = "✕";
            this.mCloseButton.UseVisualStyleBackColor = true;
            this.mCloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // mWindowSubtitleLabel
            // 
            this.mWindowSubtitleLabel.AutoSize = true;
            this.mWindowSubtitleLabel.Location = new System.Drawing.Point(68, 31);
            this.mWindowSubtitleLabel.Name = "mWindowSubtitleLabel";
            this.mWindowSubtitleLabel.Size = new System.Drawing.Size(50, 15);
            this.mWindowSubtitleLabel.TabIndex = 2;
            this.mWindowSubtitleLabel.Text = "Settings";
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
            // mGeneralPanel
            // 
            this.mGeneralPanel.Controls.Add(this.mCheckBoxRunOnStartup);
            this.mGeneralPanel.Controls.Add(this.mCheckBoxAlwaysOnTop);
            this.mGeneralPanel.CornerRadius = 0;
            this.mGeneralPanel.Location = new System.Drawing.Point(13, 67);
            this.mGeneralPanel.Name = "mGeneralPanel";
            this.mGeneralPanel.Size = new System.Drawing.Size(394, 74);
            this.mGeneralPanel.TabIndex = 1;
            // 
            // mCheckBoxRunOnStartup
            // 
            this.mCheckBoxRunOnStartup.AutoSize = true;
            this.mCheckBoxRunOnStartup.Location = new System.Drawing.Point(16, 16);
            this.mCheckBoxRunOnStartup.Name = "mCheckBoxRunOnStartup";
            this.mCheckBoxRunOnStartup.Size = new System.Drawing.Size(145, 19);
            this.mCheckBoxRunOnStartup.TabIndex = 0;
            this.mCheckBoxRunOnStartup.Text = "Run on system startup";
            this.mCheckBoxRunOnStartup.UseVisualStyleBackColor = true;
            this.mCheckBoxRunOnStartup.CheckedChanged += new System.EventHandler(this.mCheckBoxRunOnStartup_CheckedChanged);
            // 
            // mCheckBoxAlwaysOnTop
            // 
            this.mCheckBoxAlwaysOnTop.AutoSize = true;
            this.mCheckBoxAlwaysOnTop.Location = new System.Drawing.Point(16, 44);
            this.mCheckBoxAlwaysOnTop.Name = "mCheckBoxAlwaysOnTop";
            this.mCheckBoxAlwaysOnTop.Size = new System.Drawing.Size(156, 19);
            this.mCheckBoxAlwaysOnTop.TabIndex = 1;
            this.mCheckBoxAlwaysOnTop.Text = "Keep diagram above all";
            this.mCheckBoxAlwaysOnTop.UseVisualStyleBackColor = true;
            this.mCheckBoxAlwaysOnTop.CheckedChanged += new System.EventHandler(this.mCheckBoxAlwaysOnTop_CheckedChanged);
            // 
            // mAppearancePanel
            // 
            this.mAppearancePanel.Controls.Add(this.label2);
            this.mAppearancePanel.Controls.Add(this.mOpacityValueLabel);
            this.mAppearancePanel.Controls.Add(this.TrackBarOpacity);
            this.mAppearancePanel.Controls.Add(this.label3);
            this.mAppearancePanel.Controls.Add(this.mSentColorLabel);
            this.mAppearancePanel.Controls.Add(this.mSentColorPickButton);
            this.mAppearancePanel.Controls.Add(this.label5);
            this.mAppearancePanel.Controls.Add(this.mReceivedColorLabel);
            this.mAppearancePanel.Controls.Add(this.mReceivedColorPickButton);
            this.mAppearancePanel.CornerRadius = 0;
            this.mAppearancePanel.Location = new System.Drawing.Point(13, 153);
            this.mAppearancePanel.Name = "mAppearancePanel";
            this.mAppearancePanel.Size = new System.Drawing.Size(394, 140);
            this.mAppearancePanel.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Window opacity";
            // 
            // mOpacityValueLabel
            // 
            this.mOpacityValueLabel.Location = new System.Drawing.Point(322, 18);
            this.mOpacityValueLabel.Name = "mOpacityValueLabel";
            this.mOpacityValueLabel.Size = new System.Drawing.Size(56, 15);
            this.mOpacityValueLabel.TabIndex = 1;
            this.mOpacityValueLabel.Text = "100%";
            this.mOpacityValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TrackBarOpacity
            // 
            this.TrackBarOpacity.AutoSize = false;
            this.TrackBarOpacity.LargeChange = 1;
            this.TrackBarOpacity.Location = new System.Drawing.Point(16, 40);
            this.TrackBarOpacity.Maximum = 100;
            this.TrackBarOpacity.Minimum = 10;
            this.TrackBarOpacity.Name = "TrackBarOpacity";
            this.TrackBarOpacity.Size = new System.Drawing.Size(362, 28);
            this.TrackBarOpacity.TabIndex = 2;
            this.TrackBarOpacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBarOpacity.Value = 100;
            this.TrackBarOpacity.ValueChanged += new System.EventHandler(this.TrackBarOpacity_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Sent color";
            // 
            // mSentColorLabel
            // 
            this.mSentColorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mSentColorLabel.Location = new System.Drawing.Point(212, 74);
            this.mSentColorLabel.Name = "mSentColorLabel";
            this.mSentColorLabel.Size = new System.Drawing.Size(94, 23);
            this.mSentColorLabel.TabIndex = 4;
            this.mSentColorLabel.Text = "#F0D0F0";
            this.mSentColorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mSentColorLabel.TextChanged += new System.EventHandler(this.mSentColorLabel_TextChanged);
            // 
            // mSentColorPickButton
            // 
            this.mSentColorPickButton.Location = new System.Drawing.Point(312, 73);
            this.mSentColorPickButton.Name = "mSentColorPickButton";
            this.mSentColorPickButton.Size = new System.Drawing.Size(66, 24);
            this.mSentColorPickButton.TabIndex = 5;
            this.mSentColorPickButton.Text = "Pick";
            this.mSentColorPickButton.UseVisualStyleBackColor = true;
            this.mSentColorPickButton.Click += new System.EventHandler(this.mSentColorPickButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Received color";
            // 
            // mReceivedColorLabel
            // 
            this.mReceivedColorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mReceivedColorLabel.Location = new System.Drawing.Point(212, 102);
            this.mReceivedColorLabel.Name = "mReceivedColorLabel";
            this.mReceivedColorLabel.Size = new System.Drawing.Size(94, 23);
            this.mReceivedColorLabel.TabIndex = 7;
            this.mReceivedColorLabel.Text = "#C0F0F0";
            this.mReceivedColorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mReceivedColorLabel.TextChanged += new System.EventHandler(this.mReceivedColorLabel_TextChanged);
            // 
            // mReceivedColorPickButton
            // 
            this.mReceivedColorPickButton.Location = new System.Drawing.Point(312, 101);
            this.mReceivedColorPickButton.Name = "mReceivedColorPickButton";
            this.mReceivedColorPickButton.Size = new System.Drawing.Size(66, 24);
            this.mReceivedColorPickButton.TabIndex = 8;
            this.mReceivedColorPickButton.Text = "Pick";
            this.mReceivedColorPickButton.UseVisualStyleBackColor = true;
            this.mReceivedColorPickButton.Click += new System.EventHandler(this.mReceivedColorPickButton_Click);
            // 
            // mColorDialog
            // 
            this.mColorDialog.AnyColor = true;
            this.mColorDialog.FullOpen = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 306);
            this.Controls.Add(this.mAppearancePanel);
            this.Controls.Add(this.mGeneralPanel);
            this.Controls.Add(this.mTitleBarPanel);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Move += new System.EventHandler(this.SettingsForm_Move);
            this.mLanguageMenu.ResumeLayout(false);
            this.mTitleBarPanel.ResumeLayout(false);
            this.mTitleBarPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAppIconBox)).EndInit();
            this.mGeneralPanel.ResumeLayout(false);
            this.mGeneralPanel.PerformLayout();
            this.mAppearancePanel.ResumeLayout(false);
            this.mAppearancePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarOpacity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip mToolTip;
        private System.Windows.Forms.ContextMenuStrip mLanguageMenu;
        private System.Windows.Forms.ToolStripMenuItem mLanguageMenuItemEnglish;
        private System.Windows.Forms.ToolStripMenuItem mLanguageMenuItemRussian;
        private System.Windows.Forms.Panel mTitleBarPanel;
        private System.Windows.Forms.Button mLanguageButton;
        private System.Windows.Forms.Button mThemeButton;
        private System.Windows.Forms.Button mCloseButton;
        private System.Windows.Forms.Label mWindowSubtitleLabel;
        private System.Windows.Forms.Label mWindowTitleLabel;
        private System.Windows.Forms.PictureBox mAppIconBox;
        private SurfacePanel mGeneralPanel;
        private System.Windows.Forms.CheckBox mCheckBoxRunOnStartup;
        private System.Windows.Forms.CheckBox mCheckBoxAlwaysOnTop;
        private SurfacePanel mAppearancePanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label mOpacityValueLabel;
        private System.Windows.Forms.TrackBar TrackBarOpacity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label mSentColorLabel;
        private System.Windows.Forms.Button mSentColorPickButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label mReceivedColorLabel;
        private System.Windows.Forms.Button mReceivedColorPickButton;
        private System.Windows.Forms.ColorDialog mColorDialog;
    }
}
