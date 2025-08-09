using System.Diagnostics;
using System.IO;
using System.Windows;
using MoodTracker.Core;
using MoodTracker.Pages;

namespace MoodTracker
{
    /// <summary>
    /// Represents the main application class for the Mood Tracker application.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets or sets the main application window.
        /// </summary>
        public new MainWindow MainWindow { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new MoodTracker.MainWindow();
            MainWindow.SetActivePage(MoodPage);
            MainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            return;
        }

        public static bool CenterWindow(Window? window)
        {
            if (window == null) return false;

            // center the window

            window.Left = (SystemParameters.PrimaryScreenWidth - window.Width) / 2;
            window.Top = (SystemParameters.PrimaryScreenHeight - window.Height) / 2;
            return true;
        }

        #region Pages

        /// <summary>
        /// Representing the mood page instance.
        /// </summary>
        internal static PgMood MoodPage => new PgMood();

        #endregion

        #region Mood management

        internal static string _moodFile => Path.Combine("MoodHistory.csv");

        public static bool RegisterMoodRecord(MoodRecord? moodRecord)
        {
            // check mood record validity
            if (moodRecord == null) return false;

            MoodRecord value = moodRecord.Value;

            try
            {
                // write record data
                File.AppendAllText(_moodFile, value.ToCSV() + Environment.NewLine);
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
