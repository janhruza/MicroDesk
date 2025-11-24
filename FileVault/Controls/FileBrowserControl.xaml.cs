using LibMicroDesk;

using System;
using System.IO;
using System.Windows.Controls;

namespace FileVault.Controls;

/// <summary>
/// Representing the file browser control.
/// </summary>
public partial class FileBrowserControl : UserControl
{
    /// <summary>
    /// Creates a new instance of the <see cref="FileBrowserControl"/> class.
    /// </summary>
    public FileBrowserControl()
    {
        InitializeComponent();
        _path = string.Empty;
    }

    /// <summary>
    /// Create a new instance of the <see cref="FileBrowserControl"/> class and navigates to the <paramref name="path"/> directory.
    /// </summary>
    /// <param name="path"></param>
    public FileBrowserControl(string path)
    {
        InitializeComponent();
        _path = path;
        this.Loaded += (s, e) => Navigate(path);
    }

    private string _path;

    /// <summary>
    /// Navigates to the specified directory.
    /// </summary>
    /// <param name="path">Path to the target directory.</param>
    public void Navigate(string path)
    {
        try
        {
            if (Directory.Exists(path) == false)
            {
                // folder not found
                return;
            }

            lbxItems.Items.Clear();

            // refresh UI

            // go back item
            ListBoxItem lbiBack = new ListBoxItem
            {
                Content = "[Parent directory]"
            };

            lbiBack.MouseDoubleClick += (s, e) =>
            {
                var parent = Directory.GetParent(path);
                if (parent == null)
                {
                    // go to the drives page
                    App.Navigate(App.PgDrives);
                }

                else
                {
                    string path = parent.FullName;
                    Navigate(path);
                }
            };

            // add go-back item
            lbxItems.Items.Add(lbiBack);

            // get directories
            foreach (string dir in Directory.GetDirectories(path))
            {
                ListBoxItem lbi = new ListBoxItem
                {
                    Content = new FileSystemItem(dir)
                    {
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                    },

                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                };

                lbi.MouseDoubleClick += (s, e) =>
                {
                    // navigate to the directory
                    Navigate(dir);
                    return;
                };

                lbxItems.Items.Add(lbi);
            }

            // get files
            foreach (string file in Directory.GetFiles(path))
            {
                ListBoxItem lbi = new ListBoxItem
                {
                    Content = new FileSystemItem(file)
                    {
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                    },

                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch
                };

                lbi.MouseDoubleClick += (s, e) =>
                {
                    // open file
                    MDCore.CreateShellProcess(file, out _);
                    return;
                };

                lbxItems.Items.Add(lbi);
            }

            App.SetStatusMessage(path);
            return;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(FileBrowserControl.Navigate));
        }
    }
}
