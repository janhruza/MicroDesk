using System.Windows;
using System.Windows.Controls;
using FileVault.Pages;
using LibMicroDesk;

namespace FileVault
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // load settings
            MDCore.EnsureSettings();

            // app startup
            MainWindow mw = new MainWindow();
            MainWindow = mw;
            MainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // post exit cleanup
            return;
        }

        internal static PgDrives PgDrives = new PgDrives();

        internal static bool Navigate(Page page)
        {
            return FileVault.MainWindow.Navigate(page);
        }

        internal static void SetStatusMessage(string message)
        {
            FileVault.MainWindow.SetStatusMessage(message);
            return;
        }
    }
}
