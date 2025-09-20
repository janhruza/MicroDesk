using System.Collections.Generic;

namespace Headlines.Core;

/// <summary>
/// Representing a single RSS feed item.
/// </summary>
public struct RssFeed
{
    /// <summary>
    /// Initializes the struct.
    /// </summary>
    public RssFeed()
    {
        Title = string.Empty;
        Link = string.Empty;
        Description = string.Empty;
        Image = string.Empty;
        Items = [];
    }

    /// <summary>
    /// Representing the feed title.
    /// </summary>
    public string Title;

    /// <summary>
    /// Representing the feed source link.
    /// </summary>
    public string Link;

    /// <summary>
    /// Representing the feed description.
    /// </summary>
    public string Description;

    /// <summary>
    /// Representing the feed channel image (if any).
    /// </summary>
    public string Image;

    /// <summary>
    /// Representing the feed items.
    /// </summary>
    public List<RssFeedItem> Items;
}
