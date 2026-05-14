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

namespace Update.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="SettingsPage"/> class.
    /// </summary>
    public SettingsPage()
    {
        InitializeComponent();

        cbxStyle.SelectedIndex = 1;
        _isInitialized = true;
    }

    private bool _isInitialized = false;
    private void cbxStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isInitialized == false) return;

        switch (cbxStyle.SelectedIndex)
        {
            default: break;

            case 0:
                // Accent
                App.Current.Resources.Clear();
                break;

            case 1:
                // Custom
                break;
        }
    }
}
