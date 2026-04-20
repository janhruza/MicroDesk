using System;
using System.Runtime.InteropServices;

namespace Financier.Core;

/// <summary>
/// Representing a single transaction info struct.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct TransactionInfo
{
    /// <summary>
    /// Representing the value of the transaction.
    /// </summary>
    public decimal Value;

    /// <summary>
    /// Representing a point in time when the transaction was created.
    /// </summary>
    public DateTime Timestamp;

    /// <summary>
    /// Representing the transaction type.
    /// </summary>
    public TransactionType Type;

    /// <summary>
    /// Representing the category of the transaction as a byte
    /// to be compatible with both expanse and income categories.
    /// </summary>
    public byte Category;

    /// <summary>
    /// Representing the additional user-defined textual description of the transaction.
    /// </summary>
    public string Text;
}
