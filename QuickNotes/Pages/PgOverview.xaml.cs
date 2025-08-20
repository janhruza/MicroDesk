using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using QuickNotes.Core;

namespace QuickNotes.Pages;

/// <summary>
/// Representing the overview (dashboard) page.
/// </summary>
public partial class PgOverview : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgOverview"/> class.
    /// </summary>
    public PgOverview()
    {
        InitializeComponent();
        this.RefreshUI();
    }

    private Button CreateDeleteButton(Note? target)
    {
        Button btn = new Button
        {
            Content = "",   // delete character
            FontFamily = new System.Windows.Media.FontFamily("Segoe Fluent Icons"),
            Visibility = System.Windows.Visibility.Collapsed,
            Margin = new Thickness(5, 0, 0, 0)
        };

        btn.Click += delegate
        {
            // delete the note
            if (target.HasValue == false)
            {
                return;
            }

            var note = target.Value;
            if (MessageBox.Show($"Do you want to delete the \'{note.Text}\' note? This action is irreversible.", "Delete note?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // remove the note from the notes list
                if (App.Notes.Remove(note) == false)
                {
                    // can't remove note
                    _ = MessageBox.Show("Unable to remove the selected note. Please try again later.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                RefreshUI();
                return;
            }
        };

        return btn;
    }

    private bool _showAllNotes => cbShowAllNotes.IsChecked == true;

    private void RefreshUI()
    {
        // load all notes and display them
        // into the list box
        lbxNotes.Items.Clear();

        if (App.Notes.Count == 0)
        {
            // no need to refresh the items list
            return;
        }

        foreach (Note note in App.Notes)
        {
            if (_showAllNotes == false)
            {
                // exclude expired notes
                if (note.HasExpired())
                {
                    continue;
                }
            }

            // create item's content
            Button btnDelete = CreateDeleteButton(note);

            ListBoxItem lbiNote = new ListBoxItem
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(),
                        new ColumnDefinition { Width = GridLength.Auto }
                    },

                    Children =
                    {
                        new Label
                        {
                            //Content = note.Text,
                            Content = new TextBlock
                            {
                                Inlines =
                                {
                                    new Run
                                    {
                                        FontSize = 16,
                                        Text = note.Text
                                    },

                                    new LineBreak(),

                                    new Run
                                    {
                                        FontSize = 12,
                                        Text = (note.Expiration == note.Creation ? "Indefinite" : note.Expiration.ToString("d"))
                                    }
                                }
                            },

                            VerticalAlignment = VerticalAlignment.Center
                        },

                        btnDelete
                    }
                }
            };

            Grid.SetColumn(btnDelete, 1);

            lbiNote.MouseEnter += (s, e) => btnDelete.Visibility = Visibility.Visible;
            lbiNote.MouseLeave += (s, e) => btnDelete.Visibility = Visibility.Collapsed;

            lbiNote.Tag = note;
            lbxNotes.Items.Add(lbiNote);
        }

        return;
    }

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
        this.RefreshUI();
        return;
    }

    private void btnNewNote_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Navigate(App.NewNotePage);
        return;
    }

    private void cbShowAllNotes_Checked(object sender, RoutedEventArgs e)
    {
        // refresh as the visibility could change
        this.RefreshUI();
        return;
    }

    private void cbShowAllNotes_Unchecked(object sender, RoutedEventArgs e)
    {
        // refresh as the visibility could change
        this.RefreshUI();
        return;
    }
}
