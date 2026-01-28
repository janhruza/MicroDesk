using FileVault.Controls;

using System;
using System.IO;
using System.Windows.Controls;

namespace FileVault.Pages;

/// <summary>
/// Representing a drives overview page.
/// </summary>
public partial class PgDrives : Page
{
    /// <summary>
    /// Creates a new <see cref="PgDrives"/> instance.
    /// </summary>
    public PgDrives()
    {
        InitializeComponent();

        Loaded += (s, e) => RefreshDrives();
    }

    private void RefreshDrives()
    {
        // clear data
        ugAvailable.Children.Clear();
        ugUnavailable.Children.Clear();

        // get drives
        string[] drives = Environment.GetLogicalDrives();

        foreach (string drive in drives)
        {
            DriveInfo di = new DriveInfo(drive);
            if (di.IsReady)
            {
                // drive is ready
                Button btn = new Button
                {
                    Content = new DriveButton(drive)
                    {
                        Width = double.NaN
                    },
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(10, 0, 10, 0)
                };

                btn.Click += (s, e) =>
                {
                    App.Navigate(new Page
                    {
                        Content = new FileBrowserControl(drive)
                    });
                };

                ugAvailable.Children.Add(btn);
            }

            else
            {
                // drive not ready
                Button btn = new Button
                {
                    Content = new DriveButton(drive),
                    IsEnabled = false,
                    Margin = new System.Windows.Thickness(10, 0, 10, 0)
                };

                ugUnavailable.Children.Add(btn);
            }
        }
    }
}
