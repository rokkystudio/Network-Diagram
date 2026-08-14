using NetworkDiagram.Core;
using NetworkDiagram.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace NetworkDiagram.ViewModels
{
    internal sealed class MainWindowViewModel : ObservableObject, IDisposable
    {
        private readonly DispatcherTimer mSpeedTimer;
        private readonly DispatcherTimer mAdaptersTimer;
        private NetworkAdapter mSelectedAdapter;
        private string mWindowTitleText;
        private string mWindowSubtitleText;
        private string mUploadTitleText;
        private string mDownloadTitleText;
        private string mUploadSpeedTitleText;
        private string mDownloadSpeedTitleText;
        private string mSettingsToolTipText;
        private string mHideToolTipText;
        private string mResetTotalsText;
        private string mSessionTotalsText;
        private string mUploadTotalText;
        private string mDownloadTotalText;
        private string mUploadSpeedText;
        private string mDownloadSpeedText;
        private bool mResetMode;
        private bool mDisposed;
        private readonly Dictionary<string, TrafficBaseline> mResetBaselines = new Dictionary<string, TrafficBaseline>();

        public MainWindowViewModel()
        {
            Adapters = new ObservableCollection<NetworkAdapter>();
            ResetTotalsCommand = new RelayCommand(delegate { ResetTotals(); }, delegate { return SelectedAdapter != null; });
            RestoreSessionTotalsCommand = new RelayCommand(delegate { RestoreSessionTotals(); }, delegate { return SelectedAdapter != null; });
            Settings.Default.PropertyChanged += Settings_PropertyChanged;
            LocalizationService.LanguageChanged += LocalizationService_LanguageChanged;

            mSpeedTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
            mSpeedTimer.Tick += SpeedTimer_Tick;
            mSpeedTimer.Start();

            mAdaptersTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            mAdaptersTimer.Tick += AdaptersTimer_Tick;
            mAdaptersTimer.Start();

            RefreshAdapterCollection();
            UpdateLocalizedText();
            SelectAdapterById(Settings.Default.ActiveAdapter);
            if (SelectedAdapter == null && Adapters.Count > 0) {
                SelectedAdapter = Adapters[0];
            }

            UpdateMetrics(0, 0);
        }

        public event Action<int, int> SpeedsUpdated;

        public ObservableCollection<NetworkAdapter> Adapters { get; }

        public ICommand ResetTotalsCommand { get; private set; }

        public ICommand RestoreSessionTotalsCommand { get; private set; }

        public NetworkAdapter SelectedAdapter
        {
            get { return mSelectedAdapter; }
            set
            {
                if (!SetProperty(ref mSelectedAdapter, value)) {
                    return;
                }

                Settings.Default.ActiveAdapter = value == null ? string.Empty : value.Id;
                Settings.Default.Save();
                UpdateMetrics(0, 0);
            }
        }

        public string WindowTitleText
        {
            get { return mWindowTitleText; }
            private set { SetProperty(ref mWindowTitleText, value); }
        }

        public string WindowSubtitleText
        {
            get { return mWindowSubtitleText; }
            private set { SetProperty(ref mWindowSubtitleText, value); }
        }

        public string UploadTitleText
        {
            get { return mUploadTitleText; }
            private set { SetProperty(ref mUploadTitleText, value); }
        }

        public string DownloadTitleText
        {
            get { return mDownloadTitleText; }
            private set { SetProperty(ref mDownloadTitleText, value); }
        }

        public string UploadSpeedTitleText
        {
            get { return mUploadSpeedTitleText; }
            private set { SetProperty(ref mUploadSpeedTitleText, value); }
        }

        public string DownloadSpeedTitleText
        {
            get { return mDownloadSpeedTitleText; }
            private set { SetProperty(ref mDownloadSpeedTitleText, value); }
        }

        public string SettingsToolTipText
        {
            get { return mSettingsToolTipText; }
            private set { SetProperty(ref mSettingsToolTipText, value); }
        }

        public string HideToolTipText
        {
            get { return mHideToolTipText; }
            private set { SetProperty(ref mHideToolTipText, value); }
        }

        public string ResetTotalsText
        {
            get { return mResetTotalsText; }
            private set { SetProperty(ref mResetTotalsText, value); }
        }

        public string SessionTotalsText
        {
            get { return mSessionTotalsText; }
            private set { SetProperty(ref mSessionTotalsText, value); }
        }

        public string UploadTotalText
        {
            get { return mUploadTotalText; }
            private set { SetProperty(ref mUploadTotalText, value); }
        }

        public string DownloadTotalText
        {
            get { return mDownloadTotalText; }
            private set { SetProperty(ref mDownloadTotalText, value); }
        }

        public string UploadSpeedText
        {
            get { return mUploadSpeedText; }
            private set { SetProperty(ref mUploadSpeedText, value); }
        }

        public string DownloadSpeedText
        {
            get { return mDownloadSpeedText; }
            private set { SetProperty(ref mDownloadSpeedText, value); }
        }

        public void Dispose()
        {
            if (mDisposed) {
                return;
            }

            mDisposed = true;
            Settings.Default.PropertyChanged -= Settings_PropertyChanged;
            LocalizationService.LanguageChanged -= LocalizationService_LanguageChanged;
            mSpeedTimer.Stop();
            mAdaptersTimer.Stop();
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveAdapter") {
                SelectAdapterById(Settings.Default.ActiveAdapter);
            }
        }

        private void LocalizationService_LanguageChanged(object sender, EventArgs e)
        {
            UpdateLocalizedText();
        }

        private void AdaptersTimer_Tick(object sender, EventArgs e)
        {
            RefreshAdapterCollection();
        }

        private void SpeedTimer_Tick(object sender, EventArgs e)
        {
            if (SelectedAdapter == null) {
                UpdateMetrics(0, 0);
                return;
            }

            int sentSpeed = SelectedAdapter.GetSentCount() / (int)mSpeedTimer.Interval.TotalMilliseconds * 1000;
            int receivedSpeed = SelectedAdapter.GetReceivedCount() / (int)mSpeedTimer.Interval.TotalMilliseconds * 1000;
            UpdateMetrics(sentSpeed, receivedSpeed);
        }

        private void UpdateMetrics(int sentSpeed, int receivedSpeed)
        {
            if (SelectedAdapter == null) {
                UploadTotalText = FormatBytes(0);
                DownloadTotalText = FormatBytes(0);
            } else {
                TrafficBaseline baseline = GetDisplayBaseline(SelectedAdapter);
                long sentTotal = Math.Max(0, SelectedAdapter.GetSentBytesAll() - baseline.SentBytes);
                long receivedTotal = Math.Max(0, SelectedAdapter.GetReceivedBytesAll() - baseline.ReceivedBytes);
                UploadTotalText = FormatBytes(sentTotal);
                DownloadTotalText = FormatBytes(receivedTotal);
            }

            UploadSpeedText = FormatRate(sentSpeed);
            DownloadSpeedText = FormatRate(receivedSpeed);
            SpeedsUpdated?.Invoke(sentSpeed, receivedSpeed);
        }

        private void UpdateLocalizedText()
        {
            WindowTitleText = LocalizationService.Text("app_name");
            WindowSubtitleText = LocalizationService.Text("app_subtitle");
            UploadTitleText = LocalizationService.Text("upload");
            DownloadTitleText = LocalizationService.Text("download");
            UploadSpeedTitleText = LocalizationService.Text("upload_speed");
            DownloadSpeedTitleText = LocalizationService.Text("download_speed");
            SettingsToolTipText = LocalizationService.Text("tooltip_settings");
            HideToolTipText = LocalizationService.Text("tooltip_hide");
            ResetTotalsText = LocalizationService.Text("reset");
            SessionTotalsText = LocalizationService.Text("current_session");
        }

        private void RefreshAdapterCollection()
        {
            List<NetworkAdapter> currentAdapters = NetworkAdapter.GetAdapters();
            HashSet<string> currentIds = new HashSet<string>(currentAdapters.Select(adapter => adapter.Id));

            foreach (NetworkAdapter adapter in currentAdapters) {
                    if (Adapters.All(existing => existing.Id != adapter.Id)) {
                    Adapters.Add(adapter);
                }
            }

            for (int index = Adapters.Count - 1; index >= 0; index--) {
                if (!currentIds.Contains(Adapters[index].Id)) {
                    if (ReferenceEquals(SelectedAdapter, Adapters[index])) {
                        SelectedAdapter = null;
                    }

                    Adapters.RemoveAt(index);
                }
            }

            if (SelectedAdapter == null && Adapters.Count > 0) {
                SelectAdapterById(Settings.Default.ActiveAdapter);
                if (SelectedAdapter == null) {
                    SelectedAdapter = Adapters[0];
                }
            }
        }

        private void SelectAdapterById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                return;
            }

            NetworkAdapter adapter = Adapters.FirstOrDefault(item => item.Id == id);
            if (adapter != null) {
                SelectedAdapter = adapter;
            }
        }

        private void ResetTotals()
        {
            if (SelectedAdapter == null) {
                return;
            }

            mResetMode = true;
            mResetBaselines.Clear();
            foreach (NetworkAdapter adapter in Adapters) {
                mResetBaselines[adapter.Id] = CreateBaseline(adapter);
            }

            UpdateMetrics(0, 0);
        }

        private void RestoreSessionTotals()
        {
            mResetMode = false;
            mResetBaselines.Clear();
            UpdateMetrics(0, 0);
        }

        private TrafficBaseline GetDisplayBaseline(NetworkAdapter adapter)
        {
            if (!mResetMode) {
                return TrafficBaseline.Zero;
            }

            TrafficBaseline baseline;
            if (!mResetBaselines.TryGetValue(adapter.Id, out baseline)) {
                baseline = CreateBaseline(adapter);
                mResetBaselines[adapter.Id] = baseline;
            }

            return baseline;
        }

        private static TrafficBaseline CreateBaseline(NetworkAdapter adapter)
        {
            return new TrafficBaseline(adapter.GetSentBytesAll(), adapter.GetReceivedBytesAll());
        }

        private static string FormatBytes(long bytes)
        {
            if (bytes > 0 && bytes < 1024) {
                bytes = 1024;
            }

            if (bytes == 0) {
                return "0.00 KB";
            }

            string[] sizes = { "", "KB", "MB", "GB", "TB" };
            double value = bytes;
            int order = 0;
            while (value >= 1024 && order < sizes.Length - 1) {
                order++;
                value /= 1024;
            }

            return string.Format(CultureInfo.CurrentCulture, "{0:N2} {1}", value, sizes[order]);
        }

        private static string FormatRate(long bytesPerSecond)
        {
            return FormatBytes(bytesPerSecond) + "/s";
        }

        private sealed class TrafficBaseline
        {
            public static readonly TrafficBaseline Zero = new TrafficBaseline(0, 0);

            public TrafficBaseline(long sentBytes, long receivedBytes)
            {
                SentBytes = sentBytes;
                ReceivedBytes = receivedBytes;
            }

            public long SentBytes { get; private set; }
            public long ReceivedBytes { get; private set; }

            public TrafficBaseline Clone()
            {
                return new TrafficBaseline(SentBytes, ReceivedBytes);
            }
        }
    }
}
