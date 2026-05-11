using System.Runtime.InteropServices;

namespace Update.Core.Data;

/// <summary>
/// Representing a single RSS channel feed entry item.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct FeedEntry
{
    /// <summary>
    /// Representing the title of the RSS channel feed entry.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Representing the link of the RSS channel feed entry.
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// Representing the content of the RSS channel feed entry.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Representing the timestamp of the RSS channel feed entry.
    /// </summary>
    public string Timestamp { get; set; }
}
