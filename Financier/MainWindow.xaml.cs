using Financier.Core;
using Financier.Pages;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new, empty, main window instance.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;

        // minimum window size
        OverlappedPresenter presenter = OverlappedPresenter.Create();
        presenter.PreferredMinimumWidth = 640;
        presenter.PreferredMinimumHeight = 480;
        AppWindow.SetPresenter(presenter);

        // default window size
        AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(800, 600));
    }

    private void titleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        this.nav.IsPaneOpen = !this.nav.IsPaneOpen;
    }

    private void nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            _ = this.frm.Navigate(typeof(SettingsPage), App.PgSettings);
            this.tbTitle.Text = "Settings";
            return;
        }

        // change pages
        if (args.SelectedItem is NavigationViewItem item)
        {
            if (item.Tag is string sId)
            {
                string pageTitle;
                switch (sId)
                {
                    default:
                        pageTitle = "Unknown Page";
                        break;

                    case "home":
                        _ = this.frm.Navigate(typeof(HomePage), App.PgHome);
                        pageTitle = "Dashboard";
                        break;

                    case "expanses":
                        _ = this.frm.Navigate(typeof(NewTransactionPage), Core.TransactionType.Expense);
                        pageTitle = NewTransactionPage.TITLE_EXPANSES;
                        break;

                    case "incomes":
                        _ = this.frm.Navigate(typeof(NewTransactionPage), Core.TransactionType.Income);
                        pageTitle = NewTransactionPage.TITLE_INCOMES;
                        break;
                }

                // update the UI page title
                this.tbTitle.Text = pageTitle;
            }
        }
    }

    private void frm_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Parameter is TransactionType type)
        {
            this.titleBar.Subtitle = type switch
            {
                TransactionType.Income => "New Income",
                TransactionType.Expense => "New Expense",
                _ => "Transaction"
            };
        }
        else if (e.Content is Page pg && pg.Tag is string sValue)
        {
            // Fallback pro ostatní stránky (Home atd.)
            this.titleBar.Subtitle = sValue;
        }
    }
}
