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

namespace Update;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
    }

    /// <summary>
    /// Navigates to the specified page type, optionally passing a parameter to the target page.
    /// </summary>
    /// <param name="pageType">The type of the page to navigate to. Must not be null.</param>
    /// <param name="parameter">An optional parameter to pass to the target page. Can be null if no parameter is required.</param>
    /// <returns>true if navigation to the specified page was successful; otherwise, false.</returns>
    public bool NavigateTo(Type pageType, object? parameter = null)
    {
        return frm.Navigate(pageType, parameter);
    }

    /// <summary>
    /// Displays a message in the InfoBar with the specified severity and message content.
    /// </summary>
    /// <param name="severity">Message severity.</param>
    /// <param name="message">Message content.</param>
    public void DisplayMessage(InfoBarSeverity severity, string message)
    {
        infoBar.Severity = severity;
        infoBar.Title = severity switch
        {
            InfoBarSeverity.Error => "Error",
            InfoBarSeverity.Warning => "Warning",
            InfoBarSeverity.Success => "Success",
            _ => "Info"
        };
        infoBar.Message = message;
        infoBar.IsOpen = true;
        return;
    }

    private void titleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        // unused
        nvMenu.IsPaneOpen = !nvMenu.IsPaneOpen;
        return;
    }

    private void titleBar_BackRequested(TitleBar sender, object args)
    {
        // unused
        return;
    }
}
