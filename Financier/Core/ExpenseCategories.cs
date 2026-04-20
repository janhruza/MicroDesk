namespace Financier.Core;

/// <summary>
/// Representing all valid expanse categories.
/// </summary>
public enum ExpenseCategories : byte
{
    /// <summary>
    /// Housing expenses, such as a rent, utilities, etc.
    /// </summary>
    Housing = 128,

    /// <summary>
    /// All expenses related to grocery shopping.
    /// </summary>
    Groceries,

    /// <summary>
    /// Fuel, public transportation, etc.
    /// </summary>
    Travel,

    /// <summary>
    /// Other than home-cooked meals, e. g. restaurant expanses.
    /// </summary>
    Meals,

    /// <summary>
    /// Movies, streaming services or any other entertainment expenses.
    /// </summary>
    Entertainment,

    /// <summary>
    /// All expenses related to health and beauty.
    /// </summary>
    Health,

    /// <summary>
    /// All kinds of shopping other than grocery shopping.
    /// </summary>
    Shopping,

    /// <summary>
    /// All personal-growth related purchases.
    /// </summary>
    PersonalGrowth,

    /// <summary>
    /// Other financial expenses, such as interests, taxes or other payments.
    /// </summary>
    Finances,

    /// <summary>
    /// Other kinds of expenses.
    /// </summary>
    Other
}
