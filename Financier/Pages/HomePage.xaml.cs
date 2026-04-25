using Financier.Core;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Financier.Pages;

public sealed partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
        lvHistory.ContainerContentChanging += OnContainerContentChanging;
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
        rUsername.Text = profile.Name;

        decimal balance = profile.Transactions.Sum(t => t.Type == TransactionType.Income ? t.Value : -t.Value);
        tbBalance.Text = balance.ToString("C");
        tbBalance.Foreground = (Brush)Application.Current.Resources["AccentTextFillColorPrimaryBrush"];

        lvHistory.ItemsSource = profile.Transactions.OrderByDescending(t => t.Timestamp).ToList();
        
        if (lvHistory.Items.Count > 0)
        {
            lvHistory.SelectedIndex = 0;
        }
        else
        {
            stpPreview.Visibility = Visibility.Collapsed;
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        LoadUI();
    }

    private void lvHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lvHistory.SelectedItem is TransactionInfo tr)
        {
            stpPreview.Visibility = Visibility.Visible;
            txtDateDisplay.Text = tr.Timestamp.ToString("D");
            txtValueDisplay.Text = tr.Value.ToString("C");
            
            var accentBrush = (Brush)Application.Current.Resources["AccentTextFillColorPrimaryBrush"];
            var defaultForeground = (Brush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            
            txtValueDisplay.Foreground = tr.Type == TransactionType.Income ? accentBrush : defaultForeground;
            txtCategoryDisplay.Text = tr.Type == TransactionType.Income ? AppData.IncomeNames[(IncomeCategories)tr.Category] : AppData.ExpenseNames[(ExpenseCategories)tr.Category];
            
            if (string.IsNullOrWhiteSpace(tr.Note))
            {
                spNotes.Visibility = Visibility.Collapsed;
            }
            else
            {
                spNotes.Visibility = Visibility.Visible;
                txtNotesDisplay.Text = tr.Note;
            }
        }
        else
        {
            stpPreview.Visibility = Visibility.Collapsed;
        }
    }
}
