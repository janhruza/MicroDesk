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

namespace Journal.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PgSettings : Page
{
    public PgSettings()
    {
        InitializeComponent();
    }

    private void cbxTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cbxTheme.SelectedItem is ComboBoxItem cbi)
        {
            if (cbi.Tag is string sType)
            {
                switch (sType)
                {
                    default:
                        break;

                    case "themeLight":
                        App.SetThemeMode(ElementTheme.Light);
                        break;

                    case "themeDark":
                        App.SetThemeMode(ElementTheme.Dark);
                        break;

                    case "themeSystem":
                        App.SetThemeMode(ElementTheme.Default);
                        break;
                }
            }
        }
    }

    private void cbxBackdrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cbxBackdrop.SelectedItem is ComboBoxItem cbi)
        {
            if (cbi.Tag is string sBack)
            {
                switch (sBack)
                {
                    default:
                        break;

                    case "bpNone":
                        App.SetAppBackdrop(null);
                        break;

                    case "bpAcrylic":
                        App.SetAppBackdrop(new DesktopAcrylicBackdrop());
                        break;

                    case "bpMica":
                        App.SetAppBackdrop(new MicaBackdrop() { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base} );
                        break;
                }
            }
        }
    }
}
