using System.Windows;
using MoodTracker.Pages;

namespace MoodTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MoodTracker.MainWindow mw = new MoodTracker.MainWindow();
            MainWindow = mw;
            MainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            return;
        }

        #region Pages

        /// <summary>
        /// Representing the mood page instance.
        /// </summary>
        internal static PgMood MoodPage => new PgMood();

        #endregion
    }

}
