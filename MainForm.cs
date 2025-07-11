using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NetworkDiagram
{
    // Главное окно приложения, отвечающее за отображение сетевой активности,
    // управление настройками, взаимодействие с системным треем и обработку таймеров.
    public partial class MainForm : Form
    {
        // Индекс графика приёма данных
        private const int DIAGRAM_RECEIVED = 0;

        // Индекс графика отправки данных
        private const int DIAGRAM_SENT = 1;

        // Флаг, указывающий, нужно ли завершить приложение при закрытии формы
        private bool mCloseApplication = false;

        // Включён ли компактный режим интерфейса
        private bool mCompactMode = false;

        // Менеджер иконки в трее
        private NotifyManager mNotifyManager;


        // Текущий выбранный сетевой адаптер
        // Обновляет ComboBox без срабатывания события
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

        // Индекс выбранного адаптера в списке
        // Обновляет ComboBox безопасно (без триггера событий)
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

        // Конструктор формы. Инициализирует интерфейс.
        // Загружает и применяет сохранённые настройки.
        // Устанавливает иконку, обновляет цвета, адаптеры.
        // Запоминает последнюю выбранную сетевую карту.
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

        // Переопределён для исключения окна из ALT+TAB
        protected override CreateParams CreateParams
        {
            get {
                CreateParams cp = base.CreateParams;

                // Убираем окно из ALT+TAB: делаем его "вспомогательным"
                cp.ExStyle |= 0x80; // WS_EX_TOOLWINDOW
                return cp;
            }
        }

        // Отменяет закрытие окна, если приложение не завершается полностью, и скрывает окно в трей.
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!mCloseApplication) {
                e.Cancel = true;
                Hide();
            }
        }

        // Сохраняет выбранный адаптер в настройки.
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mSelectedAdapter == null) return;
            Properties.Settings.Default.ActiveAdapter = mSelectedAdapter.Id;
            Properties.Settings.Default.Save();
        }

        // Отслеживает изменения пользовательских настроек и применяет их на лету.
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

        // Устанавливают цвет графика отправки данных на основе сохранённых настроек.
        private void UpdateSentColor() {
            Color color = Tools.HexToColor(Properties.Settings.Default.SentColor);
            if (color != Color.Empty) mDiagramBox.SetColor(DIAGRAM_SENT, color);
        }

        // Устанавливают цвет графика приёма данных на основе сохранённых настроек.
        private void UpdateReceivedColor() {
            Color color = Tools.HexToColor(Properties.Settings.Default.ReceivedColor);
            if (color != Color.Empty) mDiagramBox.SetColor(DIAGRAM_RECEIVED, color);
        }

        // Устанавливает адаптер в выпадающем списке по ID из настроек.
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

        // Вызывается по таймеру. Получает текущую скорость отправки/приёма,
        // обновляет график, текст и иконку в трее.
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

        // Преобразует байты в удобочитаемую строку (KB, MB, ...).
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

        // Периодически обновляет список сетевых адаптеров.
        private void AdaptersTimer_Tick(object sender, EventArgs e) { 
            NetworkAdapter.UpdateAdapters(mAdaptersComboBox);
        }

        // Заглушки под анимации (пока не реализованы).
        public void OnAnimationStart() {
            throw new NotImplementedException();
        }

        // Заглушки под анимации (пока не реализованы).
        public void OnAnimationFinish() {
            throw new NotImplementedException();
        }

        // Восстанавливает окно при двойном клике по иконке в трее.
        private void NotifyIcon_DoubleClick(object sender, EventArgs e) {
            showDiagram();
        }

        // Скрывает окно при сворачивании.
        private void MainForm_Resize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) Hide();
        }

        // Сохраняет позицию окна при перемещении.
        private void MainForm_Move(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized) {
                Properties.Settings.Default.WindowPosX = Left;
                Properties.Settings.Default.WindowPosY = Top;
                Properties.Settings.Default.Save();
            }
        }

        // Открывает окно настроек.
        private void MainMenuItemSettings_Click(object sender, EventArgs e) {
            showSettings();
        }

        // Закрывает приложение.
        private void MainMenuItemExit_Click(object sender, EventArgs e) {
            closeApplication();
        }

        // Переключает режимо отображения компактный / обычный.
        private void MainMenuItemCompact_Click(object sender, EventArgs e) {
            setCompactMode(true);
        }

        // Открывает окно приложения.
        private void NotifyMenuItemOpen_Click(object sender, EventArgs e) {
            showDiagram();
        }
        
        // Открывает окно настроек.
        private void NotifyMenuItemSettings_Click(object sender, EventArgs e) {
            showSettings();
        }

        // Сбрасывает положение окна по умолчанию.
        private void NotifyMenuItemReset_Click(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;
            showDiagram();

            Properties.Settings.Default.Opacity = 1;
            Properties.Settings.Default.Save();
        }

        // Закрывает приложение.
        private void NotifyMenuItemExit_Click(object sender, EventArgs e) {
            closeApplication();
        }

        // Показывает основное окно и делает его активным.
        private void showDiagram() {
            WindowState = FormWindowState.Normal;
            BringToFront();
            Show();
        }

        // Открывает окно настроек.
        private void showSettings() {
            new SettingsForm().Show();
        }

        // Закрывает приложение.
        private void closeApplication() {
            mCloseApplication = true;
            Close();
        }

        // Устанавливает прозрачность окон.
        private void setOpacity(float value) {
            if (value > 0 && value <= 1f) Opacity = value;
        }

        // Переключает в компактный режим по двойному клику на графике.
        private void SpeedDiagram_MouseDoubleClick(object sender, MouseEventArgs e) {
            setCompactMode(!mCompactMode);
        }

        // Переключает диаграмму в компактный режим
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

        // Добавляет или удаляет приложение из автозагрузки через реестр Windows.
        public void StartupEnableClassic(bool enabled)
        {
            String name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            String path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            using (var key = Registry.CurrentUser.OpenSubKey(path, true)) {
                if (key != null) {
                    if (enabled)
                        key.SetValue(name, '"' + Application.ExecutablePath + '"');
                    else
                        key.DeleteValue(name, false); // false = silent if not exists
                }
            }
        }
    }
}
