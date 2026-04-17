using Journal.Core;

using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;

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
        this.txtPreviewTitle.Text = string.Empty;
        this.txtPreviewContent.Text = string.Empty;

        // determine if any entries exist
        if (JournalEntry.Entries.Count == 0)
        {
            this.lbxHistory.Items.Add(new ListBoxItem
            {
                Content = "No entries so far.",
            });
        }

        else
        {
            // populate the list
            this.lbxHistory.Items.Clear();
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
                            new Run { Text = entry.Title, FontSize = 16, FontWeight = FontWeights.Bold },
                            new LineBreak(),
                            new Run { Text = displayText }
                        }
                    }
                };

                this.lbxHistory.Items.Add(lbi);
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
        if (this.lbxHistory.SelectedItem is ListBoxItem lbi)
        {
            if (lbi.Tag is JournalEntry entry)
            {
                this.txtPreviewTitle.Text = entry.Title;
                this.txtPreviewContent.Text = entry.Content;
            }
        }
    }
}
