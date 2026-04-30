using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Journal.Core;

[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true)]
[JsonSerializable(typeof(AppSettings))]
internal partial class AppSettingsSerializer : JsonSerializerContext
{
}

/// <summary>
/// Representing the app settings.
/// </summary>
public struct AppSettings
{
    /// <summary>
    /// Gets or sets the width value.
    /// </summary>
    public int Width;

    /// <summary>
    /// Gets or sets the height value.
    /// </summary>
    public int Height;

    /// <summary>
    /// Gets or sets the vertical position or offset value.
    /// </summary>
    public int Top;

    /// <summary>
    /// Represents the x-coordinate of the left edge.
    /// </summary>
    public int Left;

    private static string _settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
    private static AppSettings? _current = null;

    /// <summary>
    /// Retrieves the current application settings instance.
    /// </summary>
    /// <returns>The current <see cref="AppSettings"/> instance representing the application's configuration.</returns>
    public static AppSettings? GetCurrent() => _current;

    /// <summary>
    /// Sets the current application settings instance to the specified value.
    /// </summary>
    /// <remarks>Use this method to update or initialize the application's current settings. Passing null will
    /// reset the settings to their default values.</remarks>
    /// <param name="settings">The application settings to set as current. If null, a new instance of AppSettings is used.</param>
    public static void SetCurrent(AppSettings? settings)
    {
        if (settings.HasValue == false)
        {
            settings = new AppSettings();
        }

        _current = settings;
        return;
    }

    private static JsonSerializerOptions _options { get; } = new JsonSerializerOptions
    {
        IncludeFields = true,
        WriteIndented = true
    };

    /// <summary>
    /// Loads application settings from the configuration file if it exists.
    /// </summary>
    /// <remarks>If the configuration file is not found, the current settings remain unchanged. This method
    /// overwrites the current settings only when valid settings are successfully loaded from the file.</remarks>
    public static void LoadSettings()
    {
        if (File.Exists(_settingsPath) == false)
        {
            return;
        }

        string data = File.ReadAllText(_settingsPath, Encoding.UTF8);
        AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(data, AppSettingsSerializer.Default.AppSettings);
        if (settings.HasValue)
        {
            _current = settings.Value;
        }

        return;
    }

    /// <summary>
    /// Saves the current application settings to persistent storage using the configured file path and serialization
    /// options.
    /// </summary>
    /// <remarks>This method overwrites any existing settings file at the specified path. Changes made to the
    /// settings are not persisted until this method is called.</remarks>
    public static void SaveSettings()
    {
        string data = JsonSerializer.Serialize(_current, AppSettingsSerializer.Default.AppSettings);
        File.WriteAllText(_settingsPath, data, Encoding.UTF8);
        return;
    }
}
