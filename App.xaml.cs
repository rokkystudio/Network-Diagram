using System;
using System.Threading;
using System.Windows;

namespace NetworkDiagram
{
    public partial class App : Application
    {
        private Mutex mMutex;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            mMutex = new Mutex(true, "ROKKY_STUDIO_NETWORK_DIAGRAM_MUTEX", out bool createdNew);
            if (!createdNew) {
                Shutdown();
                return;
            }

            MainWindow mainWindow = new MainWindow();
            MainWindow = mainWindow;
            mainWindow.RestoreWindowPlacementAndVisibility();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            if (mMutex == null) {
                return;
            }

            mMutex.ReleaseMutex();
            mMutex.Dispose();
            mMutex = null;
        }
    }
}
