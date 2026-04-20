using Financier.Pages;

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

namespace Financier;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    /// <summary>
    /// Creates a new, empty, main window instance.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        this.ExtendsContentIntoTitleBar = true;

        // minimum window size
        OverlappedPresenter presenter = OverlappedPresenter.Create();
        presenter.PreferredMinimumWidth = 640;
        presenter.PreferredMinimumHeight = 480;
        this.AppWindow.SetPresenter(presenter);

        // default window size
        this.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(800, 600));
    }

    private void titleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        nav.IsPaneOpen = !nav.IsPaneOpen;
    }

    private void nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            frm.Navigate(typeof(SettingsPage), App.PgSettings);
            return;
        }

        // change pages
        if (args.SelectedItem is NavigationViewItem item)
        {
            if (item.Tag is string sId)
            {
                switch (sId)
                {
                    case "home":
                        frm.Navigate(typeof(HomePage), App.PgHome);
                        break;

                    case "expanses":
                        frm.Navigate(typeof(NewTransactionPage), App.PgNewExpanse);
                        break;

                    case "incomes":
                        frm.Navigate(typeof(NewTransactionPage), App.PgNewIncome);
                        break;
                }
            }
        }
    }

    private void frm_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Content is Page pg)
        {
            if (pg.Tag is string sValue)
            {
                titleBar.Subtitle = sValue;
            }
        }
    }
}
