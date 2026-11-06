using Financier.Core;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Linq;
using System.Threading.Tasks;

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
        this.stpPreview.Visibility = Visibility.Collapsed;
        this.stpEmptyPreview.Visibility = Visibility.Visible;

        this.lvHistory.ItemsSource = profile.Transactions.OrderByDescending(t => t.Timestamp).ToList();

        if (this.lvHistory.Items.Count > 0)
        {
            this.stpEmptyHistory.Visibility = Visibility.Collapsed;
            this.lvHistory.SelectedIndex = 0;
        }
        else
        {
            this.stpEmptyHistory.Visibility = Visibility.Visible;
            this.stpPreview.Visibility = Visibility.Collapsed;
        }
    }

    private async void btnEditTransaction_Click(object sender, RoutedEventArgs e)
    {
        if (this.lvHistory.SelectedItem is TransactionInfo transaction)
        {
            await EditTransactionAsync(transaction);
        }
    }

    private async void transactionEditMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem menuItem && menuItem.Tag is TransactionInfo transaction)
        {
            this.lvHistory.SelectedItem = transaction;
            await EditTransactionAsync(transaction);
        }
    }

    private async Task EditTransactionAsync(TransactionInfo transaction)
    {
        if (this.XamlRoot is null)
        {
            return;
        }

        CalendarDatePicker datePicker = new CalendarDatePicker
        {
            Header = "Date",
            Date = new DateTimeOffset(transaction.Timestamp),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        ComboBox categoryPicker = new ComboBox
        {
            Header = "Category",
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        if (transaction.Type == TransactionType.Income)
        {
            foreach (var categoryEntry in AppData.IncomeNames)
            {
                categoryPicker.Items.Add(new ComboBoxItem
                {
                    Content = categoryEntry.Value,
                    Tag = (byte)categoryEntry.Key
                });
            }
        }
        else
        {
            foreach (var categoryEntry in AppData.ExpenseNames)
            {
                categoryPicker.Items.Add(new ComboBoxItem
                {
                    Content = categoryEntry.Value,
                    Tag = (byte)categoryEntry.Key
                });
            }
        }

        for (int index = 0; index < categoryPicker.Items.Count; index++)
        {
            if (categoryPicker.Items[index] is ComboBoxItem item && item.Tag is byte categoryCode && categoryCode == transaction.Category)
            {
                categoryPicker.SelectedIndex = index;
                break;
            }
        }

        NumberBox amountBox = new NumberBox
        {
            Header = "Amount",
            Value = (double)transaction.Value,
            SmallChange = 1,
            SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        TextBox noteBox = new TextBox
        {
            Header = "Note (Optional)",
            Text = transaction.Note ?? string.Empty,
            AcceptsReturn = true,
            MinHeight = 120,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        StackPanel form = new StackPanel
        {
            Spacing = 16,
            MinWidth = 360
        };
        form.Children.Add(datePicker);
        form.Children.Add(categoryPicker);
        form.Children.Add(amountBox);
        form.Children.Add(noteBox);

        ContentDialog dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = transaction.Type == TransactionType.Income ? "Edit income" : "Edit expense",
            Content = form,
            PrimaryButtonText = "Save changes",
            SecondaryButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary
        };

        ContentDialogResult result = await dialog.ShowAsync();
        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        if (!datePicker.Date.HasValue || double.IsNaN(amountBox.Value) || double.IsInfinity(amountBox.Value) || amountBox.Value < 0 || categoryPicker.SelectedItem is not ComboBoxItem selectedCategory || selectedCategory.Tag is not byte category)
        {
            App.MainWindow?.DisplayMessage(InfoBarSeverity.Error, "Invalid transaction", "Enter a valid date, amount, and category.");
            return;
        }

        transaction.Timestamp = datePicker.Date.Value.DateTime;
        transaction.Value = (decimal)Math.Abs(amountBox.Value);
        transaction.Category = category;
        transaction.Note = noteBox.Text;

        UserProfile profile = UserProfile.GetCurrent();
        _ = UserProfile.Save(profile);
        _ = UserProfile.SetCurrent(profile);
        LoadUI();
        App.MainWindow?.DisplayMessage(InfoBarSeverity.Success, "Transaction updated", "The transaction was updated successfully.");
    }

    private async void btnDeleteTransaction_Click(object sender, RoutedEventArgs e)
    {
        if (this.lvHistory.SelectedItem is TransactionInfo transaction)
        {
            await DeleteTransactionAsync(transaction);
        }
    }

    private async void transactionDeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem menuItem && menuItem.Tag is TransactionInfo transaction)
        {
            this.lvHistory.SelectedItem = transaction;
            await DeleteTransactionAsync(transaction);
        }
    }

    private async Task DeleteTransactionAsync(TransactionInfo transaction)
    {
        if (this.XamlRoot is null)
        {
            return;
        }

        ContentDialog dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = "Delete transaction?",
            Content = "This transaction will be permanently removed from the current profile.",
            PrimaryButtonText = "Delete",
            SecondaryButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Secondary
        };

        ContentDialogResult result = await dialog.ShowAsync();
        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        UserProfile profile = UserProfile.GetCurrent();
        if (!profile.Transactions.Remove(transaction))
        {
            return;
        }

        _ = UserProfile.Save(profile);
        _ = UserProfile.SetCurrent(profile);
        LoadUI();
        App.MainWindow?.DisplayMessage(InfoBarSeverity.Success, "Transaction deleted", "The transaction was removed successfully.");
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
            this.stpEmptyPreview.Visibility = Visibility.Collapsed;
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
            this.stpEmptyPreview.Visibility = Visibility.Visible;
        }
    }
}
