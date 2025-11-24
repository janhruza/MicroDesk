using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace LibMicroDesk.Windows;

/// <summary>
/// Representing a dialog window with a specific style for MicroDesk applications.
/// The iconless behavior is achieved by removing the default dialog frame.
/// </summary>
public class IconlessWindow : Window
{
    const int GWL_EXSTYLE = -20;
    const int WS_EX_DLGMODALFRAME = 0x0001;
    const int SWP_NOSIZE = 0x0001;
    const int SWP_NOMOVE = 0x0002;
    const int SWP_NOZORDER = 0x0004;
    const int SWP_FRAMECHANGED = 0x0020;

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
        int x, int y, int cx, int cy, uint flags);

    /// <summary>
    /// Raises the <see cref="Window.SourceInitialized"/> event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        RemoveDialogFrame();
    }

    private void RemoveDialogFrame()
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        this.Handle = hwnd;
        int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

        // Force non-client area to update
        SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0,
            SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IconlessWindow"/> class.
    /// </summary>
    public IconlessWindow()
    {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.SnapsToDevicePixels = true;
        this.UseLayoutRounding = true;

        // custom behavior
        this.KeyDown += this.IconlessWindow_KeyDown;
        this.Activated += (s, e) =>
        {
            MDCore.activeWindow = this.Handle;
        };
    }

    /// <summary>
    /// Representing the window handle.
    /// </summary>
    public nint Handle { get; private set; }

    private void IconlessWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.F1)
        {
            MDCore.AboutBox();
        }

        else if (e.Key == System.Windows.Input.Key.F2)
        {
            _ = new WndGeneralSettings().ShowDialog();
        }
    }

    /// <summary>
    /// Overrides the default <see cref="ShowDialog"/> method.
    /// </summary>
    /// <returns>Dialog result as <see cref="bool"/>.</returns>
    /// <remarks>
    /// This method also adjusts window's position based on whether it has a n owner or not.
    /// If window has a parent, its startup position is set to <see cref="WindowStartupLocation.CenterOwner"/>,
    /// otherwise <see cref="WindowStartupLocation.CenterScreen"/>.
    /// </remarks>
    public new bool? ShowDialog()
    {
        this.WindowStartupLocation = (Owner != null) ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
        return base.ShowDialog();
    }
}
