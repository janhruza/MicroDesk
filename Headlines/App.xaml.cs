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

        // log entry
        Log.ApplicationStarted();
    }

    private async void Application_Exit(object sender, ExitEventArgs e)
    {
        // post exit cleanup
        await SaveSources();

        // log entry
        Log.ApplicationStopped();
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

    internal static async Task FetchAllFeeds(List<RssFeed> feeds)
    {
        var tasks = new List<Task<RssFeed>>();

        foreach (string source in RssFeedSources)
        {
            tasks.Add(Task.Run(() =>
            {
                var feed = RssReader.Parse(source);
                return feed;
            }));
        }

        var results = await Task.WhenAll(tasks);
        feeds.AddRange(results);
    }
}
