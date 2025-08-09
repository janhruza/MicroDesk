using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MoodTracker.Core;

namespace MoodTracker.Pages;

/// <summary>
/// Representing the mood selection page of the Mood Tracker application.
/// </summary>
public partial class PgMood : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgMood"/> class.
    /// </summary>
    public PgMood()
    {
        InitializeComponent();

        // assign mood tags to buttons
        tbGood.Tag = Mood.Good;
        tbOkay.Tag = Mood.Okay;
        tbBad.Tag = Mood.Bad;

        // mood buttons events
        foreach (ToggleButton tb in _moddButtons)
        {
            tb.Checked += (s, e) =>
            {
                // only check one button at a time
                foreach (ToggleButton other in _moddButtons)
                {
                    if (other != tb)
                    {
                        other.IsChecked = false;
                    }
                }
            };
        }

        this.Loaded += (s, e) =>
        {
            App.CenterWindow(App.Current.MainWindow);
        };
    }

    private ToggleButton[] _moddButtons => new ToggleButton[3]
    {
        tbGood,
        tbOkay,
        tbBad
    };

    private Mood GetSelectedMood()
    {
        foreach (ToggleButton tb in _moddButtons)
        {
            if (tb.IsChecked == true)
            {
                return (Mood)tb.Tag;
            }
        }
        return Mood.None; // or throw an exception
    }

    private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        App.Current.MainWindow.Close();
        return;
    }

    private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        // save the mood record
        Mood mActive = GetSelectedMood();
        MoodRecord record = new MoodRecord(DateTime.Now, mActive);

        if (App.RegisterMoodRecord(record) == false)
        {
            // Show error
            _ = MessageBox.Show($"Unable to save the mood report.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // close the window (not the app)
        App.Current.MainWindow.Close();
        return;
    }
}
