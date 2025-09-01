﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Headlines.Core;
using LibMicroDesk;
using LibMicroDesk.Windows;

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
        TreeViewItem tvi = new TreeViewItem
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Header = feed.Title,
            Padding = new System.Windows.Thickness(10),
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

                                    new LineBreak(),
                                    new LineBreak(),

                                    new Run(item.Description)
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
        List<RssFeed> feeds = [];
        await App.FetchAllFeeds(feeds);

        // build the feeds list
        trvFeeds.Items.Clear();
        lbxEntries.Items.Clear();

        foreach (RssFeed feed in feeds)
        {
            CreateFeedItemButton(feed);
        }

        return;
    }

    private async void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        await RefreshFeeds();
    }
}
