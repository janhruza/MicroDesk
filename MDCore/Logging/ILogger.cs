using System;

namespace MDCore.Logging;

/// <summary>
/// Representing the base logging interface.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Writes an informational message to the log at the Info level.
    /// </summary>
    /// <remarks>Use this method to record general informational events that highlight the progress of the
    /// application at a coarse-grained level. The message will typically be visible in logs configured to include
    /// informational output.</remarks>
    /// <param name="message">The message to log. Cannot be null.</param>
    void Info(string message);

    /// <summary>
    /// Logs an error message to the configured logging target.
    /// </summary>
    /// <param name="message">The error message to log. Cannot be null or empty.</param>
    void Error(string message);

    /// <summary>
    /// Logs an error event with the specified exception information.
    /// </summary>
    /// <remarks>Use this method to record error details for diagnostic or auditing purposes. The
    /// implementation may include additional context such as stack trace or inner exceptions when logging the
    /// error.</remarks>
    /// <param name="ex">The exception that describes the error to log. Cannot be null.</param>
    void Error(Exception ex);

    /// <summary>
    /// Writes a warning message to the log.
    /// </summary>
    /// <remarks>Use this method to record non-critical issues or potential problems that do not prevent
    /// normal operation. Warning messages are typically used to highlight unexpected situations that may require
    /// attention.</remarks>
    /// <param name="message">The warning message to log. Cannot be null.</param>
    void Warning(string message);

    /// <summary>
    /// Writes a success message to the log.
    /// </summary>
    /// <param name="message">The success message to log. Cannot be null.</param>
    void Success(string message);
}
