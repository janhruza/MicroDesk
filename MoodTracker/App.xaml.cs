using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using MoodTracker.Core;
using MoodTracker.Pages;

using System.Windows.Forms;

using Application = System.Windows.Application;

namespace MoodTracker
{
    /// <summary>
    /// Represents the main application class for the Mood Tracker application.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Shell notify icon object.
        /// </summary>
        private NotifyIcon? _trayIcon;

        /// <summary>
        /// Gets or sets the main application window.
        /// </summary>
        public new MainWindow MainWindow { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // create the shell tray icon
            _trayIcon = new NotifyIcon
            {
                Icon = System.Drawing.SystemIcons.Application,
                Text = "Mood Tracker",
                Visible = true
            };

            // create tray menu items
            _trayIcon.ContextMenuStrip = new ContextMenuStrip();
            _trayIcon.ContextMenuStrip.Items.Add("New mood record", null, (s, args) =>
            {
                // show the main window
                if (MainWindow == null || !MainWindow.IsVisible)
                {
                    MainWindow = new MoodTracker.MainWindow();
                    MainWindow.SetActivePage(MoodPage);
                    MainWindow.Show();
                }
                else
                {
                    MainWindow.SetActivePage(MoodPage);
                    MainWindow.Activate();
                }
            });

            _trayIcon.ContextMenuStrip.Items.Add("Show mood history", null, (s, args) =>
            {
                // show the mood history page
                if (MainWindow == null || !MainWindow.IsVisible)
                {
                    MainWindow = new MoodTracker.MainWindow();
                    MainWindow.SetActivePage(MoodHistoryPage);
                    MainWindow.Show();
                }
                else
                {
                    MainWindow.SetActivePage(MoodHistoryPage);
                    MainWindow.Activate();
                }
            });

            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, (s, args) =>
            {
                // exit the application
                Application.Current.Shutdown();
            });

            // create the main window
            MainWindow = new MoodTracker.MainWindow();
            MainWindow.SetActivePage(MoodPage);
            MainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // clean up resources
            if (_trayIcon != null)
            {
                _trayIcon.Dispose();
            }

            return;
        }

        /// <summary>
        /// Centers the specified window on the primary screen.
        /// </summary>
        /// <param name="window">Target window.</param>
        /// <returns>Operation result.</returns>
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

        /// <summary>
        /// Representing the mood history page instance.
        /// </summary>
        internal static PgMoodHistory MoodHistoryPage => new PgMoodHistory();

        #endregion

        #region Mood management

        internal static string _moodFile => Path.Combine("MoodHistory.csv");

        /// <summary>
        /// Attempts to register a new mood record in the mood history file.
        /// </summary>
        /// <param name="moodRecord">A mood history record.</param>
        /// <returns>Operation result.</returns>
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

        /// <summary>
        /// Reads the mood history file and returns an array of mood records.
        /// </summary>
        /// <param name="moodRecords">Output array of history records.</param>
        /// <returns>Operation result.</returns>
        public static bool ReadMoodHistoryFile(out MoodRecord[] moodRecords)
        {
            moodRecords = [];
            // check if the file exists
            if (File.Exists(_moodFile) == false)
            {
                return false;
            }
            try
            {
                // read all lines from the file
                string[] lines = File.ReadAllLines(_moodFile);
                moodRecords = new MoodRecord[lines.Length];
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] parts = line.Split(';');
                    if (parts.Length != 2)
                    {
                        continue; // skip invalid lines
                    }
                    DateTime timestamp = DateTime.Parse(parts[0]);
                    Mood mood = (Mood)Enum.Parse(typeof(Mood), parts[1]);
                    moodRecords[i] = new MoodRecord(timestamp, mood);
                }
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
