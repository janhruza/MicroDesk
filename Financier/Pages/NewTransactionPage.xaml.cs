using Financier.Core;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public partial class NewTransactionPage : Page
{
    internal const string TITLE_EXPANSES = "New Expense";
    internal const string TITLE_INCOMES = "New Income";

    private TransactionType _type;

    /// <summary>
    /// Initializes a new instance of the NewTransactionPage class.
    /// </summary>
    /// <remarks>This constructor sets up the page and initializes its components. Use this constructor when
    /// creating a new transaction page in the application.</remarks>
    public NewTransactionPage()
    {
        InitializeComponent();
        this._type = default;
    }

    /// <summary>
    /// Handles navigation to the page and initializes the transaction type based on the navigation parameter.
    /// </summary>
    /// <remarks>If the navigation parameter is a valid TransactionType, the page is configured accordingly.
    /// Otherwise, the method returns without making changes.</remarks>
    /// <param name="e">The event data containing navigation parameters and state information.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is TransactionType type)
        {
            this._type = type;
            switch (type)
            {
                case TransactionType.Expense:
                    Tag = TITLE_EXPANSES;
                    break;

                case TransactionType.Income:
                    Tag = TITLE_INCOMES;
                    break;
            }

            LoadUI();
        }
    }

    private void LoadUI()
    {
        this.cbxCategory.Items.Clear();
        switch (this._type)
        {
            case TransactionType.Expense:
                LoadExpenses();
                break;

            case TransactionType.Income:
                LoadIncomes();
                break;

            default:
                break;
        }

        // clear the other fields to defaults
        this.txtValue.Text = "0";
        this.txtNote.Text = string.Empty;
        this.dpDate.Date = DateTime.Today;
    }

    private void LoadExpenses()
    {
        foreach (var kvp in AppData.ExpenseNames)
        {
            this.cbxCategory.Items.Add(new ComboBoxItem
            {
                Tag = (byte)kvp.Key,
                Content = kvp.Value
            });
        }

        this.cbxCategory.SelectedIndex = 0;
        return;
    }

    private void LoadIncomes()
    {
        foreach (var kvp in AppData.IncomeNames)
        {
            this.cbxCategory.Items.Add(new ComboBoxItem
            {
                Tag = (byte)kvp.Key,
                Content = kvp.Value
            });
        }

        this.cbxCategory.SelectedIndex = 0;
        return;
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        LoadUI();
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        if (UserProfile.IsLoaded() == false)
        {
            // no profile loaded
            await App.ShowDialog(XamlRoot, "Unable to create a new transaction. No user profile is loaded.", "Error");
            return;
        }

        UserProfile profile = UserProfile.GetCurrent();

        // check if the transaction is valid
        if (decimal.TryParse(this.txtValue.Text, out decimal dValue) == false)
        {
            await App.ShowDialog(XamlRoot, "Unable to create a new transaction. Please make sure the transaction value is in the correct numeric format.", "Invalid value");
            return;
        }

        // get the absolute value of the transaction
        dValue = decimal.Abs(dValue);

        // get the category
        byte categoryCode = 0xFF;

        if (this.cbxCategory.SelectedItem is ComboBoxItem cbi)
        {
            if (cbi.Tag is byte bVal)
            {
                categoryCode = bVal;
            }
        }

        if (categoryCode == 0xFF)
        {
            // invalid category
            // default to 0
            switch (this._type)
            {
                case TransactionType.Income:
                    categoryCode = (byte)default(IncomeCategories);
                    break;

                case TransactionType.Expense:
                    categoryCode = (byte)default(ExpenseCategories);
                    break;

                default:
                    categoryCode = 0;
                    break;
            }
        }

        // get current date from dpDate
        DateTime dt = DateTime.Now;
        if (dpDate.Date.HasValue)
        {
            dt = dpDate.Date.Value.DateTime;
        }

        // create the transaction
        TransactionInfo tr = new TransactionInfo
        {
            Timestamp = dt,
            Type = this._type,
            Category = categoryCode,
            Value = dValue,
            Note = this.txtNote.Text
        };

        // assign the transaction to the active profile,
        // save it and update it to sync changes
        profile.Transactions.Add(tr);
        _ = UserProfile.Save(profile);
        _ = UserProfile.SetCurrent(profile);

        // display a confirmation message
        await App.ShowDialog(XamlRoot, "Transaction added successfully.", "Transaction added");

        // clear the UI
        LoadUI();
        return;
    }

    /// <summary>
    /// Initializes a new instance of the NewTransactionPage class for the specified transaction type.
    /// </summary>
    /// <param name="eType">The type of transaction to be created. Determines the behavior and layout of the page.</param>
    //public BaseTransactionPage(TransactionType eType)
    //{
    //    InitializeComponent();
    //    _type = eType;
    //}
}
