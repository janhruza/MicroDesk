using System;
using System.Collections.Generic;

namespace Financier.Core;

/// <summary>
/// Representing a class with all in-app data bindings, dictionaries (maps) and other data.
/// </summary>
public static class AppData
{
    /// <summary>
    /// Internal testing profile.
    /// </summary>
    internal static UserProfile TestProfile { get; } = new UserProfile
    {
        Name = "Test Profile",
        Transactions = [
            new TransactionInfo {
                Type = TransactionType.Income,
                Category = (byte)IncomeCategories.Salary,
                Value = 2850,
                Timestamp = DateTime.Now.AddDays(-7),
                Text = "Weekly Pay"
            },

            new TransactionInfo {
                Type = TransactionType.Expense,
                Category = (byte)ExpenseCategories.Shopping,
                Value = 26,
                Timestamp = DateTime.Now.AddDays(-3)
            },

            new TransactionInfo {
                Type = TransactionType.Expense,
                Category = (byte)ExpenseCategories.Housing,
                Value = 120,
                Timestamp = DateTime.Now.AddDays(-6)
            },

            new TransactionInfo {
                Type = TransactionType.Income,
                Category = (byte)IncomeCategories.Gifts,
                Value = 15,
                Timestamp = DateTime.Now.AddDays(-5)
            },

            new TransactionInfo {
                Type = TransactionType.Expense,
                Category = (byte)ExpenseCategories.Entertainment,
                Value = 10,
                Timestamp = DateTime.Now
            }
        ]
    };

    /// <summary>
    /// Representing a map of all expense categories and their string representation for use in the app UI.
    /// </summary>
    public static Dictionary<ExpenseCategories, string> ExpenseNames { get; } = new Dictionary<ExpenseCategories, string>
    {
        { ExpenseCategories.Housing, "Housing" },
        { ExpenseCategories.Groceries, "Groceries" },
        { ExpenseCategories.Travel, "Travel" },
        { ExpenseCategories.Meals, "Meals" },
        { ExpenseCategories.Entertainment, "Entertainment" },
        { ExpenseCategories.Health, "Health" },
        { ExpenseCategories.Shopping, "Shopping" },
        { ExpenseCategories.PersonalGrowth, "Personal Growth" },
        { ExpenseCategories.Finances, "Finances" },
        { ExpenseCategories.Other, "Other" }
    };

    /// <summary>
    /// Representing a map of all income categories and their string representation for use in the app UI.
    /// </summary>
    public static Dictionary<IncomeCategories, string> IncomeNames { get; } = new Dictionary<IncomeCategories, string>
    {
        { IncomeCategories.Salary, "Salary/Wage" },
        { IncomeCategories.Business, "Business" },
        { IncomeCategories.Investments, "Investments" },
        { IncomeCategories.Gifts, "Gift/Bonus" },
        { IncomeCategories.Market, "Market Sales" },
        { IncomeCategories.Refunds, "Refunds" },
        { IncomeCategories.Other, "Other" }
    };
}
