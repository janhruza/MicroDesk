using Journal.Core;

using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;

using System;

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

    private ListBoxItem LbiNoItems()
    {
        ListBoxItem item = new ListBoxItem
        {
            Content = "No entries so far."
        };

        return item;
    }

    private void LoadUI()
    {
        // clear first
        this.lbxHistory.Items.Clear();
        this.txtPreviewTitle.Text = string.Empty;
        this.txtPreviewContent.Text = string.Empty;

        // determine if any entries exist
        if (JournalEntry.Entries.Count == 0)
        {
            ListBoxItem lbiNoItems = LbiNoItems();
            lbiNoItems.IsSelected = true;
            this.lbxHistory.Items.Add(lbiNoItems);
            return;
        }

        else
        {
            // populate the list
            foreach (JournalEntry entry in JournalEntry.Entries)
            {
                string displayText = entry.Content.Length > 50 ? entry.Content[..47] + "..." : entry.Content;
                ListBoxItem lbi = new ListBoxItem
                {
                    Tag = entry,
                    Content = new TextBlock
                    {
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        TextWrapping = TextWrapping.NoWrap,
                        Inlines =
                        {
                            new Run { Text = entry.Title, FontSize = 18, FontWeight = FontWeights.SemiBold },
                            new LineBreak(),
                            new Run { Text = displayText, FontSize = 14 }
                        }
                    }
                };

                MenuFlyoutItem mfiDelete = new MenuFlyoutItem
                {
                    Text = "Delete",
                    KeyboardAccelerators =
                    {
                        new KeyboardAccelerator()
                        {
                            Key = Windows.System.VirtualKey.Delete
                        }
                    }
                };

                // evil, spaghetti, but works
                mfiDelete.Click += async (s, e) =>
                {
                    // confirm & remove
                    ContentDialog dlg = new ContentDialog
                    {
                        XamlRoot = XamlRoot,
                        Title = $"Delete \'{entry.Title ?? "Untitled"}\'",
                        Content = "Do you wan to delete this entry? This action is irreversable.",
                        PrimaryButtonText = "Delete",
                        CloseButtonText = "Cancel",
                        DefaultButton = ContentDialogButton.Close
                    };

                    var res = await dlg.ShowAsync();
                    if (res == ContentDialogResult.Primary)
                    {
                        // delete
                        if (JournalEntry.Unregister(entry) == true)
                        {
                            LoadUI();
                        }
                    }
                };

                lbi.ContextFlyout = new MenuFlyout
                {
                    Items =
                    {
                        mfiDelete
                    }
                };

                this.lbxHistory.Items.Add(lbi);
            }

            // select the first item
            this.lbxHistory.SelectedIndex = 0;
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
        if (this.lbxHistory.SelectedItem is ListBoxItem lbi)
        {
            if (lbi.Tag is JournalEntry entry)
            {
                this.txtPreviewTitle.Text = entry.Title;
                this.txtPreviewContent.Text = entry.Content;
                this.txtPreviewTitle.Visibility = Visibility.Visible;
                this.txtPreviewContent.Visibility = Visibility.Visible;
            }

            else
            {
                this.txtPreviewTitle.Visibility = Visibility.Collapsed;
                this.txtPreviewContent.Visibility = Visibility.Collapsed;
            }
        }
    }
}
