using LibMicroDesk;

using System;
using System.IO;
using System.Windows.Controls;

namespace FileVault.Controls;

/// <summary>
/// Representing a simple drive button.
/// </summary>
public partial class DriveButton : UserControl
{
    /// <summary>
    /// Creates a new instance of the <see cref="DriveButton"/> class.
    /// </summary>
    /// <param name="drive">Target drive letter. Use <see cref="Environment.GetLogicalDrives"/> to get a vlaid drive string value.</param>
    public DriveButton(string? drive)
    {
        // load ui
        InitializeComponent();
        _drive = drive ?? string.Empty;

        // events
        Loaded += (s, e) => RefreshDriveInfo();
    }

    private string _drive;

    private long GetGB(long bytes)
    {
        return bytes / 1_073_741_824;
    }

    private string GetLabel(string driveLabel)
    {
        if (string.IsNullOrEmpty(driveLabel.Trim()) == false) return driveLabel;
        else
        {
            return "Local Disk";
        }
    }

    private void RefreshDriveInfo()
    {
        // draw UI
        if (string.IsNullOrEmpty(_drive) == true)
        {
            // no drive selected
            return;
        }

        // display drive info
        try
        {
            DriveInfo di = new DriveInfo(_drive);
            if (di.IsReady == false)
            {
                // drive not ready
                rLetter.Text = _drive.Replace("\\", string.Empty);
                rLabel.Text = "Drive";
                pbFree.Visibility = System.Windows.Visibility.Collapsed;
                lblFreeSpace.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            // general
            rLetter.Text = _drive.Replace("\\", string.Empty);
            rLabel.Text = GetLabel(di.VolumeLabel);
            pbFree.Visibility = System.Windows.Visibility.Visible;
            lblFreeSpace.Visibility = System.Windows.Visibility.Visible;

            // free space
            long free = di.AvailableFreeSpace;
            long total = di.TotalSize;

            pbFree.Maximum = total;
            pbFree.Value = total - free;

            rFree.Text = GetGB(free).ToString();
            rTotal.Text = GetGB(total).ToString();
            return;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(RefreshDriveInfo));
        }
    }
}
