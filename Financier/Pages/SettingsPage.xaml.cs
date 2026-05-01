using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page
{
    /// <summary>
    /// Creates a new <see cref="SettingsPage"/> instance.
    /// </summary>
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void cbxThemeMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.cbxThemeMode.SelectedItem is ComboBoxItem cbi)
        {
            if (cbi.Tag is string sThemeMode)
            {
                switch (sThemeMode)
                {
                    case "themeLight":
                        App.SetAppThemeMode(Microsoft.UI.Xaml.ElementTheme.Light);
                        break;

                    case "themeDark":
                        App.SetAppThemeMode(Microsoft.UI.Xaml.ElementTheme.Dark);
                        break;

                    case "themeSystem":
                        App.SetAppThemeMode(Microsoft.UI.Xaml.ElementTheme.Default);
                        break;
                }
            }
        }
    }

    private void cbxBackdrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.cbxBackdrop.SelectedItem is ComboBoxItem item)
        {
            if (item.Tag is string sBackdrop)
            {
                switch (sBackdrop)
                {
                    default:
                    case "backdropMica":
                        App.SetAppBackdropMode(new MicaBackdrop());
                        break;

                    case "backdropMicaAlt":
                        App.SetAppBackdropMode(new MicaBackdrop() { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt});
                        break;

                    case "backdropAcrylic":
                        App.SetAppBackdropMode(new DesktopAcrylicBackdrop());
                        break;

                    case "backdropNone":
                        App.SetAppBackdropMode(null);
                        break;
                }
            }
        }
    }
}
