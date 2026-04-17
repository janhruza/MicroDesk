using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Journal.Core;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Journal.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PgHome : Page
{
    public PgHome()
    {
        InitializeComponent();
    }

    private void LoadUI()
    {
        txtPreviewTitle.Text = string.Empty;
        txtPreviewContent.Text = string.Empty;

        // determine if any entries exist
        if (JournalEntry.Entries.Count == 0)
        {
            lbxHistory.Items.Add(new ListBoxItem
            {
                Content = "No entries so far.",
            });
        }

        else
        {
            // populate the list
            lbxHistory.Items.Clear();
            foreach (JournalEntry entry in JournalEntry.Entries)
            {
                string displayText = entry.Content.Length > 50 ? entry.Content.Substring(0, 47) + "..." : entry.Content;
                ListBoxItem lbi = new ListBoxItem
                {
                    Tag = entry,
                    Content = new TextBlock
                    {
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        TextWrapping = TextWrapping.NoWrap,
                        Inlines =
                        {
                            new Run { Text = entry.Title, FontSize = 16, FontWeight = FontWeights.Bold },
                            new LineBreak(),
                            new Run { Text = displayText }
                        }
                    }
                };

                lbxHistory.Items.Add(lbi);
            }
        }

        return;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        LoadUI();
    }

    private void mfUpdate_Click(object sender, RoutedEventArgs e)
    {
        LoadUI();
    }

    private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.F5)
        {
            LoadUI();
        }
    }

    private void lbxHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lbxHistory.SelectedItem is ListBoxItem lbi)
        {
            if (lbi.Tag is JournalEntry entry)
            {
                txtPreviewTitle.Text = entry.Title;
                txtPreviewContent.Text = entry.Content;
            }
        }
    }
}
