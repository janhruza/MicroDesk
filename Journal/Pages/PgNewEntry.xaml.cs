using Journal.Core;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Journal.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PgNewEntry : Page
{
    public PgNewEntry()
    {
        InitializeComponent();
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        JournalEntry entry = new JournalEntry();
        entry.Id = Guid.CreateVersion7();
        entry.Timestamp = DateTime.Now;
        entry.Title = this.txtTitle.Text ?? string.Empty;
        entry.Content = this.txtContent.Text ?? string.Empty;

        string msg;
        if (string.IsNullOrWhiteSpace(entry.Title) && string.IsNullOrWhiteSpace(entry.Content))
        {
            msg = "Invalid input. You must provide at least one of the two fields.";
            goto dialogDisplay;
        }


        if (JournalEntry.Register(entry) == false)
        {
            msg = "Unable to register your journal entry.";
        }

        else
        {
            msg = "Your journal entry was successfully registered!";
        }

    dialogDisplay:
        ContentDialog dlg = new ContentDialog
        {
            Content = msg,
            Title = "New Entry",
            XamlRoot = XamlRoot,
            PrimaryButtonText = "OK",
            IsSecondaryButtonEnabled = false
        };

        _ = await dlg.ShowAsync();
    }
}
