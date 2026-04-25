using Financier.Core;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Globalization;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NewProfilePage : Page
{
    /// <summary>
    /// Creates a new <see cref="NewProfilePage"/> instance.
    /// </summary>
    public NewProfilePage()
    {
        InitializeComponent();
        LoadCultures();
    }

    private void LoadCultures()
    {
        this.cbxCultures.Items.Clear();

        foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            this.cbxCultures.Items.Add(new ComboBoxItem
            {
                Content = $"{culture.DisplayName} ({culture.Name})",
                Tag = culture.Name
            });
        }

        this.cbxCultures.SelectedIndex = 0;
    }

    private void btnSave_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        string name = this.txtName.Text.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            // input error
            this.ib.Severity = InfoBarSeverity.Error;
            this.ib.Title = "Error";
            this.ib.Message = "Invalid input.";
            this.ib.IsOpen = true;
            return;
        }

        if (this.cbxCultures.SelectedItem is not ComboBoxItem cbi || cbi.Tag is not string sCulture)
        {
            // input error
            this.ib.Severity = InfoBarSeverity.Error;
            this.ib.Title = "Error";
            this.ib.Message = "Invalid culture selection.";
            this.ib.IsOpen = true;
            return;
        }

        UserProfile profile = new UserProfile
        {
            Name = name,
            Culture = sCulture,
            WinPos = default,
            Transactions = []
        };

        if (UserProfile.Save(profile) == false)
        {
            // saving error
            this.ib.Severity = InfoBarSeverity.Error;
            this.ib.Title = "Error";
            this.ib.Message = "Unable to save the user profile.";
            this.ib.IsOpen = true;
            return;
        }

        _ = UserProfile.SetCurrent(profile);

        // navigate to the home page
        if (App.MainWindow is null)
        {
            // error, impossible if this page is visible to the user
            this.ib.Severity = InfoBarSeverity.Error;
            this.ib.Title = "Error";
            this.ib.Message = "Unable to get the main window.";
            this.ib.IsOpen = true;
            return;
        }

        // set the home page
        App.MainWindow.nviHome.IsSelected = true;
        return;
    }
    private void btnClear_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        this.txtName.Text = string.Empty;
        this.ib.Severity = InfoBarSeverity.Success;
        this.ib.Title = "Success";
        this.ib.Message = "Input fields cleared.";
        this.ib.IsOpen = true;
        return;
    }

    private void btnSwitchProfiles_Click(object sender, RoutedEventArgs e)
    {
        _ = (App.MainWindow?.WindowFrame.Navigate(typeof(ProfileSelectionPage)));
    }
}
