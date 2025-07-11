using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public class NetworkAdapter
    {
        public static void UpdateAdapters(ComboBox comboBox)
        {
            List<NetworkAdapter> adapters = NetworkAdapter.GetAdapters();

            // Add new adapters
            foreach (NetworkAdapter adapter in adapters)
            {
                bool found = false;
                foreach (NetworkAdapter item in comboBox.Items)
                {
                    if (item.Id == adapter.Id) {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    comboBox.Items.Add(adapter);
                }
            }

            // Remove old adapters
            NetworkAdapter[] items = comboBox.Items.Cast<NetworkAdapter>().ToArray();
            foreach (NetworkAdapter item in items)
            {
                bool found = false;
                foreach (NetworkAdapter adapter in adapters)
                {
                    if (item.Id == adapter.Id) {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    comboBox.Items.Remove(item);
                }
            }
        }


        public static List<NetworkAdapter> GetAdapters()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            List<NetworkAdapter> result = new List<NetworkAdapter>();
            foreach (NetworkInterface i in interfaces) {
                result.Add(new NetworkAdapter(i));
            }
            return result;
        }

        private NetworkInterface mInterface;
        private long mBytesSentLast, mBytesReceivedLast;

        public string Id {
            get { return mInterface.Id; }
        }

        public string Description {
            get { return mInterface.Description; }
        }

        public NetworkInterface Interface {
            get { return mInterface; }
        }

        public NetworkAdapter(NetworkInterface adapter) {
            mInterface = adapter;
            mBytesSentLast = GetSentBytesAll();
            mBytesReceivedLast = GetReceivedBytesAll();
        }

        public int GetSentCount() {
            long bytesSent = GetSentBytesAll();
            long sentSpeed = bytesSent - mBytesSentLast;
            mBytesSentLast = bytesSent;
            return (int) sentSpeed;
        }

        public int GetReceivedCount() {
            long bytesReceived = GetReceivedBytesAll();
            long receivedSpeed = bytesReceived - mBytesReceivedLast;
            mBytesReceivedLast = bytesReceived;
            return (int) receivedSpeed;
        }

        public long GetSentBytesAll() {
            return mInterface.GetIPv4Statistics().BytesSent;
        }

        public long GetReceivedBytesAll() {
            return mInterface.GetIPv4Statistics().BytesReceived;
        }

        public override string ToString() {
            return mInterface.Description;
        }
    }
}
