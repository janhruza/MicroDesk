namespace Update.Core.Data;

/// <summary>
/// Representing the RSS service.
/// </summary>
public static class RSS
{
    /// <summary>
    /// Registers a new RSS feed to the active settings file.
    /// </summary>
    /// <param name="feedUrl">The feed URL to add.</param>
    /// <returns>True, if the feed was added; otherwise false.</returns>
    public static bool RegisterFeed(string feedUrl)
    {
        return App.CurrentSettings.Feeds.Add(feedUrl);
    }

    /// <summary>
    /// Unregisters the specified feed URL from the application's feed collection.
    /// </summary>
    /// <remarks>Removes the URL from App.CurrentSettings.Feeds.</remarks>
    /// <param name="feedUrl">The feed URL to remove.</param>
    /// <returns>True if the feed was removed; otherwise, false.</returns>
    public static bool UnregisterFeed(string feedUrl)
    {
        return App.CurrentSettings.Feeds.Remove(feedUrl);
    }
}
