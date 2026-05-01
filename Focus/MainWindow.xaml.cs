using Focus.Pages;

using MDCore;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Focus;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;

        // Navigate to the initial page (PgNewSession) when the main window is loaded.
        frm.Navigate(typeof(PgNewSession));

        this.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(400, 400));

        this.Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args)
    {
        Log.Info(Log.AppStopped);
    }

    private void frm_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Content is Page page)
        {
            // Set the title of the window to the title of the page.
            string title = (string)page.Tag;
            titleBar.Subtitle = title;
        }
    }
}
