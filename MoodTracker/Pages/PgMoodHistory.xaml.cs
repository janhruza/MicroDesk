using System.IO;
using System.Windows.Controls;

namespace MoodTracker.Pages;

/// <summary>
/// Representing the mood history page of the Mood Tracker application.
/// </summary>
public partial class PgMoodHistory : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgMoodHistory"/> class.
    /// </summary>
    public PgMoodHistory()
    {
        InitializeComponent();

        // refresh the mood history on page load
        RefreshHistory();
    }

    private static string[] moodChars => new string[3]
    {
        "😃",    // good
        "😐",    // okay
        "😞"     // bad
    };

    private void RefreshHistory()
    {
        if (File.Exists(App._moodFile) == false)
        {
            rGood.Text = "0";
            rOkay.Text = "0";
            rBad.Text = "0";
            return;
        }

        // read mood history file
        if (App.ReadMoodHistoryFile(out MoodTracker.Core.MoodRecord[] moodRecords) == false)
        {
            rGood.Text = "0";
            rOkay.Text = "0";
            rBad.Text = "0";
            return;
        }

        // process mood records
        int goodCount = 0;
        int okayCount = 0;
        int badCount = 0;

        foreach (MoodTracker.Core.MoodRecord record in moodRecords)
        {
            switch (record.Mood)
            {
                case MoodTracker.Core.Mood.Good:
                    goodCount++;
                    break;
                case MoodTracker.Core.Mood.Okay:
                    okayCount++;
                    break;
                case MoodTracker.Core.Mood.Bad:
                    badCount++;
                    break;
            }
        }

        rGood.Text = goodCount.ToString();
        rOkay.Text = okayCount.ToString();
        rBad.Text = badCount.ToString();

        // get average mood
        int totalCount = goodCount + okayCount + badCount;
        if (totalCount > 0)
        {
            double averageMood = (goodCount * 2 + okayCount) / (double)totalCount;
            lblAverage.Content = averageMood.ToString("F2");

            // get average mood character
            if (averageMood >= 1.5)
            {
                tbAverage.Text = moodChars[0]; // good
            }
            else if (averageMood >= 0.5)
            {
                tbAverage.Text = moodChars[1]; // okay
            }
            else
            {
                tbAverage.Text = moodChars[2]; // bad
            }
        }
        else
        {
            lblAverage.Content = "N/A"; // no records to calculate average
        }
    }
}
