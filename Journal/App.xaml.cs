using Journal.Core;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

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

    static Window? _window;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // create initial data
        JournalEntry.SetupJournals();
        JournalEntry.LoadEntries();

        _window = new MainWindow();
        _window.Closed += _window_Closed;
        _window.Activate();
    }

    private void _window_Closed(object sender, WindowEventArgs args)
    {
        JournalEntry.SaveEntries(JournalEntry.Entries);
    }
}
