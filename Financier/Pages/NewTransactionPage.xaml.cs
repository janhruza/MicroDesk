using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Financier.Core;
using Microsoft.UI.System;
using Windows.UI.Popups;
using System.Threading.Tasks;

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
        _type = default;
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
            _type = type;
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
        cbxCategory.Items.Clear();
        switch (_type)
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
        txtValue.Text = "0";
        txtNote.Text = string.Empty;
    }

    private void LoadExpenses()
    {
        foreach (var kvp in AppData.ExpenseNames)
        {
            cbxCategory.Items.Add(new ComboBoxItem
            {
                Tag = (byte)kvp.Key,
                Content = kvp.Value
            });
        }

        cbxCategory.SelectedIndex = 0;
        return;
    }

    private void LoadIncomes()
    {
        foreach (var kvp in AppData.IncomeNames)
        {
            cbxCategory.Items.Add(new ComboBoxItem
            {
                Tag = (byte)kvp.Key,
                Content = kvp.Value
            });
        }

        cbxCategory.SelectedIndex = 0;
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
            await App.ShowDialog(this.XamlRoot, "Unable to create a new transaction. No user profile is loaded.", "Error");
            return;
        }

        UserProfile profile = UserProfile.GetCurrent();

        // check if the transaction is valid
        if (decimal.TryParse(txtValue.Text, out decimal dValue) == false)
        {
            await App.ShowDialog(this.XamlRoot, "Unable to create a new transaction. Please make sure the transaction value is in the correct numeric format.", "Invalid value");
            return;
        }

        // get the absolute value of the transaction
        dValue = decimal.Abs(dValue);

        // get the category
        byte categoryCode = 0xFF;

        if (cbxCategory.SelectedItem is ComboBoxItem cbi)
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
            switch (_type)
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

        // create the transaction
        TransactionInfo tr = new TransactionInfo
        {
            Timestamp = DateTime.Now,
            Type = _type,
            Category = categoryCode,
            Value = dValue,
            Note = txtNote.Text
        };

        // assign the transaction to the active profile,
        // save it and update it to sync changes
        profile.Transactions.Add(tr);
        UserProfile.Save(profile);
        UserProfile.SetCurrent(profile);

        // display a confirmation message
        await App.ShowDialog(this.XamlRoot, "Transaction added successfully.", "Transaction added");

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
