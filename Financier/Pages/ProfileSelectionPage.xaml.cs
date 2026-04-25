using Financier.Core;

using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Navigation;

using System.Collections.Generic;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProfileSelectionPage : Page
{
    /// <summary>
    /// Creates a new <see cref="ProfileSelectionPage"/> instance.
    /// </summary>
    public ProfileSelectionPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Custom overriden method.
    /// </summary>
    /// <param name="e">Navigation arguments.</param>
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is HashSet<UserProfile> profiles)
        {
            this._profiles = profiles;
        }

        await ReloadUI();
    }

    private HashSet<UserProfile> _profiles = [];
    private async Task ReloadUI()
    {
        this.lvProfiles.Items.Clear();
        this._profiles = UserProfile.GetAllProfiles();

        if (this._profiles.Count == 0)
        {
            // no profiles
            this.lvProfiles.Items.Add(new ListViewItem
            {
                Content = "No profiles found.",
                Padding = new Thickness(8),
                IsEnabled = false
            });

            return;
        }

        foreach (UserProfile profile in this._profiles)
        {
            ListViewItem lvi = new ListViewItem
            {
                Tag = profile,
                Content = new TextBlock
                {
                    Inlines =
                    {
                        new Run { Text = profile.Name, FontSize = 18, FontWeight = FontWeights.SemiBold },
                        new LineBreak(),
                        new Run { Text = $"Transactions: {profile.Transactions.Count.ToString()}", FontSize = 14 }
                    },

                    Margin = new Thickness(8)
                }
            };

            this.lvProfiles.Items.Add(lvi);
        }

        // unselect any profiles
        this.lvProfiles.SelectedIndex = -1;
    }

    private async void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
        await ReloadUI();
    }

    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
        if (App.MainWindow is null)
        {
            // critical error
            this.ib.Severity = InfoBarSeverity.Error;
            this.ib.Title = "Error";
            this.ib.Message = "Window not found.";
            this.ib.IsOpen = true;
            return;
        }

        // load the selected profile
        if (this.lvProfiles.SelectedItem is ListViewItem item)
        {
            if (item.Tag is UserProfile profile)
            {
                _ = UserProfile.SetCurrent(profile);
                App.MainWindow.nviHome.IsSelected = true;
                return;
            }
        }
    }

    private void lvProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // dynamically enable/disable the accept button
        this.btnAccept.IsEnabled = this.lvProfiles.SelectedIndex >= 0;
    }

    private void btnNewProfile_Click(object sender, RoutedEventArgs e)
    {
        if (App.MainWindow is null) return;
        UserProfile.SetCurrent(new UserProfile());
        _ = App.MainWindow.WindowFrame.Navigate(typeof(NewProfilePage));
    }
}
