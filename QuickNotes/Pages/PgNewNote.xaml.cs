using System;
using System.Windows;
using System.Windows.Controls;
using QuickNotes.Core;

namespace QuickNotes.Pages;

/// <summary>
/// Representing a new note page.
/// </summary>
public partial class PgNewNote : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgNewNote"/> class.
    /// </summary>
    public PgNewNote()
    {
        InitializeComponent();
    }

    private void cbCanExpire_Checked(object sender, System.Windows.RoutedEventArgs e)
    {
        // show expiration date box
        dtExpiration.Visibility = System.Windows.Visibility.Visible;
    }

    private void cbCanExpire_Unchecked(object sender, System.Windows.RoutedEventArgs e)
    {
        // hide expiration date box
        dtExpiration.Visibility = System.Windows.Visibility.Collapsed;
    }

    private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        MainWindow.Navigate(App.OverviewPage);
        return;
    }

    private bool ValidateNote(out string text, out DateTime creation, out DateTime expiration)
    {
        DateTime dtCurrent = DateTime.Now;

        // check text first
        text = txtText.Text;
        if (string.IsNullOrEmpty(text)) goto Error;
        if (string.IsNullOrWhiteSpace(text)) goto Error;

        creation = dtCurrent;

        if (cbCanExpire.IsChecked == true)
        {
            if (dtExpiration.SelectedDate.HasValue == false)
            {
                expiration = DateTime.MinValue;
                return false;
            }

            expiration = dtExpiration.SelectedDate.Value;
            return true;
        }

        else
        {
            // creation date is expiration date => indifinite
            expiration = dtCurrent;
            return true;
        }

    Error:
        creation = dtCurrent;
        expiration = DateTime.MinValue;
        return false;
    }

    private void btnCreate_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        // check if note is valid and create it

        if (ValidateNote(out string text, out DateTime creation, out DateTime expiration) == true)
        {
            // create the note
            Note note = new Note(text, creation, expiration);
            App.Notes.Add(note);

            // navigate to the overview page
            MainWindow.Navigate(App.OverviewPage);
            return;
        }

        else
        {
            // can't create note
            App.ShowMessage("Unable to create a new note. Please verify the input validity of all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }
}
