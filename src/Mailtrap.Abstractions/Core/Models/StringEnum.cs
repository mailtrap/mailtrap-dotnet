using System.Runtime.CompilerServices;


namespace Mailtrap.Core.Models;


/// <summary>
/// Generic string enum implementation.
/// </summary>
///
/// <typeparam name="T">
/// Particular <see cref="StringEnum{T}"/> implementation type.
/// </typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public abstract record StringEnum<T> where T : StringEnum<T>, new()
{
    private static readonly ConcurrentDictionary<string, T> s_values = new();


    /// <summary>
    /// Gets empty enum value.
    /// </summary>
    ///
    /// <value>
    /// Empty value.
    /// </value>
    public static T None { get; } = Define(string.Empty);

    /// <summary>
    /// Gets unknown enum value.<br/>
    /// E.g. that cannot be parsed.
    /// </summary>
    ///
    /// <value>
    /// Unknown value.
    /// </value>
    public static T Unknown { get; } = Define(nameof(Unknown));


    static StringEnum()
    {
        // Explicitly initialize static fields in derived enum type upon type initialization.
        // This will ensure that s_values dictionary is populated with all valid enum entries.
        RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);

        s_values.TryAdd(None._value, None);
        s_values.TryAdd(Unknown._value, Unknown);
    }

    /// <summary>
    /// Finds enum entry by the specified string value.
    /// </summary>
    ///
    /// <param name="value">
    /// String value to find enum entry for.
    /// </param>
    ///
    /// <returns>
    /// Enum entry for the specified string value if found, <see langword="null"/> otherwise.
    /// </returns>
    ///
    /// <remarks>
    /// Search is case-sensitive.
    /// </remarks>
    public static T Find(string? value)
    {
        return
            string.IsNullOrEmpty(value) ? None :
            s_values.TryGetValue(value!, out var result) ? result :
            Unknown;
    }


    /// <summary>
    /// Defines new enum entry and adds it to the values dictionary.
    /// </summary>
    ///
    /// <param name="value">
    /// Enum entry value.
    /// </param>
    ///
    /// <returns>
    /// Created enum entry instance.
    /// </returns>
    protected static T Define(string value)
    {
        Ensure.NotNull(value, nameof(value));

        var enumValue = new T { _value = value };

        var added = s_values.TryAdd(value, enumValue);

        return added
            ? enumValue
            : throw new ArgumentException($"Attempt to define a duplicate value '{value}' in '{typeof(T).Name}'.", nameof(value));
    }

    /// <summary>
    /// Checks whether the specified value is defined in the enum.
    /// </summary>
    /// <param name="value">
    /// Value to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the specified value is defined in the enum, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    /// The method returns <see langword="false"/> for <see langword="null"/>, <see cref="None"/> and <see cref="Unknown"/>.
    /// </remarks>
    internal static bool IsDefined(T? value)
    {
        return value is not null
            && !ReferenceEquals(value, None)
            && !ReferenceEquals(value, Unknown)
            && s_values.Values.Contains(value);
    }


    private string _value = string.Empty;


    /// <inheritdoc />
    public sealed override string ToString() => _value;

    /// <inheritdoc />
    public override int GetHashCode() => _value.GetHashCode();

    // We can safely do a reference comparison here, because we know that all enum values are singletons.
    /// <inheritdoc />
    public virtual bool Equals(T? other) => ReferenceEquals(this, other);
}
