using MDCore;

using System;
using System.IO;

namespace Update.Core.Profiles;

/// <summary>
/// Representing the user profile, which contains all the information about the user, such as the username, password, email, etc.
/// </summary>
public class UserProfile : IDumpable<UserProfile>
{
    /// <summary>
    /// Representing the folder where all the user profiles are stored. The user profiles are stored in the "Profiles" folder in the base directory of the application.
    /// </summary>
    public static string ProfilesFolder { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");

    /// <summary>
    /// Creates a new instance of the <see cref="UserProfile"/> class. This constructor is used to create a new user profile.
    /// </summary>
    public UserProfile()
    {
        Id = Guid.NewGuid();
        Name = Environment.UserName;
    }

    /// <summary>
    /// Representing the unique identifier of the user profile. The ID is used to identify the user profile and is unique for each user profile.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Representing the username of the user. The username is used to identify the user and is unique for each user profile.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Representing the folder where all the data of the user profile is stored. The data of the user profile is stored in a folder with the name of the ID of the user profile in the "Profiles" folder.
    /// </summary>
    public string DataFolder => Path.Combine(ProfilesFolder, Id.ToString());

    public bool WriteToFile(string fileName)
    {
        // TODO: Implement the method to write the user profile to a file. The file should be stored in the "Profiles" folder with the name of the ID of the user profile.
        return false;
    }

    public bool ReadFromFile(string fileName, out UserProfile profile)
    {
        // TODO: Implement the method to read the user profile from a file. The file should be stored in the "Profiles" folder with the name of the ID of the user profile.
        profile = new UserProfile();
        return false;
    }
}
