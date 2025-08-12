﻿using System;

namespace QuickNotes.Core;

/// <summary>
/// Representing a single note object.
/// </summary>
/// <param name="Text">Representing the text of the note.</param>
/// <param name="Creation">Representing the note's creation date and time.</param>
/// <param name="Expiration">
/// Representing the note's expiration date and time (if any).
/// If the <paramref name="Expiration"/> has the same value as <paramref name="Creation"/>, the note will be marked as indefinite.
/// </param>
public record struct Note(string Text, DateTime Creation, DateTime Expiration)
{
    /// <summary>
    /// Determines whether the note can expire or not.
    /// </summary>
    /// <returns></returns>
    public readonly bool IsIndefinite() => this.Expiration == this.Creation;
}
