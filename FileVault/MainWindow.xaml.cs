using FileVault.Pages;
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
        this.Loaded += this.MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        miToggleStatusPanel.IsChecked = true;
        frm.Navigate(new PgDrives());
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
}