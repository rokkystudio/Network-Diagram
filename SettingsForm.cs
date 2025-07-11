using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public partial class SettingsForm : Form
    {
        private bool mRunOnStartup
        {
            get {
                return mCheckBoxRunOnStartup.Checked;
            }

            set {
                mCheckBoxRunOnStartup.CheckedChanged -= mCheckBoxRunOnStartup_CheckedChanged;
                mCheckBoxRunOnStartup.Checked = value;
                mCheckBoxRunOnStartup.CheckedChanged += mCheckBoxRunOnStartup_CheckedChanged;
            }
        }

        private bool mAlwaysOnTop
        {
            get {
                return mCheckBoxAlwaysOnTop.Checked;
            }

            set
            {
                mCheckBoxAlwaysOnTop.CheckedChanged -= mCheckBoxAlwaysOnTop_CheckedChanged;
                mCheckBoxAlwaysOnTop.Checked = value;
                mCheckBoxAlwaysOnTop.CheckedChanged += mCheckBoxAlwaysOnTop_CheckedChanged;
            }
        }

        private NetworkAdapter mSelectedAdapter
        {
            get {
                return (NetworkAdapter) mAdaptersComboBox.SelectedItem;
            }
            
            set {
                mAdaptersComboBox.SelectedIndexChanged -= mAdaptersComboBox_SelectedIndexChanged;
                mAdaptersComboBox.SelectedItem = value;
                mAdaptersComboBox.SelectedIndexChanged += mAdaptersComboBox_SelectedIndexChanged;
            }
        }

        private int mSelectedAdapterIndex
        {
            get {
                return mAdaptersComboBox.SelectedIndex;
            }
            
            set {
                mAdaptersComboBox.SelectedIndexChanged -= mAdaptersComboBox_SelectedIndexChanged;
                mAdaptersComboBox.SelectedIndex = value;
                mAdaptersComboBox.SelectedIndexChanged += mAdaptersComboBox_SelectedIndexChanged;
            }
        }

        private String mSentColor
        {
            get {
                return mSentColorLabel.Text;
            }

            set {
                Color color = Tools.HexToColor(value);
                mSentColorLabel.BackColor = color;
                mSentColorLabel.TextChanged -= mSentColorLabel_TextChanged;
                mSentColorLabel.Text = value;
                mSentColorLabel.TextChanged += mSentColorLabel_TextChanged;
            }
        }

        private String mReceivedColor
        {
            get {
                return mReceivedColorLabel.Text;
            }

            set {
                Color color = Tools.HexToColor(value);
                mReceivedColorLabel.BackColor = color;
                mReceivedColorLabel.TextChanged -= mReceivedColorLabel_TextChanged;
                mReceivedColorLabel.Text = value;
                mReceivedColorLabel.TextChanged += mReceivedColorLabel_TextChanged;
            }
        }

        public SettingsForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.ApplicationIcon;

            setTrackBarOpacity(Properties.Settings.Default.Opacity);
            Opacity = Properties.Settings.Default.Opacity;

            mRunOnStartup = Properties.Settings.Default.RunOnStartup;

            mAlwaysOnTop = Properties.Settings.Default.AlwaysOnTop;
            TopMost = Properties.Settings.Default.AlwaysOnTop;

            mSentColor = Properties.Settings.Default.SentColor;
            mReceivedColor = Properties.Settings.Default.ReceivedColor;

            Properties.Settings.Default.PropertyChanged += PropertyChanged;

            mAdaptersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            NetworkAdapter.UpdateAdapters(mAdaptersComboBox);

            // Select active adapter from properties
            SelectAdapterByID(Properties.Settings.Default.ActiveAdapter);

            // Select first adapter if not selected
            if (mAdaptersComboBox.Items.Count > 0 && mSelectedAdapterIndex == -1) {
                mSelectedAdapterIndex = 0;
            }
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveAdapter") {
                SelectAdapterByID(Properties.Settings.Default.ActiveAdapter);
            }
        }

        private void SelectAdapterByID(String id)
        {
            foreach (NetworkAdapter adapter in mAdaptersComboBox.Items)
            {
                if (adapter.Id == id) {
                    mSelectedAdapter = adapter;
                    break;
                }
            }
        }

        private void TrackBarOpacity_ValueChanged(object sender, EventArgs e) {
            float opacity = getTrackBarOpacity();
            Properties.Settings.Default.Opacity = opacity;
            Properties.Settings.Default.Save();
            Opacity = opacity;
        }

        private float getTrackBarOpacity() {
            float result = TrackBarOpacity.Value / 10f;
            if (result < 0.1f) result = 0.1f;
            if (result > 1f) result = 1f;
            return result; 
        }

        private void setTrackBarOpacity(float value) {
            if (value < 0.1f) value = 0.1f;
            if (value > 1f) value = 1f;
            TrackBarOpacity.Value = (int) (value * 10);
        }

        private void mAdaptersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mSelectedAdapter == null) return;
            Properties.Settings.Default.ActiveAdapter = mSelectedAdapter.Id;
            Properties.Settings.Default.Save();
        }

        private void AdaptersTimer_Tick(object sender, EventArgs e) {
            NetworkAdapter.UpdateAdapters(mAdaptersComboBox);
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
                mSentColorLabel.BackColor = mColorDialog.Color;
                mSentColorLabel.Text = Tools.ColorToHex(mColorDialog.Color);
            }
        }

        private void mReceivedColorPickButton_Click(object sender, EventArgs e)
        {
            if (mColorDialog.ShowDialog() == DialogResult.OK) {
                mReceivedColorLabel.BackColor = mColorDialog.Color;
                mReceivedColorLabel.Text = Tools.ColorToHex(mColorDialog.Color);
            }
        }

        private void mSentColorLabel_TextChanged(object sender, EventArgs e)
        {
            Color color = Tools.HexToColor(mSentColorLabel.Text);
            if (color == Color.Empty) return;

            mSentColorLabel.BackColor = color;
            Properties.Settings.Default.SentColor = mSentColorLabel.Text;
            Properties.Settings.Default.Save();
        }

        private void mReceivedColorLabel_TextChanged(object sender, EventArgs e)
        {
            Color color = Tools.HexToColor(mReceivedColorLabel.Text);
            if (color == Color.Empty) return;

            mReceivedColorLabel.BackColor = color;
            Properties.Settings.Default.ReceivedColor = mReceivedColorLabel.Text;
            Properties.Settings.Default.Save();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
