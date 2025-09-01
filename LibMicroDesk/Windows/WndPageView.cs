using System.Windows;
using System.Windows.Controls;

namespace LibMicroDesk.Windows;

/// <summary>
/// Representing a simple <see cref="Page"/> viewer <see cref="Window"/> class.
/// </summary>
public class WndPageView : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndPageView"/> class with the given content <see cref="Page"/> <paramref name="pgContent"/>.
    /// </summary>
    /// <param name="pgContent">Page with content to view.</param>
    public WndPageView(Page pgContent)
    {
        this.MinHeight = 300;
        this.MinWidth = 300;
        this.Title = "Page Viewer";
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        // frame
        Frame frame = new Frame
        {
            Margin = new Thickness(10),
            NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden
        };

        this.Loaded += (s, e) =>
        {
            if (frame == null) return;

            // display page
            if (frame.Navigate(pgContent) == true)
            {
                this.Title = pgContent.Title;
            }
        };
    }
}
