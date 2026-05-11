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
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
    }

    public bool NavigateTo(Type pageType, object? parameter = null)
    {
        return frm.Navigate(pageType, parameter);
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
