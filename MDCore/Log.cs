using MDCore.Logging;

using System;
using System.IO;

namespace MDCore;

/// <summary>
/// Representing a simple file-based event logging class.
/// </summary>
public static class Log
{
    static FileLogger _logger { get; }

    static Log()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");
        _logger = new FileLogger(path);
    }

    /// <summary>
    /// Writes an informational message to the application's log using the configured logger.
    /// </summary>
    /// <param name="message">The message to log. Cannot be null.</param>
    public static void Info(string message)
    {
        _logger.Info(message);
    }

    /// <summary>
    /// Logs an error message to the application's logging system.
    /// </summary>
    /// <param name="message">The error message to log. Cannot be null.</param>
    public static void Error(string message)
    {
        _logger.Error(message);
    }

    /// <summary>
    /// Logs the specified exception as an error.
    /// </summary>
    /// <param name="ex">The exception to log. Cannot be null.</param>
    public static void Error(Exception ex)
    {
        _logger.Error(ex);
    }

    /// <summary>
    /// Writes a warning message to the application's log using the configured logger.
    /// </summary>
    /// <param name="message">The warning message to log. Cannot be null.</param>
    public static void Warning(string message)
    {
        _logger.Warning(message);
    }

    /// <summary>
    /// Logs a success message to the application's logging system.
    /// </summary>
    /// <param name="message">The message to log. Cannot be null.</param>
    public static void Success(string message)
    {
        _logger.Success(message);
    }
}
