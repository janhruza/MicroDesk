using MDCore;

namespace Update.Core;

/// <summary>
/// Representing the basic app settings class.
/// </summary>
public class Settings : IDumpable
{
    /// <summary>
    /// Creates a new empty instance of the <see cref="Settings"/> class. This constructor is used to create a new settings object with default values.
    /// </summary>
    public Settings()
    {
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

    /// <inheritdoc/>
    public bool ReadFromFile(string fileName)
    {
        // TODO : Implement the method to read the settings from a file.
        return false;
    }

    /// <inheritdoc/>
    public bool WriteToFile(string fileName)
    {
        // TODO : Implement the method to write the settings to a file.
        return false;
    }
}
