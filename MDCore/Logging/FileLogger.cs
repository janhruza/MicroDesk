using System;
using System.IO;
using System.Text;

namespace MDCore.Logging;

/// <summary>
/// Representing a simple file-based logger.
/// </summary>
/// <remarks>
/// Creates a new <see cref="FileLogger"/> instance.
/// </remarks>
/// <param name="filename">Target log file path.</param>
public sealed class FileLogger(string filename = "app.log") : ILogger
{
    private const char _sep = '|';
    private string _filename = filename;

    private bool WriteMessage(string type, string message)
    {
        if (File.Exists(_filename) == false)
        {
            // error
            return false;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append(DateTime.Now.ToString());
        sb.Append(_sep);
        sb.Append(type);
        sb.Append(_sep);
        sb.Append(message);
        sb.Append(Environment.NewLine);
        File.AppendAllText(_filename, sb.ToString(), Encoding.UTF8);
        return true;
    }

    /// <inheritdoc />
    public void Error(string message)
    {
        _ = WriteMessage("ERROR", message);
    }

    /// <inheritdoc />
    public void Error(Exception ex)
    {
        _ = WriteMessage("ERROR", ex.ToString());
    }

    /// <inheritdoc />
    public void Info(string message)
    {
        _ = WriteMessage("INFO", message);
    }

    /// <inheritdoc />
    public void Success(string message)
    {
        _ =WriteMessage("INFO", message);
    }

    /// <inheritdoc />
    public void Warning(string message)
    {
        _ = WriteMessage("WARNING", message);
    }
}
