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
    }

    private void titleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        this.nav.IsPaneOpen = !this.nav.IsPaneOpen;
    }

    /// <summary>
    /// Gets the associated frame control.
    /// </summary>
    public Frame WindowFrame => this.frm;

    /// <summary>
    /// Displays an in-app banner message.
    /// </summary>
    /// <param name="severity">Message severity.</param>
    /// <param name="title">Message title.</param>
    /// <param name="message">Message text.</param>
    public void DisplayMessage(InfoBarSeverity severity, string title, string message)
    {
        this.ib.Severity = severity;
        this.ib.Title = title;
        this.ib.Message = message;
        this.ib.IsOpen = true;
        return;
    }

    private void nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            _ = this.frm.Navigate(typeof(SettingsPage), App.PgSettings);
            return;
        }

        // change pages
        if (args.SelectedItem is NavigationViewItem item)
        {
            if (item.Tag is string sId)
            {
                switch (sId)
                {
                    case "home":
                        _ = this.frm.Navigate(typeof(HomePage), App.PgHome);
                        break;

                    case "expanses":
                        _ = this.frm.Navigate(typeof(NewTransactionPage), Core.TransactionType.Expense);
                        break;

                    case "incomes":
                        _ = this.frm.Navigate(typeof(NewTransactionPage), Core.TransactionType.Income);
                        break;

                    case "logout":
                        _ = UserProfile.SetCurrent(null);
                        _ = this.frm.Navigate(typeof(ProfileSelectionPage));
                        DisplayMessage(InfoBarSeverity.Informational, "Logout", "Logout successful.");
                        break;
                }
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
            this.titleBar.Subtitle = sValue;
        }
    }

    private void frm_Navigating(object sender, NavigatingCancelEventArgs e)
    {
        // If the user profile is not loaded, prevent navigation to any page except the profile selection page
        if (UserProfile.IsLoaded() == false && e.SourcePageType != typeof(ProfileSelectionPage))
        {
            e.Cancel = true;
            _ = this.frm.Navigate(typeof(ProfileSelectionPage));

            // notify the user that they need to select a profile first
            DisplayMessage(InfoBarSeverity.Warning, "No profile loaded", "Please select a user profile to continue.");
            return;
        }
    }
}
