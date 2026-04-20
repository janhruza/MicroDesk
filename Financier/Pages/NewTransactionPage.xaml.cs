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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Financier.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public partial class NewTransactionPage : Page
{
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
                case TransactionType.Expanse:
                    Tag = "New Expanse";
                    break;

                case TransactionType.Income:
                    Tag = "New Income";
                    break;
            }
        }
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
