using System.Collections.Generic;

namespace AarpgTutorial.Common.Utilities;

public static class Extensions
{
    /// <summary>Returns true if the value is present in the provided set.</summary>
    /// <param name="value">The value to search for.</param>
    /// <param name="set">The values to search within.</param>
    public static bool In<T>(this T value, params T[] set) =>
        ((ICollection<T>)set).Contains(value);
}