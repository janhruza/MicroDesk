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
        this._drive = drive ?? string.Empty;

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
        if (string.IsNullOrEmpty(this._drive) == true)
        {
            // no drive selected
            return;
        }

        // display drive info
        try
        {
            DriveInfo di = new DriveInfo(this._drive);
            if (di.IsReady == false)
            {
                // drive not ready
                this.rLetter.Text = this._drive.Replace("\\", string.Empty);
                this.rLabel.Text = "Drive";
                this.pbFree.Visibility = System.Windows.Visibility.Collapsed;
                this.lblFreeSpace.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            // general
            this.rLetter.Text = this._drive.Replace("\\", string.Empty);
            this.rLabel.Text = GetLabel(di.VolumeLabel);
            this.pbFree.Visibility = System.Windows.Visibility.Visible;
            this.lblFreeSpace.Visibility = System.Windows.Visibility.Visible;

            // free space
            long free = di.AvailableFreeSpace;
            long total = di.TotalSize;

            this.pbFree.Maximum = total;
            this.pbFree.Value = total - free;

            this.rFree.Text = GetGB(free).ToString();
            this.rTotal.Text = GetGB(total).ToString();
            return;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(RefreshDriveInfo));
        }
    }
}
