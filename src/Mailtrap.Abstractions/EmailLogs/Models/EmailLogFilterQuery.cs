using System.Globalization;


namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Shared logic for emitting email log filter query parameters (operator + value(s)).
/// </summary>
internal static class EmailLogFilterQuery
{
    /// <summary>
    /// Returns an enumerable of value strings from either the multi-value list (each item's ToString()) or the single value when the list is empty.
    /// Used by enum-like filters (status, events, sending stream) and string filters to keep emission consistent.
    /// </summary>
    internal static IEnumerable<string?> GetValueStrings<T>(T? singleValue, IList<T> values) where T : class
    {
        if (values.Count > 0)
        {
            return values.Select(v => v?.ToString());
        }

        var str = singleValue?.ToString();
        return !string.IsNullOrEmpty(str) ? new[] { str } : Array.Empty<string?>();
    }

    /// <summary>
    /// Returns an enumerable of value strings from either the multi-value list or the single value when the list is empty.
    /// Used by numeric filters (e.g. sending_domain_id) with invariant culture formatting.
    /// </summary>
    internal static IEnumerable<string?> GetValueStrings(long? singleValue, IList<long> values)
    {
        return values.Count > 0
            ? values.Select(l => l.ToString(CultureInfo.InvariantCulture)).Cast<string?>()
            : (singleValue is not null ? new[] { singleValue.Value.ToString(CultureInfo.InvariantCulture) } : Array.Empty<string?>());
    }

    /// <summary>
    /// Yields the operator pair and then one pair per non-null, non-empty value string.
    /// Single value uses key [value]; multiple values use [value][] for array-style parameters.
    /// </summary>
    internal static IEnumerable<KeyValuePair<string, string>> Emit(string prefix, string field, string operatorValue, IEnumerable<string?> valueStrings)
    {
        yield return new KeyValuePair<string, string>($"{prefix}[{field}][operator]", operatorValue);
        var list = valueStrings.Where(v => !string.IsNullOrEmpty(v)).ToList();
        if (list.Count == 0)
        {
            yield break;
        }

        var valueKey = list.Count == 1 ? $"{prefix}[{field}][value]" : $"{prefix}[{field}][value][]";
        foreach (var v in list)
        {
            yield return new KeyValuePair<string, string>(valueKey, v!);
        }
    }

    /// <summary>
    /// Emits filter query parameters using GetValueStrings then Emit. Use from filters with single value + list (string or enum-like).
    /// </summary>
    internal static IEnumerable<KeyValuePair<string, string>> EmitWithValues<T>(string prefix, string field, string operatorValue, T? singleValue, IList<T> values) where T : class
        => Emit(prefix, field, operatorValue, GetValueStrings(singleValue, values));

    /// <summary>
    /// Emits filter query parameters using GetValueStrings then Emit. Use from filters with single long? + list (e.g. sending_domain_id).
    /// </summary>
    internal static IEnumerable<KeyValuePair<string, string>> EmitWithValues(string prefix, string field, string operatorValue, long? singleValue, IList<long> values)
        => Emit(prefix, field, operatorValue, GetValueStrings(singleValue, values));
}
