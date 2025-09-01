using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Headlines.Core;
using Headlines.Pages;
using LibMicroDesk;

namespace Headlines;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Representing the app title.
    /// </summary>
    public const string AppName = "Headlines";

    const string SourcesFile = "sources.txt";

    internal static PgOverview PgOverview => new PgOverview();

    static App()
    {
        RssFeedSources = new List<string>();
    }

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        // load app settings
        MDCore.EnsureSettings();

        // load rss sources list
        await LoadSources();

        // create and display the main window
        Headlines.MainWindow mw = new Headlines.MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private async void Application_Exit(object sender, ExitEventArgs e)
    {
        // post exit cleanup
        await SaveSources();
        return;
    }

    internal static List<string> RssFeedSources { get; set; }

    private async Task LoadSources()
    {
        RssFeedSources.Clear();

        if (File.Exists(SourcesFile) == false)
        {
            return;
        }

        string[] lines = await File.ReadAllLinesAsync(SourcesFile);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) == false)
            {
                RssFeedSources.Add(line.Trim());
            }
        }

        return;
    }

    private async Task SaveSources()
    {
        await File.WriteAllLinesAsync(SourcesFile, RssFeedSources);
        return;
    }

    internal static Task FetchAllFeeds(List<RssFeed> feeds)
    {
        List<Task> tasks = new List<Task>();
        foreach (string source in RssFeedSources)
        {
            tasks.Add(Task.Run(() =>
            {
                RssFeed feed = RssParser.Parse(source);
                feeds.Add(feed);
            }));
        }
        return Task.WhenAll(tasks);
    }
}
