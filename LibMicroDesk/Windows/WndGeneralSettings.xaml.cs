using System.Collections.Generic;
using System.Windows.Controls;

namespace LibMicroDesk.Windows;

/// <summary>
/// Represents a window for general settings in the application.
/// </summary>
public partial class WndGeneralSettings : IconlessWindow
{
    /// <summary>
    /// Constructs a new instance of the <see cref="WndGeneralSettings"/> class.
    /// </summary>
    public WndGeneralSettings()
    {
        InitializeComponent();
        Loaded += (s, e) => ReloadUI();
    }

    private Dictionary<SystemTheme, string> _themeNames => new Dictionary<SystemTheme, string>
    {
        { SystemTheme.Light, "Light" },
        { SystemTheme.Dark, "Dark" },
        { SystemTheme.Auto, "System" },
        { SystemTheme.None, "Default" }
    };

    private void ReloadUI()
    {
        if (IsLoaded == false)
        {
            return;
        }

        MDCore.EnsureSettings();

        // theme options
        {
            // Reload UI elements based on current settings
            cbxTheme.Items.Clear();

            for (int i = 0; i < 4; i++)
            {
                ComboBoxItem cbi = new ComboBoxItem
                {
                    Tag = (SystemTheme)i,
                    Content = _themeNames[(SystemTheme)i]
                };

                cbxTheme.Items.Add(cbi);

                if ((SystemTheme)i == MDCore.Settings.Theme)
                {
                    cbxTheme.SelectedItem = cbi; // Select the current theme
                }
            }
        }

        // culture options
        {
            cbxCulture.Items.Clear();
            foreach (var culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures))
            {
                ComboBoxItem cbi = new ComboBoxItem
                {
                    Tag = culture.Name,
                    Content = culture.DisplayName
                };

                cbxCulture.Items.Add(cbi);

                if (culture.Name == MDCore.Settings.Culture)
                {
                    cbxCulture.SelectedItem = cbi; // Select the current culture
                }
            }
        }
    }

    private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Close();
    }

    private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        // Save the settings
        if (cbxTheme.SelectedItem is ComboBoxItem selectedTheme)
        {
            SystemTheme theme = (SystemTheme)selectedTheme.Tag;
            MDCore.Settings.Theme = theme;
            MDCore.ApplyTheme(theme);
        }

        if (cbxCulture.SelectedItem is ComboBoxItem selectedCulture)
        {
            string culture = selectedCulture.Tag.ToString() ?? "en-US"; // Default to en-US if null
            MDCore.Settings.Culture = culture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
        }

        // Save the settings to file
        if (AppSettings.Save(MDCore.Settings))
        {
            Close();
        }

        else
        {
            // Show an error message if saving fails
            System.Windows.MessageBox.Show("Failed to save settings.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }

    private void btnApply_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        // same as ok but do not close the window
        // Save the settings
        if (cbxTheme.SelectedItem is ComboBoxItem selectedTheme)
        {
            SystemTheme theme = (SystemTheme)selectedTheme.Tag;
            MDCore.Settings.Theme = theme;
            MDCore.ApplyTheme(theme);
        }

        if (cbxCulture.SelectedItem is ComboBoxItem selectedCulture)
        {
            string culture = selectedCulture.Tag.ToString() ?? "en-US"; // Default to en-US if null
            MDCore.Settings.Culture = culture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
        }

        // Save the settings to file
        if (AppSettings.Save(MDCore.Settings) != true)
        {
            // Show an error message if saving fails
            System.Windows.MessageBox.Show("Failed to save settings.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
