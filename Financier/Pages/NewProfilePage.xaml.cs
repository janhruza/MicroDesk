using Financier.Core;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

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

    private bool _valid = false;

    /// <summary>
    /// Override the method called when the page is navigated to, to reset the _valid flag to false, indicating that the profile creation process has not been completed yet.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        _valid = false;
    }

    /// <summary>
    /// Override the method called when the page is navigating from, to check if the _valid flag is false. If it is, cancel the navigation and display a warning message to the user, prompting them to complete the profile creation before leaving the page.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (_valid == false)
        {
            e.Cancel = true;
            App.MainWindow?.DisplayMessage(InfoBarSeverity.Warning, "Warning", "Please complete the profile creation before leaving this page.");
        }
    }

    private void LoadCultures()
    {
        this.cbxCultures.Items.Clear();

        int currentIdx = 0;
        foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            this.cbxCultures.Items.Add(new ComboBoxItem
            {
                Content = $"{culture.DisplayName} ({culture.Name})",
                Tag = culture.Name
            });

            if (culture.Name == CultureInfo.CurrentCulture.Name)
            {
                currentIdx = this.cbxCultures.Items.Count - 1;
            }
        }

        this.cbxCultures.SelectedIndex = currentIdx;
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

        _valid = true;
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
        this.ib.Severity = InfoBarSeverity.Informational;
        this.ib.Title = "Info";
        this.ib.Message = "Input fields cleared.";
        this.ib.IsOpen = true;
        return;
    }

    private void btnSwitchProfiles_Click(object sender, RoutedEventArgs e)
    {
        _ = (App.MainWindow?.WindowFrame.Navigate(typeof(ProfileSelectionPage)));
    }
}
