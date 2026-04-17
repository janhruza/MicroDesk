using Journal.Pages;

using Microsoft.UI.Windowing;
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

namespace Journal;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // set default params
        ExtendsContentIntoTitleBar = true;
        this.AppWindow.SetPresenter(CreatePresenter());
        this.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(800, 600));
        this.OnInitialized();
    }

    private OverlappedPresenter CreatePresenter()
    {
        OverlappedPresenter presenter = OverlappedPresenter.Create();
        presenter.PreferredMinimumHeight = 600;
        presenter.PreferredMinimumWidth = 600;

        return presenter;
    }

    private void OnInitialized()
    {
        if (frm is null)
        {
            return;
        }

        frm.Navigated += (s, e) =>
        {
            if (e.Content is PgHome)
            {
                titleBar.Subtitle = "Home";
            }

            else if (e.Content is PgNewEntry)
            {
                titleBar.Subtitle = "New Entry";
            }
        };

        // display the startup page
        frm.Navigate(typeof(PgHome), App.HomePage);
    }

    private void titleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        // toogle the navigation bar
        nav.IsPaneOpen = !nav.IsPaneOpen;
    }

    private void nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            frm.Navigate(typeof(PgSettings), App.SettingsPage);
            return;
        }

        if (sender.SelectedItem is NavigationViewItem item)
        {
            if (item.Tag is string tag)
            {
                switch (tag)
                {
                    case "home":
                        frm.Navigate(typeof(PgHome), App.HomePage);
                        break;

                    case "new":
                        frm.Navigate(typeof(PgNewEntry), App.NewEntryPage);
                        break;
                }
            }
        }
    }
}
