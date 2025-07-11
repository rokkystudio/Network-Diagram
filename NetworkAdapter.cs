using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace NetworkDiagram
{
    // Представляет сетевой адаптер и предоставляет данные о сетевой активности
    public class NetworkAdapter
    {
        private NetworkInterface mInterface;
        private long mBytesSentLast, mBytesReceivedLast;

        // Обновляет список адаптеров в ComboBox:
        // добавляет новые и удаляет отключённые.
        public static void UpdateAdapters(ComboBox comboBox)
        {
            List<NetworkAdapter> currentAdapters = GetAdapters();
            var currentIds = new HashSet<string>(currentAdapters.Select(a => a.Id));

            // Добавление новых
            foreach (var adapter in currentAdapters) {
                if (!comboBox.Items.Cast<NetworkAdapter>().Any(x => x.Id == adapter.Id)) {
                    comboBox.Items.Add(adapter);
                }
            }

            // Удаление неактуальных
            var itemsToRemove = comboBox.Items.Cast<NetworkAdapter>()
                .Where(item => !currentIds.Contains(item.Id))
                .ToList();

            foreach (var item in itemsToRemove) {
                comboBox.Items.Remove(item);
            }
        }

        // Возвращает список всех сетевых адаптеров на устройстве.
        public static List<NetworkAdapter> GetAdapters()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            List<NetworkAdapter> result = new List<NetworkAdapter>();
            foreach (NetworkInterface i in interfaces) {
                result.Add(new NetworkAdapter(i));
            }
            return result;
        }

        // Уникальный идентификатор адаптера.
        public string Id {
            get { return mInterface.Id; }
        }

        // Описание адаптера (для отображения).
        public string Description {
            get { return mInterface.Description; }
        }

        // Ссылка на оригинальный NetworkInterface.
        public NetworkInterface Interface {
            get { return mInterface; }
        }

        // Создаёт адаптер на основе System.Net.NetworkInformation.NetworkInterface.
        public NetworkAdapter(NetworkInterface adapter) {
            mInterface = adapter;
            mBytesSentLast = GetSentBytesAll();
            mBytesReceivedLast = GetReceivedBytesAll();
        }

        // Возвращает количество отправленных байт с момента последнего вызова.
        public int GetSentCount() {
            long bytesSent = GetSentBytesAll();
            long sentSpeed = Math.Max(0, bytesSent - mBytesSentLast);
            mBytesSentLast = bytesSent;
            return (int)sentSpeed;
        }

        // Возвращает количество полученных байт с момента последнего вызова.
        public int GetReceivedCount() {
            long bytesReceived = GetReceivedBytesAll();
            long receivedSpeed = Math.Max(0, bytesReceived - mBytesReceivedLast);
            mBytesReceivedLast = bytesReceived;
            return (int) receivedSpeed;
        }
        
        // Получает общее количество отправленных байт IPv4.
        // В случае ошибки — возвращает 0.
        public long GetSentBytesAll() {
            try {
                return mInterface.GetIPv4Statistics().BytesSent;
            }
            catch {
                return 0;
            }
        }

        // Получает общее количество полученных байт IPv4.
        // В случае ошибки — возвращает 0.
        public long GetReceivedBytesAll() {
            try {
                return mInterface.GetIPv4Statistics().BytesReceived;
            }
            catch {
                return 0;
            }
        }

        // Возвращает описание адаптера (используется в ComboBox).
        public override string ToString() {
            return mInterface.Description;
        }
    }
}
