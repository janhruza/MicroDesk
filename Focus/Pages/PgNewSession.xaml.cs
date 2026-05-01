using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Focus.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PgNewSession : Page
{
    /// <summary>
    /// Creates a new <see cref="PgNewSession"/> instance.
    /// </summary>
    public PgNewSession()
    {
        InitializeComponent();
        tp.SelectedTime = TimeSpan.FromMinutes(15);
    }

    private bool IncludeBreaks => cbIncludeBreaks.IsChecked ?? false;

    private int GetBreaks()
    {
        if (tp is null) return 0;
        if (IncludeBreaks == false) return 0;

        // calculate breaks based on the selected time
        // according to pomodoro
        TimeSpan time = tp.SelectedTime ?? TimeSpan.Zero;
        return (int)(time.TotalMinutes / 25);
    }

    private void UpdateText()
    {
        if (tp is null || tbInfo is null) return;
        int breaks = GetBreaks();
        tbInfo.Text = $"You will have {breaks} break";
        if (breaks != 1) tbInfo.Text += "s";
        tbInfo.Text += " during this session.";
    }

    private void tp_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
    {
        UpdateText();
    }

    private void cbIncludeBreaks_Click(object sender, RoutedEventArgs e)
    {
        UpdateText();
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        // reset the default value
        tp.SelectedTime = TimeSpan.FromMinutes(15);
    }
}
