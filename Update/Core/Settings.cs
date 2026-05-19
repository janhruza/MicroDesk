using MDCore;

using Microsoft.Windows.Widgets.Feeds.Providers;

using System;
using System.Collections.Generic;
using System.IO;

using Windows.ApplicationModel.UserDataTasks;

namespace Update.Core;

/// <summary>
/// Representing the basic app settings class.
/// </summary>
public class Settings : IDumpable
{
    internal const string _settingsPath = "settings.bin";

    /// <summary>
    /// Creates a new empty instance of the <see cref="Settings"/> class. This constructor is used to create a new settings object with default values.
    /// </summary>
    public Settings()
    {
        FeedsFile = string.Empty;
        Feeds = new HashSet<string>();
        Log.Info("Settings instance has been created.");
        return;
    }

    /// <summary>
    /// Destroys the instance of the <see cref="Settings"/> class. This destructor is used to clean up any resources used by the settings object.
    /// </summary>
    ~Settings()
    {
        Log.Info("Settings instance is being destroyed.");
    }

    /// <summary>
    /// Representing the RSS feeds file path.
    /// </summary>
    public string FeedsFile { get; set; }

    /// <summary>
    /// Representing the RSS feeds.
    /// </summary>
    public HashSet<string> Feeds { get; }

    /// <inheritdoc/>
    public bool ReadFromFile(string fileName = _settingsPath)
    {
        // read settings from file

        try
        {
            if (File.Exists(fileName) == false)
            {
                File.Create(fileName, 0).Close();
                this.FeedsFile = Path.GetTempFileName();
                return this.WriteToFile();
            }

            this.Feeds.Clear();
            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    int count = br.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string feedUrl = br.ReadString();
                        this.Feeds.Add(feedUrl);
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }

    /// <inheritdoc/>
    public bool WriteToFile(string fileName = _settingsPath)
    {
        // write settings to file

        try
        {
            using (FileStream fs = File.OpenWrite(fileName))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(Feeds.Count);
                    foreach (string str in Feeds)
                    {
                        bw.Write(str);
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }
}
