using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDiagram
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(true, "ROKKY_STUDIO_NETWORK_DIAGRAM_MUTEX", out bool createdNew)) {
                if (createdNew) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    try {
                        Application.Run(new MainForm());
                    }
                    finally {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }
    }
}
