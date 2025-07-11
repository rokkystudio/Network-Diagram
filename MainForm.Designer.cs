
namespace NetworkDiagram
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mSpeedTimer = new System.Windows.Forms.Timer(this.components);
            this.mAdaptersTimer = new System.Windows.Forms.Timer(this.components);
            this.mNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mNotifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NotifyMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenuItemReset = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mMainMenu = new System.Windows.Forms.MenuStrip();
            this.mMainMenuItemCompact = new System.Windows.Forms.ToolStripMenuItem();
            this.mMainMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mMainMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mPanelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.Panel = new System.Windows.Forms.Panel();
            this.mReceivedTitleLabel = new System.Windows.Forms.Label();
            this.mSentTitleLabel = new System.Windows.Forms.Label();
            this.mReceivedSpeedLabel = new System.Windows.Forms.Label();
            this.mSentSpeedLabel = new System.Windows.Forms.Label();
            this.mAdaptersComboBox = new System.Windows.Forms.ComboBox();
            this.mPanelBottom = new System.Windows.Forms.Panel();
            this.mDiagramBox = new NetworkDiagram.DiagramBox();
            this.mNotifyMenu.SuspendLayout();
            this.mMainMenu.SuspendLayout();
            this.mPanelTop.SuspendLayout();
            this.Panel.SuspendLayout();
            this.mPanelBottom.SuspendLayout();
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
            this.mNotifyIcon.Text = "NotifyIcon";
            this.mNotifyIcon.Visible = true;
            this.mNotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // mNotifyMenu
            // 
            this.mNotifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NotifyMenuItemOpen,
            this.NotifyMenuItemSettings,
            this.NotifyMenuItemReset,
            this.NotifyMenuItemExit});
            this.mNotifyMenu.Name = "NotifyMenu";
            this.mNotifyMenu.Size = new System.Drawing.Size(117, 92);
            // 
            // NotifyMenuItemOpen
            // 
            this.NotifyMenuItemOpen.Name = "NotifyMenuItemOpen";
            this.NotifyMenuItemOpen.Size = new System.Drawing.Size(116, 22);
            this.NotifyMenuItemOpen.Text = "Open";
            this.NotifyMenuItemOpen.Click += new System.EventHandler(this.NotifyMenuItemOpen_Click);
            // 
            // NotifyMenuItemSettings
            // 
            this.NotifyMenuItemSettings.Name = "NotifyMenuItemSettings";
            this.NotifyMenuItemSettings.Size = new System.Drawing.Size(116, 22);
            this.NotifyMenuItemSettings.Text = "Settings";
            this.NotifyMenuItemSettings.Click += new System.EventHandler(this.NotifyMenuItemSettings_Click);
            // 
            // NotifyMenuItemReset
            // 
            this.NotifyMenuItemReset.Name = "NotifyMenuItemReset";
            this.NotifyMenuItemReset.Size = new System.Drawing.Size(116, 22);
            this.NotifyMenuItemReset.Text = "Reset";
            this.NotifyMenuItemReset.Click += new System.EventHandler(this.NotifyMenuItemReset_Click);
            // 
            // NotifyMenuItemExit
            // 
            this.NotifyMenuItemExit.Name = "NotifyMenuItemExit";
            this.NotifyMenuItemExit.Size = new System.Drawing.Size(116, 22);
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
            this.mMainMenu.Size = new System.Drawing.Size(314, 24);
            this.mMainMenu.TabIndex = 7;
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
            this.mMainMenuItemExit.Click += new System.EventHandler(this.MainMenuItemExit_Click);
            // 
            // mPanelTop
            // 
            this.mPanelTop.AutoSize = true;
            this.mPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mPanelTop.Controls.Add(this.Panel);
            this.mPanelTop.Controls.Add(this.mPanelBottom);
            this.mPanelTop.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mPanelTop.Location = new System.Drawing.Point(0, 26);
            this.mPanelTop.Name = "mPanelTop";
            this.mPanelTop.Size = new System.Drawing.Size(314, 215);
            this.mPanelTop.TabIndex = 8;
            // 
            // Panel
            // 
            this.Panel.BackColor = System.Drawing.SystemColors.Control;
            this.Panel.Controls.Add(this.mReceivedTitleLabel);
            this.Panel.Controls.Add(this.mSentTitleLabel);
            this.Panel.Controls.Add(this.mReceivedSpeedLabel);
            this.Panel.Controls.Add(this.mSentSpeedLabel);
            this.Panel.Controls.Add(this.mAdaptersComboBox);
            this.Panel.Location = new System.Drawing.Point(3, 3);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(308, 103);
            this.Panel.TabIndex = 0;
            // 
            // mReceivedTitleLabel
            // 
            this.mReceivedTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mReceivedTitleLabel.Location = new System.Drawing.Point(11, 71);
            this.mReceivedTitleLabel.Name = "mReceivedTitleLabel";
            this.mReceivedTitleLabel.Size = new System.Drawing.Size(70, 18);
            this.mReceivedTitleLabel.TabIndex = 10;
            this.mReceivedTitleLabel.Text = "Received:";
            this.mReceivedTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mSentTitleLabel
            // 
            this.mSentTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mSentTitleLabel.Location = new System.Drawing.Point(11, 42);
            this.mSentTitleLabel.Name = "mSentTitleLabel";
            this.mSentTitleLabel.Size = new System.Drawing.Size(70, 18);
            this.mSentTitleLabel.TabIndex = 9;
            this.mSentTitleLabel.Text = "Sent:";
            this.mSentTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mReceivedSpeedLabel
            // 
            this.mReceivedSpeedLabel.Location = new System.Drawing.Point(194, 71);
            this.mReceivedSpeedLabel.Name = "mReceivedSpeedLabel";
            this.mReceivedSpeedLabel.Size = new System.Drawing.Size(100, 18);
            this.mReceivedSpeedLabel.TabIndex = 8;
            this.mReceivedSpeedLabel.Text = "0";
            this.mReceivedSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mSentSpeedLabel
            // 
            this.mSentSpeedLabel.Location = new System.Drawing.Point(194, 42);
            this.mSentSpeedLabel.Name = "mSentSpeedLabel";
            this.mSentSpeedLabel.Size = new System.Drawing.Size(100, 18);
            this.mSentSpeedLabel.TabIndex = 7;
            this.mSentSpeedLabel.Text = "0";
            this.mSentSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mAdaptersComboBox
            // 
            this.mAdaptersComboBox.FormattingEnabled = true;
            this.mAdaptersComboBox.Location = new System.Drawing.Point(14, 11);
            this.mAdaptersComboBox.Name = "mAdaptersComboBox";
            this.mAdaptersComboBox.Size = new System.Drawing.Size(280, 21);
            this.mAdaptersComboBox.TabIndex = 6;
            this.mAdaptersComboBox.TabStop = false;
            this.mAdaptersComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // mPanelBottom
            // 
            this.mPanelBottom.Controls.Add(this.mDiagramBox);
            this.mPanelBottom.Location = new System.Drawing.Point(3, 112);
            this.mPanelBottom.Name = "mPanelBottom";
            this.mPanelBottom.Size = new System.Drawing.Size(308, 100);
            this.mPanelBottom.TabIndex = 8;
            // 
            // mDiagramBox
            // 
            this.mDiagramBox.BackColor = System.Drawing.Color.White;
            this.mDiagramBox.Location = new System.Drawing.Point(14, 9);
            this.mDiagramBox.Name = "mDiagramBox";
            this.mDiagramBox.Size = new System.Drawing.Size(280, 83);
            this.mDiagramBox.TabIndex = 8;
            this.mDiagramBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SpeedDiagram_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(314, 241);
            this.Controls.Add(this.mPanelTop);
            this.Controls.Add(this.mMainMenu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mMainMenu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Network Diagram";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mNotifyMenu.ResumeLayout(false);
            this.mMainMenu.ResumeLayout(false);
            this.mMainMenu.PerformLayout();
            this.mPanelTop.ResumeLayout(false);
            this.Panel.ResumeLayout(false);
            this.mPanelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer mSpeedTimer;
        private System.Windows.Forms.Timer mAdaptersTimer;
        private System.Windows.Forms.NotifyIcon mNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip mNotifyMenu;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemExit;
        private System.Windows.Forms.MenuStrip mMainMenu;
        private System.Windows.Forms.ToolStripMenuItem mMainMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem mMainMenuItemCompact;
        private System.Windows.Forms.ToolStripMenuItem mMainMenuItemExit;
        private System.Windows.Forms.FlowLayoutPanel mPanelTop;
        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.Label mReceivedTitleLabel;
        private System.Windows.Forms.Label mSentTitleLabel;
        private System.Windows.Forms.Label mReceivedSpeedLabel;
        private System.Windows.Forms.Label mSentSpeedLabel;
        private System.Windows.Forms.ComboBox mAdaptersComboBox;
        private System.Windows.Forms.Panel mPanelBottom;
        private DiagramBox mDiagramBox;
        private System.Windows.Forms.ToolStripMenuItem NotifyMenuItemReset;
    }
}

