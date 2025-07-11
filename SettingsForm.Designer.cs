
namespace NetworkDiagram
{
    partial class SettingsForm
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
            this.mAdaptersComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mCheckBoxAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.TrackBarOpacity = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.mColorDialog = new System.Windows.Forms.ColorDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.mSentColorPickButton = new System.Windows.Forms.Button();
            this.mSentColorLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mReceivedColorLabel = new System.Windows.Forms.Label();
            this.mReceivedColorPickButton = new System.Windows.Forms.Button();
            this.mAdaptersTimer = new System.Windows.Forms.Timer(this.components);
            this.mCheckBoxRunOnStartup = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // mAdaptersComboBox
            // 
            this.mAdaptersComboBox.FormattingEnabled = true;
            this.mAdaptersComboBox.Location = new System.Drawing.Point(12, 65);
            this.mAdaptersComboBox.Name = "mAdaptersComboBox";
            this.mAdaptersComboBox.Size = new System.Drawing.Size(300, 21);
            this.mAdaptersComboBox.TabIndex = 21;
            this.mAdaptersComboBox.TabStop = false;
            this.mAdaptersComboBox.SelectedIndexChanged += new System.EventHandler(this.mAdaptersComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Active network adapter:";
            // 
            // mCheckBoxAlwaysOnTop
            // 
            this.mCheckBoxAlwaysOnTop.AutoSize = true;
            this.mCheckBoxAlwaysOnTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mCheckBoxAlwaysOnTop.Location = new System.Drawing.Point(12, 99);
            this.mCheckBoxAlwaysOnTop.Name = "mCheckBoxAlwaysOnTop";
            this.mCheckBoxAlwaysOnTop.Size = new System.Drawing.Size(200, 17);
            this.mCheckBoxAlwaysOnTop.TabIndex = 4;
            this.mCheckBoxAlwaysOnTop.Text = "Diagram window always on top";
            this.mCheckBoxAlwaysOnTop.UseVisualStyleBackColor = true;
            this.mCheckBoxAlwaysOnTop.CheckedChanged += new System.EventHandler(this.mCheckBoxAlwaysOnTop_CheckedChanged);
            // 
            // TrackBarOpacity
            // 
            this.TrackBarOpacity.AutoSize = false;
            this.TrackBarOpacity.LargeChange = 1;
            this.TrackBarOpacity.Location = new System.Drawing.Point(130, 130);
            this.TrackBarOpacity.Minimum = 1;
            this.TrackBarOpacity.Name = "TrackBarOpacity";
            this.TrackBarOpacity.Size = new System.Drawing.Size(182, 38);
            this.TrackBarOpacity.TabIndex = 5;
            this.TrackBarOpacity.Value = 10;
            this.TrackBarOpacity.ValueChanged += new System.EventHandler(this.TrackBarOpacity_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(9, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Window opacity:";
            // 
            // mColorDialog
            // 
            this.mColorDialog.AnyColor = true;
            this.mColorDialog.FullOpen = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(9, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Sent Color:";
            // 
            // mSentColorPickButton
            // 
            this.mSentColorPickButton.BackColor = System.Drawing.SystemColors.Control;
            this.mSentColorPickButton.Location = new System.Drawing.Point(274, 174);
            this.mSentColorPickButton.Name = "mSentColorPickButton";
            this.mSentColorPickButton.Size = new System.Drawing.Size(27, 23);
            this.mSentColorPickButton.TabIndex = 23;
            this.mSentColorPickButton.UseVisualStyleBackColor = false;
            this.mSentColorPickButton.Click += new System.EventHandler(this.mSentColorPickButton_Click);
            // 
            // mSentColorLabel
            // 
            this.mSentColorLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.mSentColorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mSentColorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mSentColorLabel.ForeColor = System.Drawing.Color.Black;
            this.mSentColorLabel.Location = new System.Drawing.Point(145, 174);
            this.mSentColorLabel.Name = "mSentColorLabel";
            this.mSentColorLabel.Size = new System.Drawing.Size(119, 21);
            this.mSentColorLabel.TabIndex = 25;
            this.mSentColorLabel.Text = "#F0D0F0";
            this.mSentColorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mSentColorLabel.TextChanged += new System.EventHandler(this.mSentColorLabel_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(9, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Received Color:";
            // 
            // mReceivedColorLabel
            // 
            this.mReceivedColorLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.mReceivedColorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mReceivedColorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mReceivedColorLabel.ForeColor = System.Drawing.Color.Black;
            this.mReceivedColorLabel.Location = new System.Drawing.Point(145, 213);
            this.mReceivedColorLabel.Name = "mReceivedColorLabel";
            this.mReceivedColorLabel.Size = new System.Drawing.Size(120, 21);
            this.mReceivedColorLabel.TabIndex = 27;
            this.mReceivedColorLabel.Text = "#C0F0F0";
            this.mReceivedColorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mReceivedColorLabel.TextChanged += new System.EventHandler(this.mReceivedColorLabel_TextChanged);
            // 
            // mReceivedColorPickButton
            // 
            this.mReceivedColorPickButton.BackColor = System.Drawing.SystemColors.Control;
            this.mReceivedColorPickButton.Location = new System.Drawing.Point(274, 212);
            this.mReceivedColorPickButton.Name = "mReceivedColorPickButton";
            this.mReceivedColorPickButton.Size = new System.Drawing.Size(27, 23);
            this.mReceivedColorPickButton.TabIndex = 28;
            this.mReceivedColorPickButton.UseVisualStyleBackColor = false;
            this.mReceivedColorPickButton.Click += new System.EventHandler(this.mReceivedColorPickButton_Click);
            // 
            // mAdaptersTimer
            // 
            this.mAdaptersTimer.Enabled = true;
            this.mAdaptersTimer.Interval = 5000;
            this.mAdaptersTimer.Tick += new System.EventHandler(this.AdaptersTimer_Tick);
            // 
            // mCheckBoxRunOnStartup
            // 
            this.mCheckBoxRunOnStartup.AutoSize = true;
            this.mCheckBoxRunOnStartup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mCheckBoxRunOnStartup.Location = new System.Drawing.Point(12, 12);
            this.mCheckBoxRunOnStartup.Name = "mCheckBoxRunOnStartup";
            this.mCheckBoxRunOnStartup.Size = new System.Drawing.Size(218, 17);
            this.mCheckBoxRunOnStartup.TabIndex = 29;
            this.mCheckBoxRunOnStartup.Text = "Run application on system startup";
            this.mCheckBoxRunOnStartup.UseVisualStyleBackColor = true;
            this.mCheckBoxRunOnStartup.CheckedChanged += new System.EventHandler(this.mCheckBoxRunOnStartup_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 450);
            this.Controls.Add(this.mCheckBoxRunOnStartup);
            this.Controls.Add(this.mReceivedColorPickButton);
            this.Controls.Add(this.mReceivedColorLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.mSentColorLabel);
            this.Controls.Add(this.mSentColorPickButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TrackBarOpacity);
            this.Controls.Add(this.mCheckBoxAlwaysOnTop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mAdaptersComboBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarOpacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox mAdaptersComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox mCheckBoxAlwaysOnTop;
        private System.Windows.Forms.TrackBar TrackBarOpacity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog mColorDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button mSentColorPickButton;
        private System.Windows.Forms.Label mSentColorLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label mReceivedColorLabel;
        private System.Windows.Forms.Button mReceivedColorPickButton;
        private System.Windows.Forms.Timer mAdaptersTimer;
        private System.Windows.Forms.CheckBox mCheckBoxRunOnStartup;
    }
}