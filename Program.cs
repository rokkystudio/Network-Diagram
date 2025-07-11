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
            Mutex mutex = new Mutex(true, "ROKKY_STUDIO_NETWORK_DIAGRAM_MUTEX");
            if (mutex.WaitOne(TimeSpan.Zero, true)) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                mutex.ReleaseMutex();
            }
        }
    }
}
