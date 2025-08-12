using System.Windows;
using QuickNotes.Pages;

namespace QuickNotes;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    #region Page definitions

    /// <summary>
    /// Representing the overview page.
    /// </summary>
    public static PgOverview OverviewPage => new PgOverview();

    #endregion

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        return;
    }
}
