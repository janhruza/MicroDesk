using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Journal.Core;

/// <summary>
/// Representing a single journal entry.
/// </summary>
public class JournalEntry
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

    public JournalEntry()
    {
        Id = Guid.CreateVersion7();
        Timestamp = DateTime.Now;
        Title = string.Empty;
        Content = string.Empty;
    }

    #region Static methods

    static string sDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Journals");
    static string sJournalFile = Path.Combine(sDataDir, $"{Environment.UserName}.journal");

    public static List<JournalEntry> Entries { get; } = new List<JournalEntry>();

    public static void SetupJournals()
    {
        if (Directory.Exists(sDataDir) == false)
        {
            _ = Directory.CreateDirectory(sDataDir);
        }

        return;
    }

    static void CopyList(List<JournalEntry> src,  List<JournalEntry> dst)
    {
        dst.Clear();
        foreach (JournalEntry entry in src)
        {
            dst.Add(entry);
        }
    }

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


    public static bool Register(JournalEntry entry)
    {
        Entries.Add(entry);
        return true;
    }

    #endregion
}
