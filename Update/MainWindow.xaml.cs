using MDCore;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Linq;
using System.Threading.Tasks;

using Update.Core.Data;
using Update.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Update;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        this.AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
        this.Closed += MainWindow_Closed;

        NavigateTo(typeof(LandingPage), new object());

        ReloadFeeds();
    }

    private void ReloadFeeds()
    {
        nvMenu.MenuItems.Clear();

        foreach (string feed in App.CurrentSettings.Feeds)
        {
            if (RssFeed.LoadFromUrl(feed, out RssFeed rssFeed) == false)
            {
                Log.Error($"RSS reading error. Feed address: {feed}");
                continue;
            }

            NavigationViewItem nvi = new NavigationViewItem
            {
                Content = rssFeed.Title,
                Icon = new SymbolIcon(Symbol.Link),
                Tag = rssFeed
            };

            nvMenu.MenuItems.Add(nvi);
        }
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args)
    {
        // save settings
        App.CurrentSettings.WriteToFile();
    }

    /// <summary>
    /// Navigates to the specified page type, optionally passing a parameter to the target page.
    /// </summary>
    /// <param name="pageType">The type of the page to navigate to. Must not be null.</param>
    /// <param name="parameter">An optional parameter to pass to the target page. Can be null if no parameter is required.</param>
    /// <returns>true if navigation to the specified page was successful; otherwise, false.</returns>
    public bool NavigateTo(Type pageType, object? parameter = null)
    {
        return frm.Navigate(pageType, parameter);
    }

    /// <summary>
    /// Displays a message in the InfoBar with the specified severity and message content.
    /// </summary>
    /// <param name="severity">Message severity.</param>
    /// <param name="message">Message content.</param>
    public void DisplayMessage(InfoBarSeverity severity, string message)
    {
        infoBar.Severity = severity;
        infoBar.Title = severity switch
        {
            InfoBarSeverity.Error => "Error",
            InfoBarSeverity.Warning => "Warning",
            InfoBarSeverity.Success => "Success",
            _ => "Info"
        };
        infoBar.Message = message;
        infoBar.IsOpen = true;
        return;
    }

    private void titleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        // unused
        nvMenu.IsPaneOpen = !nvMenu.IsPaneOpen;
        return;
    }

    private void titleBar_BackRequested(TitleBar sender, object args)
    {
        // unused
        return;
    }

    private async Task<string> DlgNewFeed()
    {
        ContentDialog dlg = new ContentDialog
        {
            Title = "Add New Feed",
            PrimaryButtonText = "Add",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = this.Content.XamlRoot,
            Content = new TextBox
            {
                Header = "RSS Feed URL",
                PlaceholderText = "Enter feed URL",
                Margin = new Thickness(0, 10, 0, 0)
            }
        };

        var result = await dlg.ShowAsync();

        if (result == ContentDialogResult.Primary && dlg.Content is TextBox tb && !string.IsNullOrWhiteSpace(tb.Text))
        {
            return tb.Text.Trim();
        }

        return string.Empty;
    }

    private async void nvMenu_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            NavigateTo(typeof(SettingsPage));
            return;
        }

        // navigate to the RSS feed view
        if (args.SelectedItem is NavigationViewItem nvi)
        {
            if (nvi.Tag is RssFeed feed)
            {
                NavigateTo(typeof(LandingPage), feed);
            }
        }
    }

    private async void btnNewFeed_Click(object sender, RoutedEventArgs e)
    {
        string feed = await DlgNewFeed();
        if (string.IsNullOrEmpty(feed) == false)
        {
            RSS.RegisterFeed(feed);
            infoBar.Message = $"New feed added: {feed}";
            infoBar.Severity = InfoBarSeverity.Success;
            infoBar.IsOpen = true;
            return;
        }
    }
}
