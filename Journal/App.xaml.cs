using Journal.Core;

using MDCore;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Journal;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    internal static Page HomePage { get; } = new Pages.PgHome();
    internal static Page NewEntryPage { get; } = new Pages.PgNewEntry();
    internal static Page SettingsPage { get; } = new Pages.PgSettings();

    internal static void SetThemeMode(ElementTheme theme)
    {
        if (_window != null)
        {
            if (_window.Content is FrameworkElement elm)
            {
                elm.RequestedTheme = theme;
            }
        }
    }

    internal static void SetAppBackdrop(SystemBackdrop backdrop)
    {
        if (_window != null)
        {
            _window.SystemBackdrop = backdrop;
        }
    }

    private static Window? _window;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
        this.UnhandledException += Application_UnhandledException;
    }

    /// <summary>
    /// Navigates the main application window to the home view.
    /// </summary>
    /// <remarks>This method has no effect if the main window is not available or does not support home
    /// navigation.</remarks>
    public static void NavigateHome()
    {
        if (_window is MainWindow mw)
        {
            mw.NavigateHome();
        }
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        Log.Info(Log.AppStarted);

        // create initial data
        JournalEntry.SetupJournals();
        _ = JournalEntry.LoadEntries();

        _window = new MainWindow();
        _window.Closed += _window_Closed;

        // load app settings
        AppSettings.LoadSettings();
        AppSettings? settings = AppSettings.GetCurrent();
        if (settings.HasValue == false)
        {
            // set new settings
            AppSettings.SetCurrent(settings);
            Log.Info("New settings have been initialized.");
        }

        else
        {
            // apply the loaded settings
            AppSettings value = settings.Value;
            _window.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(value.Left, value.Top, value.Width, value.Height));
        }

        _window.Activate();
    }

    private void _window_Closed(object sender, WindowEventArgs args)
    {
        // save data
        _ = JournalEntry.SaveEntries(JournalEntry.Entries);

        // save app settings
        AppSettings.SaveSettings();

        Log.Info("Main window closed.");
    }

    private void Application_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        if (e.Exception is AggregateException ex)
        {
            string errors = string.Join(Environment.NewLine, ex.InnerExceptions);
            Log.Error(errors);
        }

        else
        {
            Log.Error(e.Exception.ToString() ?? Log.UnhandledException);
        }
    }
}
