using System.Windows.Controls;
using LibMicroDesk.Windows;

namespace Headlines;

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

        this.Loaded += (s, e) =>
        {
            Navigate(App.PgOverview);
        };
    }

    static MainWindow()
    {
        _instance ??= new MainWindow();
    }

    private static MainWindow _instance;

    /// <summary>
    /// Navigates to the specified contwent <paramref name="page"/>.
    /// </summary>
    /// <param name="page">Specified content <see cref="Page"/>.</param>
    /// <returns>Operation result.</returns>
    public static bool Navigate(Page page)
    {
        if (page == null) return false;
        if (_instance == null) return false;

        _instance.Title = $"{page.Title} | {App.AppName}";
        return _instance.frm.Navigate(page);
    }
}