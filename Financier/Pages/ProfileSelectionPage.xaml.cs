using Financier.Core;

using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
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
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is HashSet<UserProfile> profiles)
        {
            this._profiles = profiles;
        }

        ReloadUI();
    }

    private HashSet<UserProfile> _profiles = [];
    private void ReloadUI()
    {
        lvProfiles.Items.Clear();
        if (_profiles.Count == 0)
        {
            // no profiles
            lvProfiles.Items.Add(new ListViewItem
            {
                Content = "No profiles found.",
                Padding = new Thickness(8),
                IsEnabled = false
            });

            return;
        }

        foreach (UserProfile profile in _profiles)
        {
            ListViewItem lvi = new ListViewItem
            {
                Tag = profile,
                Padding = new Thickness(8),
                Content = new TextBlock
                {
                    Inlines =
                    {
                        new Run { Text = profile.Name, FontSize = 18, FontWeight = FontWeights.SemiBold },
                        new LineBreak(),
                        new Run { Text = $"Transactions: {profile.Transactions.Count.ToString()}", FontSize = 14 }
                    }
                }
            };

            lvProfiles.Items.Add(lvi);
        }

        // unselect any profiles
        lvProfiles.SelectedIndex = -1;
    }

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
        ReloadUI();
    }

    private void btnAccept_Click(object sender, RoutedEventArgs e)
    {
        if (App.MainWindow is null)
        {
            // critical error
            ib.Severity = InfoBarSeverity.Error;
            ib.Title = "Error";
            ib.Message = "Window not found.";
            ib.IsOpen = true;
            return;
        }

        // load the selected profile
        if (lvProfiles.SelectedItem is ListViewItem item)
        {
            if (item.Tag is UserProfile profile)
            {
                UserProfile.SetCurrent(profile);
                App.MainWindow.nviHome.IsSelected = true;
                return;
            }
        }
    }

    private void lvProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // dynamically enable/disable the accept button
        btnAccept.IsEnabled = lvProfiles.SelectedIndex >= 0;
    }
}
