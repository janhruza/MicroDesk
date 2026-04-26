using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

using Windows.Foundation;

namespace Financier.Core;

/// <summary>
/// Representing a user profile structure.
/// </summary>
public class UserProfile
{
    /// <summary>
    /// Representing the profile namme.
    /// </summary>
    public string Name;

    /// <summary>
    /// Representing the saved window size and screen position info.
    /// </summary>
    public Rect WinPos;

    /// <summary>
    /// Representing the list of all transactions.
    /// </summary>
    public List<TransactionInfo> Transactions;

    /// <summary>
    /// Representing the user's preferred culture info for formatting and parsing numbers, dates, etc.
    /// </summary>
    public string Culture;

    /// <summary>
    /// Creates a new instance of the <see cref="UserProfile"/> class with default values.
    /// </summary>
    public UserProfile()
    {
        this.Transactions = [];
        this.Culture = "en-US";
        this.Name = string.Empty;
        this.WinPos = new Rect();
    }

    #region Profile methods

    private static UserProfile? _current = null;

    /// <summary>
    /// Determines whether any user profile is loaded.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if any user profile is loaded, otherwise <see langword="false"/>.
    /// </returns>
    public static bool IsLoaded()
    {
        return _current != null;
    }

    /// <summary>
    /// Gets the currently loaded profile or an empty profile if no profile is loaded.
    /// Use <see cref="IsLoaded"/> to determine whether any user profile is loaded.
    /// </summary>
    /// <returns></returns>
    public static UserProfile GetCurrent()
    {
        if (_current != null)
        {
            return _current;
        }

        else
        {
            return new UserProfile();
        }
    }

    /// <summary>
    /// Sets the currently loaded user profile.
    /// </summary>
    /// <param name="profile">The new active user profile. Can be null to signalize no actively loaded profile.</param>
    /// <returns>Value representing whether the new profile is valid to be loaded or not.</returns>
    public static bool SetCurrent(UserProfile? profile)
    {
        _current = profile;
        return _current != null;
    }

    private static JsonSerializerOptions _options = new JsonSerializerOptions
    {
        IncludeFields = true,
        WriteIndented = true
    };

    private static string _dataFolder { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");

    /// <summary>
    /// Folder where all user profiles are located.
    /// </summary>
    public static string ProfilesFolder => _dataFolder;

    private static string GetFileName(UserProfile? profile)
    {
        if (profile == null)
        {
            return string.Empty;
        }

        else
        {
            if (Directory.Exists(_dataFolder) == false)
            {
                _ = Directory.CreateDirectory(_dataFolder);
            }

            return Path.Combine(_dataFolder, $"{profile.Name}.json");
        }
    }

    /// <summary>
    /// Serializes the user profile into a JSON string and saves it to a file.
    /// </summary>
    /// <param name="profile">The <see cref="UserProfile"/> instance to be saved.</param>
    /// <returns>
    /// <c>true</c> if the profile was successfully serialized and saved; 
    /// <c>false</c> if the filename could not be determined.
    /// </returns>
    /// <exception cref="System.IO.IOException">Thrown when there is an error writing to the file.</exception>
    public static bool Save(UserProfile profile)
    {
        string filename = GetFileName(profile);
        if (string.IsNullOrWhiteSpace(filename))
        {
            return false;
        }

        if (App.MainWindow is not null)
        {
            profile.WinPos = new Rect(App.MainWindow.AppWindow.Position.X, App.MainWindow.AppWindow.Position.Y, App.MainWindow.AppWindow.Size.Width, App.MainWindow.AppWindow.Size.Height);
        }

        string data = JsonSerializer.Serialize<UserProfile>(profile, _options);
        _ = File.WriteAllTextAsync(filename, data, Encoding.UTF8);
        return true;
    }

    /// <summary>
    /// Deserializes a JSON string into a <see cref="UserProfile"/> instance.
    /// </summary>
    /// <param name="jsonData">The JSON string representing the user profile.</param>
    /// <param name="profile">
    /// When this method returns, contains the loaded <see cref="UserProfile"/> if successful, 
    /// or a new instance if the deserialization failed or the input was empty.
    /// </param>
    /// <returns>
    /// <c>true</c> if the profile was successfully loaded; 
    /// <c>false</c> if the input data was null/empty or deserialization failed.
    /// </returns>
    public static bool Load(string jsonData, out UserProfile profile)
    {
        if (string.IsNullOrWhiteSpace(jsonData))
        {
            profile = new UserProfile();
            return false;
        }

        UserProfile? loadedProfile = JsonSerializer.Deserialize<UserProfile>(jsonData, _options);
        if (loadedProfile == null)
        {
            profile = new UserProfile();
            return false;
        }

        // move and resize the app window to the saved position and size
        App.MainWindow?.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32((int)loadedProfile.WinPos.X, (int)loadedProfile.WinPos.Y, (int)loadedProfile.WinPos.Width, (int)loadedProfile.WinPos.Height));

        // profile has value
        profile = loadedProfile;
        return true;
    }

    /// <summary>
    /// Gets all user profiles found in the profiles folder. Each profile is expected to be stored as a JSON file with a .json extension.
    /// </summary>
    /// <returns>
    /// Collection of all user profiles found in the profiles folder. If the folder does not exist or an error occurs while reading files, an empty set is returned.
    /// </returns>
    public static HashSet<UserProfile> GetAllProfiles()
    {
        if (Directory.Exists(_dataFolder) == false)
        {
            _ = Directory.CreateDirectory(_dataFolder);
            return new HashSet<UserProfile>();
        }

        HashSet<UserProfile> profiles = new HashSet<UserProfile>();
        if (Directory.Exists(_dataFolder) == false)
        {
            return profiles;
        }
        string[] files = Directory.GetFiles(_dataFolder, "*.json");
        foreach (string file in files)
        {
            try
            {
                string data = File.ReadAllText(file, Encoding.UTF8);
                if (Load(data, out UserProfile profile))
                {
                    // only add profiles with valid names to the collection
                    if (string.IsNullOrWhiteSpace(profile.Name) == true) continue;
                    _ = profiles.Add(profile);
                }
            }
            catch (IOException)
            {
                // log error
                continue;
            }
        }
        return profiles;
    }

    #endregion
}
