using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Update.Core.Data;

/// <summary>
/// Representing a single RSS channel item.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct RssChannel
{
    /// <summary>
    /// Representing all entries in the RSS channel.
    /// </summary>
    public List<FeedEntry> Entries { get; set; }
}
