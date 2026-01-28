using LibMicroDesk.Windows;

using System.Text;
using System.Windows;
using System.Windows.Controls;

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

        Loaded += (s, e) =>
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

    private void IconlessWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.F3)
        {
            e.Handled = true;

            // show loaded RSS feeds

            if (App.RssFeedSources.Count == 0)
            {
                _ = MessageBox.Show("No RSS sources created.", "RSS Sources", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (string rss in App.RssFeedSources)
            {
                sb.AppendLine(rss);
            }

            _ = MessageBox.Show(sb.ToString(), "RSS sources", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}