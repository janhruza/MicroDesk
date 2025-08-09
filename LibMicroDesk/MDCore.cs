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
    }
}
