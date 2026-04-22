using Financier.Core;

using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Navigation;

using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomePage : Page
{
    /// <summary>
    /// Creates a new home page instance.
    /// </summary>
    public HomePage()
    {
        InitializeComponent();
    }

    private void DisplayName(ref UserProfile profile)
    {
        this.rUsername.Text = profile.Name;
    }

    private void GetBalance(ref UserProfile profile)
    {
        decimal sum = 0;
        for (int i = 0; i < profile.Transactions.Count; i++)
        {
            TransactionInfo tr = profile.Transactions[i];
            if (tr.Type == TransactionType.Income)
            {
                sum += tr.Value;
            }

            else /* Expense */
            {
                sum -= tr.Value;
            }
        }

        // display in the currency format - default system currency
        this.tbBalance.Text = sum.ToString("C");
    }

    private void DisplayHistory(ref UserProfile profile)
    {
        this.lvHistory.Items.Clear();
        TransactionInfo[] history = [.. profile.Transactions.OrderByDescending(x => x.Timestamp)];
        if (history.Length == 0)
        {
            ListViewItem lvi = new ListViewItem
            {
                Content = new TextBlock
                {
                    Text = "No transaction history.",
                    Margin = new Microsoft.UI.Xaml.Thickness(8)
                },
            };
            this.lvHistory.Items.Add(lvi);
            return;
        }

        foreach (TransactionInfo tr in history)
        {
            string category = string.Empty;
            if (tr.Type == TransactionType.Expense)
            {
                category = AppData.ExpenseNames[(ExpenseCategories)tr.Category];
            }

            else
            {
                category = AppData.IncomeNames[(IncomeCategories)tr.Category];
            }

            ListViewItem lvi = new ListViewItem
            {
                Content = new TextBlock
                {
                    Inlines =
                    {
                        new Run
                        {
                            Text = tr.Value.ToString("C"),
                            FontWeight = FontWeights.SemiBold,
                            FontSize = 18
                        },

                        new LineBreak(),

                        new Run
                        {
                            Text = category,
                            FontSize = 14
                        }
                    },
                    Margin = new Microsoft.UI.Xaml.Thickness(8)
                },

                Tag = tr
            };

            this.lvHistory.Items.Add(lvi);
        }

        // select the first item
        this.lvHistory.SelectedIndex = 0;
    }

    private void LoadUI()
    {
        if (UserProfile.IsLoaded() == false)
        {
            // unable to load ui - no profile is loaded
            return;
        }

        // get profile data
        UserProfile profile = UserProfile.GetCurrent();
        DisplayName(ref profile);

        // get user's balance - sum of all transactions
        GetBalance(ref profile);

        // get and display the transaction history
        DisplayHistory(ref profile);
    }

    /// <summary>
    /// Custom method that gets called on navigation to this page.
    /// </summary>
    /// <param name="e">Arguments.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        LoadUI();
    }

    private async void lvHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.lvHistory.SelectedIndex == -1)
        {
            // no items selected
            this.stpPreview.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            return;
        }

        if (this.lvHistory.SelectedItem is ListViewItem lvi)
        {
            if (lvi.Tag is TransactionInfo tr)
            {
                // display the selected item in the preview panel
                this.dpDate.SelectedDate = tr.Timestamp;
                this.txtValue.Text = tr.Value.ToString("C");
                this.txtType.Text = tr.Type == TransactionType.Income ? "Income" : "Expense";
                this.txtCategory.Text = tr.Type == TransactionType.Income ? AppData.IncomeNames[(IncomeCategories)tr.Category] : AppData.ExpenseNames[(ExpenseCategories)tr.Category];
                this.txtDesc.Text = string.IsNullOrWhiteSpace(tr.Note) ? string.Empty : tr.Note.Trim();
                this.stpPreview.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                return;
            }
        }

        // unable to show a preview, hide the panel completely
        this.stpPreview.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        return;
    }
}
