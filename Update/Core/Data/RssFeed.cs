using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Update.Core.Data;

/// <summary>
/// Representing a single RSS feed item.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct RssFeed
{
    /// <summary>
    /// Representing the title of the RSS feed.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Representing the link of the RSS feed.
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// Representing the list of channels in the RSS feed.
    /// </summary>
    public List<RssChannel> Channels { get; set; }

    /// <summary>
    /// Loads RSS feed data from the given <paramref name="feedUrl"/>.
    /// </summary>
    /// <param name="feedUrl">Url of the RSS feed.</param>
    /// <param name="result"></param>
    /// <returns>True, if the method succeeded, otherwise false.</returns>
    public static bool LoadFromUrl(string feedUrl, out RssFeed result)
    {
        // read the RSS feed url and parse it
        // read the RSS channels
        result = new RssFeed();
        return false;
    }
}
