using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

using Windows.Foundation;

namespace Financier.Core;

/// <summary>
/// Representing a user profile structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct UserProfile
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

    #region Profile methods

    static UserProfile? _current = null;

    /// <summary>
    /// Determines whether any user profile is loaded.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if any user profile is loaded, otherwise <see langword="false"/>.
    /// </returns>
    public static bool IsLoaded()
    {
        return _current.HasValue;
    }

    /// <summary>
    /// Gets the currently loaded profile or an empty profile if no profile is loaded.
    /// Use <see cref="IsLoaded"/> to determine whether any user profile is loaded.
    /// </summary>
    /// <returns></returns>
    public static UserProfile GetCurrent()
    {
        if (_current.HasValue)
        {
            return _current.Value;
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
        return _current.HasValue;
    }

    static JsonSerializerOptions _options = new JsonSerializerOptions
    {
        IncludeFields = true
    };

    static string _dataFolder { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles");
    static string GetFileName(UserProfile? profile)
    {
        if (profile.HasValue == false)
        {
            return string.Empty;
        }

        else
        {
            return Path.Combine(_dataFolder, $"{profile.Value.Name}.json");
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

        string data = JsonSerializer.Serialize<UserProfile>(profile, _options);
        File.WriteAllTextAsync(filename, data, Encoding.UTF8);
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
        if (loadedProfile.HasValue == false)
        {
            profile = new UserProfile();
            return false;
        }

        // profile has value
        profile = loadedProfile.Value;
        return true;
    }

    #endregion
}
