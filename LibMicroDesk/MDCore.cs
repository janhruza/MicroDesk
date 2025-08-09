using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using LibMicroDesk.Windows;

namespace LibMicroDesk
{
    /// <summary>
    /// Representing the core functionality of the MicroDesk library.
    /// </summary>
    public static class MDCore
    {
        /// <summary>
        /// Shows the About dialog for the MicroDesk applications.
        /// </summary>
        public static void AboutBox()
        {
            _ = new WndAbout().ShowDialog();
        }

        /// <summary>
        /// Shows the General Settings dialog for the MicroDesk applications.
        /// </summary>
        public static void SettingsBox()
        {
            _ = new WndGeneralSettings().ShowDialog();
        }

        /// <summary>
        /// Applies the specified system theme to the application.
        /// </summary>
        /// <param name="theme">New theme to be applied.</param>
        public static void ApplyTheme(SystemTheme theme)
        {
            #pragma warning disable WPF0001

            switch (theme)
            {
                default:
                case SystemTheme.None:
                Application.Current.ThemeMode = ThemeMode.None;
                break;

            case SystemTheme.Light:
                Application.Current.ThemeMode = ThemeMode.Light;
                break;

            case SystemTheme.Dark:
                Application.Current.ThemeMode = ThemeMode.Dark;
                break;

            case SystemTheme.Auto:
                Application.Current.ThemeMode = ThemeMode.System;
                break;
            }

            #pragma warning restore WPF0001
        }

        /// <summary>
        /// Attempts to create a new process with the specified filename and arguments.
        /// Creates no window and does not use the shell to execute the process.
        /// </summary>
        /// <param name="filename">Full path to the executable file.</param>
        /// <param name="arguments">Optional command line arguments.</param>
        /// <param name="process">Output process object.</param>
        /// <returns><see langword="true"/> if the <paramref name="process"/> has started, otherwise <see langword="false"/>.</returns>
        public static bool CreateProcess(string filename, string arguments, out Process? process)
        {
            process = null;

            if (File.Exists(filename) == false)
            {
                Log.Error($"File not found: {filename}", nameof(CreateProcess));
                return false;
            }

            try
            {
                var startInfo = new ProcessStartInfo(filename, arguments)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process = Process.Start(startInfo);
                if (process == null)
                {
                    Log.Error($"Failed to start process: {filename}", nameof(CreateProcess));
                    return false;
                }

                else
                {
                    Log.Info($"Process started: {filename}", nameof(CreateProcess));
                    return true;
                }
            }

            catch (Exception ex)
            {
                Log.Error(ex, nameof(MDCore.CreateProcess));
                return false;
            }
        }

        /// <summary>
        /// Represents the current application settings.
        /// </summary>
        public static AppSettings Settings { get; set; }

        /// <summary>
        /// Ensures that the application settings are loaded and available.
        /// </summary>
        /// <returns>Operation result.</returns>
        public static bool EnsureSettings()
        {
            if (AppSettings.Load(out AppSettings? settings) == false)
            {
                settings = new AppSettings();
                AppSettings.Save(settings);
            }

            // apply settings
            ApplyTheme(settings.Theme);
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo(settings.Culture);
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo(settings.Culture);

            Settings = settings;
            return true;
        }
    }
}
