namespace MoodTracker.Core;

/// <summary>
/// Represents the mood of a user in the Mood Tracker application.
/// </summary>
public enum Mood : byte
{
    /// <summary>
    /// Representing the default mood.
    /// </summary>
    None = 0,

    /// <summary>
    /// Representing thr Good (positive) mood.
    /// </summary>
    Good,

    /// <summary>
    /// Representing the Okay (neutral) mood.
    /// </summary>
    Okay,

    /// <summary>
    /// Representing the Bad (negative) mood.
    /// </summary>
    Bad
}
