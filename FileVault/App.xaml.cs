using System.Windows;

namespace FileVault
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
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
    }
}
