using Financier.Core;
using Financier.Pages;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window? _window;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

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

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // TODO testing setup
        _ = UserProfile.SetCurrent(AppData.TestProfile);

        this._window = new MainWindow();
        this._window.Activate();
    }
}
