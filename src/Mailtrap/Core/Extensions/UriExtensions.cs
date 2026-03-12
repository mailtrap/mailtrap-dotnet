namespace Mailtrap.Core.Extensions;


/// <summary>
/// A set of helpers for <see cref="Uri"/>.
/// </summary>
internal static class UriExtensions
{
    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="url"/> is <see langword="null"/>.
    /// </exception>
    internal static Uri ToUri(this string url)
    {
        Ensure.NotNullOrEmpty(url, nameof(url));

        return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var result)
            ? result
            : throw new ArgumentException("Invalid URL format", nameof(url));
    }

    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="url"/> is <see langword="null"/>.
    /// </exception>
    internal static Uri ToAbsoluteUri(this string url)
    {
        Ensure.NotNullOrEmpty(url, nameof(url));

        return Uri.TryCreate(url, UriKind.Absolute, out var result)
            ? result
            : throw new ArgumentException("Invalid URL format - absolute URL is expected", nameof(url));
    }

    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="url"/> is <see langword="null"/>.
    /// </exception>
    internal static Uri ToRelativeUri(this string url)
    {
        Ensure.NotNullOrEmpty(url, nameof(url));

        return Uri.TryCreate(url, UriKind.Relative, out var result)
            ? result
            : throw new ArgumentException("Invalid URL format - relative URL is expected", nameof(url));
    }

    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="baseUri"/> or <paramref name="segments"/> is <see langword="null"/>.
    /// </exception>
    internal static Uri Append(this Uri baseUri, params string[] segments)
    {
        Ensure.NotNull(baseUri, nameof(baseUri));
        Ensure.NotNull(segments, nameof(segments));

        var result = new UriBuilder(baseUri);

        result.Path = result.Path.Length > 1
            ? $"{result.Path.TrimEnd('/')}/{string.Join("/", segments)}"
            : string.Join("/", segments);

        return result.Uri;
    }

    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="baseUri"/> is <see langword="null"/>.<br />
    /// When provided <paramref name="id"/> is less than or equal to zero.
    /// </exception>
    internal static Uri Append(this Uri baseUri, long id)
    {
        Ensure.NotNull(baseUri, nameof(baseUri));
        Ensure.GreaterThanZero(id, nameof(id));

        return baseUri.Append(id.ToUriSegment());
    }

    /// <summary>
    /// Appends a single query parameter to the URI. The value is encoded with <see cref="Uri.EscapeDataString"/>.
    /// </summary>
    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="uri"/> is <see langword="null"/>.<br />
    /// When provided <paramref name="key"/> is <see langword="null"/> or <see cref="string.Empty"/>.<br />
    /// When provided <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    internal static Uri AppendQueryParameter(this Uri uri, string key, string? value)
    {
        Ensure.NotNull(uri, nameof(uri));
        Ensure.NotNullOrEmpty(key, nameof(key));
        Ensure.NotNull(value, nameof(value));

        var encodedValue = Uri.EscapeDataString(value);
        var result = new UriBuilder(uri);

        result.Query = result.Query.Length > 1
            ? $"{result.Query.Substring(1)}&{key}={encodedValue}"
            : $"{key}={encodedValue}";

        return result.Uri;
    }

    /// <summary>
    /// Appends multiple query parameters to the URI. Values are encoded via <see cref="AppendQueryParameter"/>.
    /// Duplicate keys are supported (e.g. multiple filters[field][value] for array values).
    /// </summary>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="uri"/> or <paramref name="parameters"/> is <see langword="null"/>.
    /// </exception>
    internal static Uri AppendQueryParameters(this Uri uri, IEnumerable<KeyValuePair<string, string>> parameters)
    {
        Ensure.NotNull(uri, nameof(uri));
        Ensure.NotNull(parameters, nameof(parameters));

        foreach (var p in parameters.Where(p => !string.IsNullOrEmpty(p.Key)))
        {
            uri = uri.AppendQueryParameter(p.Key, p.Value);
        }

        return uri;
    }

    /// <exception cref = "ArgumentNullException" >
    /// When provided <paramref name="url"/> is <see langword="null"/>.
    /// </exception>
    /// 
    /// <exception cref = "ArgumentException" >
    /// When provided <paramref name="url"/> isn't a valid absolute URI.
    /// </exception>
    internal static Uri EnsureAbsoluteUri(this Uri url)
    {
        Ensure.NotNull(url, nameof(url));

        return
            url.IsAbsoluteUri
            ? url
            : throw new ArgumentException("Invalid URL format - absolute URL is expected", nameof(url));
    }

    internal static string ToUriSegment<T>(this T id) where T : IFormattable
        => id.ToString(null, CultureInfo.InvariantCulture);
}
