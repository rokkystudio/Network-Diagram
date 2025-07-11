using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NetworkDiagram
{
    // TODO 
    // Диаграмма рисуется кусками, крайние точки
    // Диаграмма масштабируется рывками
    // Диаграмма нет легенды
    // Диаграмма может выводить скорость сама
    // Изменение размера окна должно изменять размеры компонентов
    // Перемещение окна по клику на любой компонент

    // Выбор иконки в системном трее
    // Диаграмма в иконке системного трея

    public partial class MainForm : Form
    {
        private const int DIAGRAM_RECEIVED = 0;
        private const int DIAGRAM_SENT = 1;
        private bool mCloseApplication = false;

        private bool mCompactMode = false;
        private NotifyManager mNotifyManager;

        private NetworkAdapter mSelectedAdapter
        {
            get {
                return (NetworkAdapter) mAdaptersComboBox.SelectedItem;
            }

            set {
                mAdaptersComboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                mAdaptersComboBox.SelectedItem = value;
                mAdaptersComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        private int mSelectedAdapterIndex
        {
            get {
                return mAdaptersComboBox.SelectedIndex;
            }

            set {
                mAdaptersComboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                mAdaptersComboBox.SelectedIndex = value;
                mAdaptersComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.ApplicationIcon;
            mNotifyIcon.Icon = Properties.Resources.ApplicationIcon;
            mNotifyManager = new NotifyManager(mNotifyIcon);

            int posX = Properties.Settings.Default.WindowPosX;
            if (posX >= 0) Left = posX;

            int posY = Properties.Settings.Default.WindowPosY;
            if (posY >= 0) Top = posY;

            setOpacity(Properties.Settings.Default.Opacity);
            StartupEnableClassic(Properties.Settings.Default.RunOnStartup);

            UpdateSentColor();
            UpdateReceivedColor();

            Properties.Settings.Default.PropertyChanged += PropertyChanged;

            mAdaptersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            NetworkAdapter.UpdateAdapters(mAdaptersComboBox);

            // Restore last active adapter
            // Select active adapter from properties
            SelectAdapterByID(Properties.Settings.Default.ActiveAdapter);

            // Select first adapter if not selected
            if (mAdaptersComboBox.Items.Count > 0 && mSelectedAdapterIndex == -1) {
                mSelectedAdapterIndex = 0;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!mCloseApplication) {
                e.Cancel = true;
                Hide();
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mSelectedAdapter == null) return;
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
        }

        private void UpdateSentColor() {
            Color color = Tools.HexToColor(Properties.Settings.Default.SentColor);
            if (color != Color.Empty) mDiagramBox.SetColor(DIAGRAM_SENT, color);
        }

        private void UpdateReceivedColor() {
            Color color = Tools.HexToColor(Properties.Settings.Default.ReceivedColor);
            if (color != Color.Empty) mDiagramBox.SetColor(DIAGRAM_RECEIVED, color);
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

        private void SpeedTimer_Tick(object sender, EventArgs e)
        {
            if (mSelectedAdapter == null) return;

            int sentSpeed = mSelectedAdapter.GetSentCount() / mSpeedTimer.Interval * 1000;
            mSentSpeedLabel.Text = SpeedToString(sentSpeed);
            mDiagramBox.AddValue(DIAGRAM_SENT, sentSpeed);

            int receivedSpeed = mSelectedAdapter.GetReceivedCount() / mSpeedTimer.Interval * 1000;
            mReceivedSpeedLabel.Text = SpeedToString(receivedSpeed);
            mDiagramBox.AddValue(DIAGRAM_RECEIVED, receivedSpeed);

            int maxValue = mDiagramBox.GetMaxValue();
            if (maxValue < 1) maxValue = 1;

            int sentThroughput = sentSpeed * 100 / maxValue;
            int receivedThroughput = receivedSpeed * 100 / maxValue;

            // TODO Properties -> style
            int style = NotifyManager.DARK_ARROWS;
            mNotifyManager.DrawIcon(style, sentThroughput, receivedThroughput);
        }

        private String SpeedToString(long bytes)
        {
            if (bytes > 0 && bytes < 1024) bytes = 1024;
            if (bytes == 0) return "0 KB";

            string[] sizes = { "", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1) {
                order++;
                bytes /= 1024;
            }

            return String.Format("{0:0.##} {1}", bytes, sizes[order]);
        }

        private void AdaptersTimer_Tick(object sender, EventArgs e) { 
            NetworkAdapter.UpdateAdapters(mAdaptersComboBox);
        }

        public void OnAnimationStart() {
            throw new NotImplementedException();
        }

        public void OnAnimationFinish() {
            throw new NotImplementedException();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e) {
            showDiagram();
        }

        private void MainForm_Resize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) Hide();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized) {
                Properties.Settings.Default.WindowPosX = Left;
                Properties.Settings.Default.WindowPosY = Top;
                Properties.Settings.Default.Save();
            }
        }

        private void MainMenuItemSettings_Click(object sender, EventArgs e) {
            showSettings();
        }

        private void MainMenuItemExit_Click(object sender, EventArgs e) {
            closeApplication();
        }

        private void MainMenuItemCompact_Click(object sender, EventArgs e) {
            setCompactMode(true);
        }

        private void NotifyMenuItemOpen_Click(object sender, EventArgs e) {
            showDiagram();
        }

        private void NotifyMenuItemSettings_Click(object sender, EventArgs e) {
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

        private void NotifyMenuItemExit_Click(object sender, EventArgs e) {
            closeApplication();
        }

        private void showDiagram() {
            WindowState = FormWindowState.Normal;
            BringToFront();
            Show();
        }

        private void showSettings() {
            new SettingsForm().Show();
        }

        private void closeApplication() {
            mCloseApplication = true;
            Close();
        }

        private void setOpacity(float value) {
            if (value > 0 && value <= 1f) Opacity = value;
        }

        private void SpeedDiagram_MouseDoubleClick(object sender, MouseEventArgs e) {
            setCompactMode(!mCompactMode);
        }

        private void setCompactMode(bool enable)
        {
            if (enable) {
                mMainMenu.Hide();
                Panel.Hide();
                mCompactMode = true;
            } else {
                mMainMenu.Show();
                Panel.Show();
                mCompactMode = false;
            }
        }

        public void StartupEnableClassic(bool enabled)
        {
            String name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            String path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);
            if (enabled) {
                key.SetValue(name, '"' + Application.ExecutablePath + '"');
            }
            else {
                key.DeleteValue(name);
            }
        }
    }
}
