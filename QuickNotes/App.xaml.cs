using System.Collections.Generic;
using System.Windows;
using QuickNotes.Core;
using QuickNotes.Pages;
using System;

namespace QuickNotes;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    static App()
    {
        Notes = [];
        GenerateTestData();
    }

    #region Page definitions

    /// <summary>
    /// Representing the overview page.
    /// </summary>
    public static PgOverview OverviewPage => new PgOverview();

    #endregion

    #region Static code

    private static void GenerateTestData()
    {
        Notes.AddRange([
            new Note("Create a new application.", DateTime.Now, DateTime.Now.AddDays(7)),
            new Note("Go to a barber shop.", DateTime.Now, DateTime.Now.AddDays(14)),
            new Note("Feed cat.", DateTime.Now, DateTime.Now.AddDays(-3)),
        ]);
    }

    /// <summary>
    /// Representing a list of all loaded notes.
    /// </summary>
    public static List<Note> Notes { get; }

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
