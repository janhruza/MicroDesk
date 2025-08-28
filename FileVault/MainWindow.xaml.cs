using System.Windows.Controls;
using LibMicroDesk;
using LibMicroDesk.Windows;

namespace FileVault;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Creates a new <see cref="MainWindow"/> instance.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        _instance = this;
        this.Loaded += this.MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        miToggleStatusPanel.IsChecked = true;
        frm.Navigate(App.PgDrives);
    }

    private void miToggleStatusPanel_Checked(object sender, System.Windows.RoutedEventArgs e)
    {
        stb.Visibility = System.Windows.Visibility.Visible;
    }

    private void miToggleStatusPanel_Unchecked(object sender, System.Windows.RoutedEventArgs e)
    {
        stb.Visibility ^= System.Windows.Visibility.Collapsed;
    }

    private void miSettings_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        MDCore.SettingsBox();
    }

    private void miClose_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.Close();
    }

    private void miAbout_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        MDCore.AboutBox();
    }

    private static MainWindow _instance;

    /// <summary>
    /// Navigates to the new content <paramref name="page"/>.
    /// </summary>
    /// <param name="page">The new active page.</param>
    /// <returns>Result of the operation.</returns>
    public static bool Navigate(Page page)
    {
        // no page
        if (page == null) return false;

        // navigate to the page
        return _instance.frm.Navigate(page);
    }

    internal static void SetStatusMessage(string message)
    {
        _instance.tbMessage.Text = message;
        return;
    }
}