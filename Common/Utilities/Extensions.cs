using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AarpgTutorial.Common.Utilities;

/// <summary>General-purpose extension methods used across the project.</summary>
public static class Extensions
{
    /// <summary>Returns true if the value is present in the provided set.</summary>
    /// <param name="value">The value to search for.</param>
    /// <param name="set">The values to search within.</param>
    public static bool In<T>(this T value, params T[] set) =>
        ((ICollection<T>)set).Contains(value);

    /// <summary>Throws <see cref="ExportNullException"/> if the export is null. Call in <c>_Ready()</c> for required exports.</summary>
    /// <param name="export">Export object to be verified</param>
    /// <param name="exportName">Automatically captured from the call site via <see cref="CallerArgumentExpressionAttribute"/>.</param>
    public static T Require<T>([NotNull] this T? export, [CallerArgumentExpression(nameof(export))] string? exportName = null) where T : class
    {
        return export ?? throw new ExportNullException(typeof(T), exportName!);
    }

    /// <summary>Logs a Godot warning if the value is null, then returns the value for further null checking.</summary>
    /// <param name="value">The value to check.</param>
    /// <param name="message">The warning message to log if null.</param>
    public static T? WarnIfNull<T>(this T? value, string message) where T : class
    {
        if (value is null) Godot.GD.PushWarning(message);
        return value;
    }
}