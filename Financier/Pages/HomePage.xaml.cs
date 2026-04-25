using Financier.Core;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System.Linq;

namespace Financier.Pages;

public sealed partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
        this.lvHistory.ContainerContentChanging += OnContainerContentChanging;
    }

    private void OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        if (args.InRecycleQueue) return;

        var transaction = args.Item as TransactionInfo;
        if (transaction == null) return;

        var container = args.ItemContainer;
        if (container == null) return;

        var iconBorder = FindChildByName<Border>(container, "IconBorder");
        var typeIcon = FindChildByName<FontIcon>(container, "TypeIcon");
        var categoryText = FindChildByName<TextBlock>(container, "CategoryText");
        var valueText = FindChildByName<TextBlock>(container, "ValueText");

        // Use Theme Resources for a professional look
        var accentBrush = (Brush)Application.Current.Resources["AccentTextFillColorPrimaryBrush"];
        var subtleBackground = (Brush)Application.Current.Resources["SystemControlBackgroundListLowBrush"];
        var defaultForeground = (Brush)Application.Current.Resources["TextFillColorPrimaryBrush"];

        if (transaction.Type == TransactionType.Income)
        {
            if (iconBorder != null) iconBorder.Background = subtleBackground;
            if (typeIcon != null)
            {
                typeIcon.Glyph = "\uE74A"; // Chevron Up
                typeIcon.Foreground = accentBrush;
            }
            if (valueText != null)
            {
                valueText.Text = $"+{transaction.Value:C}";
                valueText.Foreground = accentBrush;
            }
            if (categoryText != null) categoryText.Text = AppData.IncomeNames[(IncomeCategories)transaction.Category];
        }
        else
        {
            if (iconBorder != null) iconBorder.Background = subtleBackground;
            if (typeIcon != null)
            {
                typeIcon.Glyph = "\uE74B"; // Chevron Down
                typeIcon.Foreground = defaultForeground;
            }
            if (valueText != null)
            {
                valueText.Text = $"-{transaction.Value:C}";
                valueText.Foreground = defaultForeground;
            }
            if (categoryText != null) categoryText.Text = AppData.ExpenseNames[(ExpenseCategories)transaction.Category];
        }
    }

    private T FindChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
    {
        int count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T t && (child as FrameworkElement)?.Name == name)
            {
                return t;
            }
            var result = FindChildByName<T>(child, name);
            if (result != null) return result;
        }
        return null;
    }

    private void LoadUI()
    {
        if (!UserProfile.IsLoaded()) return;

        var profile = UserProfile.GetCurrent();
        this.rUsername.Text = profile.Name;

        decimal balance = profile.Transactions.Sum(t => t.Type == TransactionType.Income ? t.Value : -t.Value);
        this.tbBalance.Text = balance.ToString("C");
        this.tbBalance.Foreground = (Brush)Application.Current.Resources["AccentTextFillColorPrimaryBrush"];

        this.lvHistory.ItemsSource = profile.Transactions.OrderByDescending(t => t.Timestamp).ToList();

        if (this.lvHistory.Items.Count > 0)
        {
            this.lvHistory.SelectedIndex = 0;
        }
        else
        {
            this.stpPreview.Visibility = Visibility.Collapsed;
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        LoadUI();
    }

    private void lvHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.lvHistory.SelectedItem is TransactionInfo tr)
        {
            this.stpPreview.Visibility = Visibility.Visible;
            this.txtDateDisplay.Text = tr.Timestamp.ToString("D");
            this.txtValueDisplay.Text = tr.Value.ToString("C");

            var accentBrush = (Brush)Application.Current.Resources["AccentTextFillColorPrimaryBrush"];
            var defaultForeground = (Brush)Application.Current.Resources["TextFillColorPrimaryBrush"];

            this.txtValueDisplay.Foreground = tr.Type == TransactionType.Income ? accentBrush : defaultForeground;
            this.txtCategoryDisplay.Text = tr.Type == TransactionType.Income ? AppData.IncomeNames[(IncomeCategories)tr.Category] : AppData.ExpenseNames[(ExpenseCategories)tr.Category];

            if (string.IsNullOrWhiteSpace(tr.Note))
            {
                this.spNotes.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.spNotes.Visibility = Visibility.Visible;
                this.txtNotesDisplay.Text = tr.Note;
            }
        }
        else
        {
            this.stpPreview.Visibility = Visibility.Collapsed;
        }
    }
}
