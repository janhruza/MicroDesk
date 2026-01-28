using System;
using System.Windows;
using System.Windows.Controls;

namespace LibMicroDesk.Windows;

/// <summary>
/// Representing a <see cref="Window"/> that serves as a <see cref="Page"/> viewer.
/// </summary>
public partial class WndPagePreview : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndPagePreview"/> class with the given content <paramref name="page"/>.
    /// </summary>
    public WndPagePreview(Page page)
    {
        // check if page is valid
        ArgumentNullException.ThrowIfNull(page);

        _page = page;
        InitializeComponent();

        Loaded += IconlessWindow_Loaded;
    }

    private Page _page;

    private bool LoadPage(Page pg)
    {
        Title = pg.Title;
        return frm.Navigate(pg);
    }

    private void IconlessWindow_Loaded(object sender, RoutedEventArgs e)
    {
        LoadPage(_page);
    }
}
