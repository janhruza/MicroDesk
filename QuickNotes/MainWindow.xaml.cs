using LibMicroDesk.Windows;

using System.Windows;
using System.Windows.Controls;

namespace QuickNotes;

/// <summary>
/// Representing the main application window.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        _instance = this;
        InitializeComponent();
    }

    private void IconlessWindow_Loaded(object sender, RoutedEventArgs e)
    {
        this.frmContent.Navigate(App.OverviewPage);
    }

    #region Static code

    private static MainWindow? _instance;

    /// <summary>
    /// Representng a current instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public static MainWindow Instance => _instance ?? (_instance = new MainWindow());

    /// <summary>
    /// Attempts to navigate to the selected <paramref name="content"/>.
    /// </summary>
    /// <param name="content">New window content (content of the <see cref="Frame"/>).</param>
    /// <returns><see langword="true"/> if navigation is successful, otherwise <see langword="false"/>.</returns>
    public static bool Navigate(object? content)
    {
        if (content == null) return false;
        if (_instance == null) return false;

        return _instance.frmContent.Navigate(content);
    }

    #endregion
}