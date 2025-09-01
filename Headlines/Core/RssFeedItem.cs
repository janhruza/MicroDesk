namespace Headlines.Core;

/// <summary>
/// Representing a single RSS feed entry item.
/// </summary>
public struct RssFeedItem
{
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
    /// Representing the feed item publication date.
    /// </summary>
    public string Date;

    /// <summary>
    /// Representing the feed item unique identifier.
    /// </summary>
    public string Guid;
}
