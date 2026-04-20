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
        Transactions = []
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
