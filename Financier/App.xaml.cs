using Financier.Core;
using Financier.Pages;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private static MainWindow? _window;

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
    internal static NewProfilePage PgNewProfile { get; } = new NewProfilePage();
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
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();

        if (Directory.Exists(UserProfile.ProfilesFolder))
        {
            foreach (string filename in Directory.EnumerateFiles(UserProfile.ProfilesFolder))
            {
                if (Path.GetFileNameWithoutExtension(filename) == Environment.UserName)
                {
                    string data = File.ReadAllText(filename);
                    if (UserProfile.Load(data, out UserProfile profile) == true)
                    {
                        // current user profile loaded
                        UserProfile.SetCurrent(profile);
                        _window.nviHome.IsSelected = true;
                    }
                }
            }
        }

        if (UserProfile.IsLoaded() == false)
        {
            // no profile loaded, create a new profile
            // set test profile for now
            UserProfile.SetCurrent(AppData.TestProfile);
            _window.WindowFrame.Navigate(typeof(NewProfilePage));
        }

        _window.Activate();
    }
}
