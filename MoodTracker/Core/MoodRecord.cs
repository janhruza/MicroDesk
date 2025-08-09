namespace MoodTracker.Core;

/// <summary>
/// Representing the mood record data structure.
/// </summary>
/// <param name="Timestamp"></param>
/// <param name="Mood"></param>
public record struct MoodRecord(DateTime Timestamp, Mood Mood)
{
    /// <summary>
    /// Gets the mood record ina premade CSV format.
    /// </summary>
    /// <returns>A CSV-formatted mood record.</returns>
    public string ToCSV()
    {
        return $"{this.Timestamp};{this.Mood}";
    }
}
