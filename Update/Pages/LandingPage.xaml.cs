using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Update.Core.Data;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Update.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LandingPage : Page
{
    /// <summary>
    /// Representing the constructor of the <see cref="LandingPage"/> class.
    /// </summary>
    public LandingPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        lvFeedEntries.Items.Clear();
        if (e.Parameter is not RssFeed feed)
        {
            ListViewItem item = new ListViewItem
            {
                Content = "No feed items available."
            };
            
            lvFeedEntries.Items.Add(item);
            return;
        }

        // param seems valid
        foreach (var channel in feed.Channels)
        {
            foreach (var entry in channel.Entries)
            {
                ListViewItem item = new ListViewItem
                {
                    Content = entry.Title
                };

                lvFeedEntries.Items.Add(item);
            }

            // TODO display all feeds
        }
    }
}
