namespace LibMicroDesk;

/// <summary>
/// Represents the application settings.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Constructs a new instance of the <see cref="AppSettings"/> class.
    /// </summary>
    public AppSettings()
    {
        Theme = SystemTheme.Auto; // Default theme
        Culture = System.Globalization.CultureInfo.CurrentCulture.Name;
    }

    /// <summary>
    /// GLobalization culture code.
    /// Represented as a culture name.
    /// Represents the current culture of the application, including the UI culture.
    /// </summary>
    public string Culture { get; set; }

    /// <summary>
    /// Represents the system theme of the application.
    /// </summary>
    public SystemTheme Theme { get; set; }

    #region Static code

    private static string _path => "AppSettings.json";

    /// <summary>
    /// Attempts to save the application settings to a file.
    /// </summary>
    /// <param name="settings">App settings instance.</param>
    /// <returns>Operation result.</returns>
    public static bool Save(AppSettings settings)
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(settings);
            System.IO.File.WriteAllText(_path, json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to load the application settings from a file.
    /// </summary>
    /// <param name="settings">Output <see cref="AppSettings"/> object.</param>
    /// <returns>Operation result.</returns>
    public static bool Load(out AppSettings settings)
    {
        settings = new AppSettings();
        try
        {
            if (System.IO.File.Exists(_path) == false)
            {
                return false;
            }

            var json = System.IO.File.ReadAllText(_path);
            settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion
}
