namespace MDCore;

/// <summary>
/// Defines methods to serialize and deserialize objects of type T to and from a file.
/// </summary>
public interface IDumpable<T>
{
    /// <summary>
    /// Writes the specified data to a file with the given name.
    /// </summary>
    /// <param name="fileName">The path and name of the file to which the data will be written. Cannot be null or empty.</param>
    /// <returns>true if the data was successfully written to the file; otherwise, false.</returns>
    bool WriteToFile(string fileName);

    /// <summary>
    /// Attempts to read and deserialize data of type T from the specified file.
    /// </summary>
    /// <param name="fileName">The path to the file to read. Cannot be null or empty.</param>
    /// <param name="outputData">When this method returns, contains the deserialized data if the read operation was successful; otherwise, the default value of T.</param>
    /// <returns>true if the file was read and deserialized successfully; otherwise, false.</returns>
    bool ReadFromFile(string fileName, out T outputData);
}
