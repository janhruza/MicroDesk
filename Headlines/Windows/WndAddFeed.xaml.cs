using System.Windows;
using System.Windows.Controls;
using LibMicroDesk;
using LibMicroDesk.Windows;

namespace Headlines.Windows;

/// <summary>
/// Representing the Add new RSS feed window.
/// </summary>
public partial class WndAddFeed : IconlessWindow
{
    /// <summary>
    /// Creates a new instance of the <see cref="WndAddFeed"/> class.
    /// </summary>
    public WndAddFeed()
    {
        InitializeComponent();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(txtUrl.Text))
        {
            MessageBox.Show(this, "Please enter a valid URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        App.RssFeedSources.Add(txtUrl.Text.Trim());
        this.Close();
        return;
    }

    private void txtUrl_TextChanged(object sender, TextChangedEventArgs e)
    {
        btnAdd.IsEnabled = !string.IsNullOrWhiteSpace(txtUrl.Text);
    }
}
