using Financier.Core;

using Microsoft.UI.Xaml.Controls;

using Windows.Foundation;

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
    }

    private void btnSave_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        string name = txtName.Text.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            // input error
            ib.Severity = InfoBarSeverity.Error;
            ib.Title = "Error";
            ib.Message = "Invalid input.";
            ib.IsOpen = true;
            return;
        }

        UserProfile profile = new UserProfile
        {
            Name = name,
            WinPos = default(Rect),
            Transactions = []
        };

        if (UserProfile.Save(profile) == false)
        {
            // saving error
            ib.Severity = InfoBarSeverity.Error;
            ib.Title = "Error";
            ib.Message = "Unable to save the user profile.";
            ib.IsOpen = true;
            return;
        }

        UserProfile.SetCurrent(profile);

        // navigate to the home page
        if (App.MainWindow is null)
        {
            // error, impossible if this page is visible to the user
            ib.Severity = InfoBarSeverity.Error;
            ib.Title = "Error";
            ib.Message = "Unable to get the main window.";
            ib.IsOpen = true;
            return;
        }

        // set the home page
        App.MainWindow.nviHome.IsSelected = true;
        return;
    }

    private void btnClear_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        txtName.Text = string.Empty;
        ib.Severity = InfoBarSeverity.Success;
        ib.Title = "Success";
        ib.Message = "Input fields cleared.";
        ib.IsOpen = true;
        return;
    }
}
