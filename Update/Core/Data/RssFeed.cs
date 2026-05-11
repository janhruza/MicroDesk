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
}
