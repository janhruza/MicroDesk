using System;
using System.Windows.Controls;
using FileVault.Controls;
using LibMicroDesk.Windows;

namespace FileVault;

/// <summary>
/// Representing the main window class.
/// </summary>
public partial class MainWindow : IconlessWindow
{
    /// <summary>
    /// Creates a new <see cref="MainWindow"/> instance.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        StackPanel stp = new StackPanel();

        foreach (string drive in Environment.GetLogicalDrives())
        {
            var ctl = new DriveButton(drive);
            Button btn = new Button
            {
                Content = ctl
            };
            stp.Children.Add(btn);
        }

        gMain.Children.Add(stp);
    }
}