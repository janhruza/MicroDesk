namespace Financier.Core;

/// <summary>
/// Representing all valid income categories.
/// </summary>
public enum IncomeCategories : byte
{
    /// <summary>
    /// Salary or wage related incomes.
    /// </summary>
    Salary,

    /// <summary>
    /// Business-related incomes.
    /// </summary>
    Business,

    /// <summary>
    /// Investment incomes.
    /// </summary>
    Investments,

    /// <summary>
    /// Gifts or other financial bonuses.
    /// </summary>
    Gifts,

    /// <summary>
    /// Personal incomes in a form of casual personal items being sold.
    /// </summary>
    Market,

    /// <summary>
    /// Monetary refunds.
    /// </summary>
    Refunds,

    /// <summary>
    /// Other kinds of personal income.
    /// </summary>
    Other
}
