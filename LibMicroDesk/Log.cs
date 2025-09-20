using System;
using System.IO;
using System.Text;
using System.Windows;

namespace LibMicroDesk;

/// <summary>
/// Representing the logging functionality of the MicroDesk library.
/// </summary>
public static class Log
{
    /// <summary>
    /// Represents the path to the log file used by the MicroDesk library.
    /// </summary>
    public static string Path => "MicroDesk.log";

    /// <summary>
    /// Determines whether the dialog window will popup if certain logging happens.
    /// </summary>
    public static bool ShowDialog { get; set; } = true;

    internal const char Separator = ';';

    static object _lock = new object();

    private static void WriteEntry(string message, string type, string tag = "Global")
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sb.Append(Separator);
        sb.Append(type);
        sb.Append(Separator);
        sb.Append(tag);
        sb.Append(Separator);
        sb.Append(message);
        sb.Append(Environment.NewLine);
        string data = sb.ToString();

        lock (_lock)
        {
            File.AppendAllText(Path, data, Encoding.Unicode);
        }
        
        return;
    }

    /// <summary>
    /// Writes an error message to the log file.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="tag">Identification tag.</param>
    public static void Error(string message, string tag = "Global")
    {
        WriteEntry(message, "Error", tag);

        if (ShowDialog)
        {
            _ = MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return;
    }

    /// <summary>
    /// Writes a warning message to the log file.
    /// </summary>
    /// <param name="message">Warning message.</param>
    /// <param name="tag">Identification tag.</param>
    public static void Warning(string message, string tag = "Global")
    {
        WriteEntry(message, "Warning", tag);
        return;
    }

    /// <summary>
    /// Writes an informational message to the log file.
    /// </summary>
    /// <param name="message">Informational message.</param>
    /// <param name="tag">Identification tag.</param>
    public static void Info(string message, string tag = "Global")
    {
        WriteEntry(message, "Info", tag);
        return;
    }

    /// <summary>
    /// Writes an error message to the log file from an exception.
    /// </summary>
    /// <param name="ex">An exception information.</param>
    /// <param name="tag">Identification tag.</param>
    public static void Error(Exception ex, string tag = "Global")
    {
        WriteEntry(ex.ToString(), "Error", tag);

        if (ShowDialog)
        {
            _ = MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return;
    }

    #region Predefined log messages

    /// <summary>
    /// Writes a message indicating that the application has started.
    /// </summary>
    public static void ApplicationStarted()
    {
        Info("Application started.", AppDomain.CurrentDomain.FriendlyName);
        return;
    }

    /// <summary>
    /// Writes a message indicating that the application has stopped.
    /// </summary>
    public static void ApplicationStopped()
    {
        Info("Application stopped.", AppDomain.CurrentDomain.FriendlyName);
        return;
    }

    #endregion
}
