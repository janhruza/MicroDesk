using System.Windows;
using System.Windows.Controls;

namespace MoodTracker;

/// <summary>
/// Represents the main window of the Mood Tracker application.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Constructs a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Sets the active page in the content frame of the main window.
    /// </summary>
    /// <param name="page">New active page.</param>
    /// <returns><see langword="true"/> if the <paramref name="page"/> is set as active, otherwise <see langword="false"/>.</returns>
    public bool SetActivePage(Page? page)
    {
        if (page == null) return false;

        if (frmContent.Navigate(page) == false)
        {
            return false;
        }

        this.Title = page.Title;
        return true;
    }

    private void Window_Closed(object sender, System.EventArgs e)
    {
        if (sender is MainWindow mainWindow)
        {
            App.ShowTrayMessage("Mood Tracker is running in the tray.");
        }
    }
}