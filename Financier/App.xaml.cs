using Financier.Core;
using Financier.Pages;

using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private static Window? _window;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    [DllImport("dwmapi")]
    internal static extern int DwmSetWindowAttribute(IntPtr hWnd, int dwAttribute, int[] pwAttribute, int cbSize);

    #region Page definitions
    internal static HomePage PgHome { get; } = new HomePage();
    internal static NewTransactionPage PgNewIncome { get; } = new NewTransactionPage();
    internal static NewTransactionPage PgNewExpanse { get; } = new NewTransactionPage();
    internal static SettingsPage PgSettings { get; } = new SettingsPage();
    #endregion

    internal static async Task ShowDialog(XamlRoot xamlRoot, string message, string title)
    {
        ContentDialog dlg = new ContentDialog
        {
            XamlRoot = xamlRoot,
            Content = message,
            Title = title,
            PrimaryButtonText = "OK"
        };

        _ = await dlg.ShowAsync();
        return;
    }

    internal static void SetAppThemeMode(ElementTheme theme)
    {
        if (_window is null) return;
        if (_window.Content is FrameworkElement elm)
        {
            elm.RequestedTheme = theme;
        }
    }

    internal static void SetAppBackdropMode(SystemBackdrop? backdrop)
    {
        if (_window is null) return;
        _window.SystemBackdrop = backdrop;
        return;
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // TODO testing setup
        _ = UserProfile.SetCurrent(AppData.TestProfile);

        _window = new MainWindow();
        _window.Activate();
    }
}
