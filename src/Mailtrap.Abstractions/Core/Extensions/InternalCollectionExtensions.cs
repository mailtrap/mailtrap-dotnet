namespace Mailtrap.Core.Extensions;

/// <summary>
/// A set of helpers for managing collections.
/// </summary>
internal static class InternalCollectionExtensions
{
    /// <summary>
    /// Returns a new collection by making a defensive copy of the provided <paramref name="items"/>.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// Item type.
    /// </typeparam>
    ///
    /// <param name="items">
    /// The items to copy.
    /// </param>
    ///
    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="items"/> is <see langword="null"/>.
    /// </exception>
    internal static IList<T> Clone<T>(this IEnumerable<T> items)
    {
        Ensure.NotNull(items, nameof(items));

        // Defensive copy to prevent post-ctor mutation.
        return items is List<T> list
                        ? new List<T>(list)    // defensive copy when already a List<T>
                        : new List<T>(items); // otherwise enumerate once
    }

    /// <summary>
    /// Returns a new dictionary by making a defensive copy of the provided <paramref name="items"/>.
    /// </summary>
    ///
    /// <typeparam name="TKey">
    /// The type of keys in the dictionary.
    /// </typeparam>
    ///
    /// <typeparam name="TValue">
    /// The type of values in the dictionary.
    /// </typeparam>
    ///
    /// <param name="items">
    /// The items to copy.
    /// </param>
    ///
    /// <returns>
    /// A new dictionary containing copies of the provided items.
    /// </returns>
    ///
    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="items"/> is <see langword="null"/>.
    /// </exception>
    internal static IDictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> items)
    {
        Ensure.NotNull(items, nameof(items));

        // Defensive copy to prevent post-ctor mutation.
        return items is Dictionary<TKey, TValue> dict
            ? new Dictionary<TKey, TValue>(dict)    // defensive copy when already a Dictionary<TKey, TValue>
            : new Dictionary<TKey, TValue>(items);  // otherwise enumerate once
    }
}
