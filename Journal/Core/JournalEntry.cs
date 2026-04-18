using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Journal.Core;

/// <summary>
/// Representing a single journal entry.
/// </summary>
public struct JournalEntry
{
    /// <summary>
    /// Id of the entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Journal entry's title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Date of the entry.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Content of the journal entry.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Representing a single journal entry.
    /// </summary>
    public JournalEntry()
    {
        Id = Guid.CreateVersion7();
        Timestamp = DateTime.Now;
        Title = string.Empty;
        Content = string.Empty;
    }

    #region Static methods

    private static string sDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Journals");
    private static string sJournalFile = Path.Combine(sDataDir, $"{Environment.UserName}.journal");

    /// <summary>
    /// Representing the in-memory list of all existing user journal entries.
    /// </summary>
    public static List<JournalEntry> Entries { get; } = new List<JournalEntry>();

    /// <summary>
    /// Set-ups all the required properties.
    /// </summary>
    public static void SetupJournals()
    {
        if (Directory.Exists(sDataDir) == false)
        {
            _ = Directory.CreateDirectory(sDataDir);
        }

        return;
    }

    private static void CopyList(List<JournalEntry> src, List<JournalEntry> dst)
    {
        dst.Clear();
        foreach (JournalEntry entry in src)
        {
            dst.Add(entry);
        }
    }

    /// <summary>
    /// Loads all user's stored entries.
    /// </summary>
    /// <returns>List of the stored entries.</returns>
    public static List<JournalEntry> LoadEntries()
    {
        if (File.Exists(sJournalFile) == false)
        {
            return [];
        }

        string data = File.ReadAllText(sJournalFile, Encoding.Unicode);
        List<JournalEntry>? entries = JsonSerializer.Deserialize<List<JournalEntry>>(data) ?? new List<JournalEntry>();
        CopyList(entries, Entries);
        return entries;
    }

    /// <summary>
    /// Saves the given list of entries into the user's journal file.
    /// </summary>
    /// <param name="entries">List of journal entries.</param>
    /// <returns>Operation result.</returns>
    public static bool SaveEntries(List<JournalEntry> entries)
    {
        string json = JsonSerializer.Serialize<List<JournalEntry>>(entries);
        if (string.IsNullOrWhiteSpace(json))
        {
            return false;
        }

        File.WriteAllText(sJournalFile, json, Encoding.Unicode);
        return true;
    }

    /// <summary>
    /// Registers the journal <paramref name="entry"/> in the global <see cref="Entries"/> list.
    /// </summary>
    /// <param name="entry">The new journal entry.</param>
    /// <returns>Operation result.</returns>
    public static bool Register(JournalEntry entry)
    {
        Entries.Add(entry);
        return Entries.Contains(entry);
    }

    /// <summary>
    /// Removes the journal <paramref name="entry"/> from the global <see cref="Entries"/> list.
    /// </summary>
    /// <param name="entry">The target entry.</param>
    /// <returns>Operation result.</returns>
    public static bool Unregister(JournalEntry entry)
    {
        return Entries.Remove(entry);
    }

    #endregion
}
