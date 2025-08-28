using System.IO;
using System.Windows.Controls;

namespace FileVault.Controls;

/// <summary>
/// Representing a custom filesystem item control.
/// </summary>
public partial class FileSystemItem : UserControl
{
    /// <summary>
    /// Creates a new instance of the <see cref="FileSystemItem"/> class.
    /// </summary>
    public FileSystemItem(string path)
    {
        InitializeComponent();
        this.Loaded += (s, e) => ReloadUI(path);
    }

    const string TYPE_FOLDER = "";
    const string TYPE_FILE = "";

    private void ReloadUI(string path)
    {
        if (Path.Exists(path) == false)
        {
            // path not found
            return;
        }

        else
        {
            FileSystemInfo info;
            if (File.Exists(path) == true)
            {
                // path is a file
                info = new FileInfo(path);

                // ui
                lblFileType.Content = TYPE_FILE;
            }

            else
            {
                // path is a directory
                info = new DirectoryInfo(path);

                // ui
                lblFileType.Content = TYPE_FOLDER;
            }

            lblName.Content = info.Name;
        }
    }
}
