using Headlines.Core;
using Headlines.Windows;

using LibMicroDesk.Windows;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Headlines.Pages;

/// <summary>
/// Representing the overview page.
/// </summary>
public partial class PgOverview : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgOverview"/> class.
    /// </summary>
    public PgOverview()
    {
        InitializeComponent();

        this.Loaded += async (s, e) =>
        {
            await RefreshFeeds();
        };
    }

    private bool CreateFeedItemButton(RssFeed feed)
    {
        // TODO better display of items
        if (string.IsNullOrWhiteSpace(feed.Title))
        {
            return false;
        }
        TreeViewItem tvi = new TreeViewItem
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Padding = new System.Windows.Thickness(0, 10, 10, 10),
            ToolTip = new TextBlock
            {
                Inlines =
                {
                    new Run(feed.Title)
                    {
                        FontWeight = System.Windows.FontWeights.SemiBold,
                        FontSize = 14
                    },

                    new LineBreak(),

                    new Run(feed.Description)
                }
            },

            Header = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new Image
                    {
                        Width = 32,
                        Height = 32,
                        Margin = new Thickness(0, 0, 5, 0),
                        //Source = !string.IsNullOrWhiteSpace(feed.Image) ? new BitmapImage(new Uri(feed.Image)) : new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/rss.png")),
                        Source = !string.IsNullOrWhiteSpace(feed.Image) ? new BitmapImage(new Uri(feed.Image)) : null
                    },

                    new TextBlock
                    {
                        Margin = new Thickness(10, 0, 0, 0),
                        Text = feed.Title,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        TextWrapping = TextWrapping.NoWrap,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                }
            }
        };

        tvi.Selected += (s, e) =>
        {
            lbxEntries.Items.Clear();
            // load the selected feed
            if (feed.Items != null)
            {
                foreach (RssFeedItem item in feed.Items)
                {
                    ListBoxItem lbi = new ListBoxItem
                    {
                        Content = item.Title
                    };

                    lbi.MouseDoubleClick += (s, e) =>
                    {
                        // TODO show description in a dialog (temparary)
                        IconlessWindow wnd = new IconlessWindow
                        {
                            Owner = Application.Current.MainWindow,
                            Title = item.Title,
                            ResizeMode = ResizeMode.NoResize,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner,
                            MaxWidth = 600,
                            Content = new TextBlock
                            {
                                TextWrapping = System.Windows.TextWrapping.Wrap,
                                Margin = new Thickness(10),
                                Inlines =
                                {
                                    new Run(item.Title)
                                    {
                                        FontWeight = System.Windows.FontWeights.Bold,
                                        FontSize = 18,
                                        Foreground = SystemColors.AccentColorBrush,
                                    },

                                    //new LineBreak(),
                                    new LineBreak(),

                                    new Run(item.Description.Trim() ?? "No description.")
                                }
                            }
                        };

                        wnd.ShowDialog();
                    };

                    lbxEntries.Items.Add(lbi);
                }
            }
        };

        trvFeeds.Items.Add(tvi);

        return true;
    }

    private async Task RefreshFeeds()
    {
        // get all feeds
        this.Cursor = System.Windows.Input.Cursors.AppStarting;
        List<RssFeed> feeds = [];
        await App.FetchAllFeeds(feeds);

        // build the feeds list
        trvFeeds.Items.Clear();
        lbxEntries.Items.Clear();

        foreach (RssFeed feed in feeds)
        {
            // check if feed is valid
            if (string.IsNullOrWhiteSpace(feed.Title))
            {
                continue;
            }

            CreateFeedItemButton(feed);
        }

        this.Cursor = System.Windows.Input.Cursors.Arrow;
        return;
    }

    private void AddNewFeed()
    {
        WndAddFeed wnd = new WndAddFeed
        {
            Owner = Application.Current.MainWindow
        };

        _ = wnd.ShowDialog();
        return;
    }

    private async void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        await RefreshFeeds();
    }

    private void btnNewFeed_Click(object sender, RoutedEventArgs e)
    {
        AddNewFeed();
    }
}
