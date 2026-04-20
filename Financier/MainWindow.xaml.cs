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
        this.ExtendsContentIntoTitleBar = true;

        // minimum window size
        OverlappedPresenter presenter = OverlappedPresenter.Create();
        presenter.PreferredMinimumWidth = 640;
        presenter.PreferredMinimumHeight = 480;
        this.AppWindow.SetPresenter(presenter);

        // default window size
        this.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(800, 600));
    }

    private void titleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        nav.IsPaneOpen = !nav.IsPaneOpen;
    }

    private void nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            frm.Navigate(typeof(SettingsPage), App.PgSettings);
            tbTitle.Text = "Settings";
            return;
        }

        // change pages
        if (args.SelectedItem is NavigationViewItem item)
        {
            if (item.Tag is string sId)
            {
                string pageTitle = string.Empty;
                switch (sId)
                {
                    default:
                        pageTitle = "Unknown Page";
                        break;

                    case "home":
                        frm.Navigate(typeof(HomePage), App.PgHome);
                        pageTitle = "Dashboard";
                        break;

                    case "expanses":
                        frm.Navigate(typeof(NewTransactionPage), Core.TransactionType.Expense);
                        pageTitle = NewTransactionPage.TITLE_EXPANSES;
                        break;

                    case "incomes":
                        frm.Navigate(typeof(NewTransactionPage), Core.TransactionType.Income);
                        pageTitle = NewTransactionPage.TITLE_INCOMES;
                        break;
                }

                // update the UI page title
                tbTitle.Text = pageTitle;
            }
        }
    }

    private void frm_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Parameter is TransactionType type)
        {
            titleBar.Subtitle = type switch
            {
                TransactionType.Income => "New Income",
                TransactionType.Expense => "New Expense",
                _ => "Transaction"
            };
        }
        else if (e.Content is Page pg && pg.Tag is string sValue)
        {
            // Fallback pro ostatní stránky (Home atd.)
            titleBar.Subtitle = sValue;
        }
    }
}
