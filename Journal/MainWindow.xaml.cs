using Journal.Pages;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Journal;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // set default params
        ExtendsContentIntoTitleBar = true;
        AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;
        AppWindow.SetPresenter(CreatePresenter());
        AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(800, 600));
        OnInitialized();
    }

    private OverlappedPresenter CreatePresenter()
    {
        OverlappedPresenter presenter = OverlappedPresenter.Create();
        presenter.PreferredMinimumHeight = 600;
        presenter.PreferredMinimumWidth = 600;

        return presenter;
    }

    private void OnInitialized()
    {
        if (this.frm is null)
        {
            return;
        }

        this.frm.Navigated += (s, e) =>
        {
            if (e.Content is PgHome)
            {
                this.titleBar.Subtitle = "Home";
            }

            else if (e.Content is PgNewEntry)
            {
                this.titleBar.Subtitle = "New Entry";
            }

            else if (e.Content is PgSettings)
            {
                this.titleBar.Subtitle = "Settings";
            }
        };

        // display the startup page
        _ = this.frm.Navigate(typeof(PgHome), App.HomePage);
    }

    private void titleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        // toogle the navigation bar
        this.nav.IsPaneOpen = !this.nav.IsPaneOpen;
    }

    private void nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            _ = this.frm.Navigate(typeof(PgSettings), App.SettingsPage);
            return;
        }

        if (sender.SelectedItem is NavigationViewItem item)
        {
            if (item.Tag is string tag)
            {
                switch (tag)
                {
                    case "home":
                        _ = this.frm.Navigate(typeof(PgHome), App.HomePage);
                        break;

                    case "new":
                        _ = this.frm.Navigate(typeof(PgNewEntry), App.NewEntryPage);
                        break;
                }
            }
        }
    }
}
