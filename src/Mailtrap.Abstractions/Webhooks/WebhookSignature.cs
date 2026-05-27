using System.Security.Cryptography;
using System.Text;

namespace Mailtrap.Webhooks;


/// <summary>
/// Helpers for verifying inbound Mailtrap webhook signatures.
/// </summary>
///
/// <remarks>
/// <para>
/// Mailtrap signs every outbound webhook by computing
/// <c>HMAC-SHA256(signing_secret, raw_request_body)</c> and sending the lowercase hex digest
/// in the <c>Mailtrap-Signature</c> HTTP header. To authenticate a webhook on the receiver
/// side, compute the same digest using the <c>signing_secret</c> returned when the webhook
/// was created and compare it to the value of the header in constant time.
/// </para>
/// <para>
/// The comparison is performed in constant time to avoid timing side-channels.
/// </para>
/// <para>
/// <see cref="Verify(string, string, string)"/> never throws on inputs that could plausibly
/// arrive over the wire (<see langword="null"/> / empty strings, wrong-length signatures,
/// non-hex characters, missing secret) — it simply returns <see langword="false"/>. This
/// makes it safe to call directly from a request handler without wrapping in try/catch.
/// </para>
/// <para>
/// See the
/// <a href="https://docs.mailtrap.io/email-api-smtp/advanced/webhooks#verifying-the-signature">
/// Mailtrap documentation — Verifying the signature</a>.
/// </para>
/// </remarks>
public static class WebhookSignature
{
    /// <summary>
    /// Hex-encoded HMAC-SHA256 signature length (SHA-256 produces 32 bytes / 64 hex chars).
    /// </summary>
    public const int SignatureHexLength = 64;

    /// <summary>
    /// Verifies the HMAC-SHA256 signature of a Mailtrap webhook payload.
    /// </summary>
    ///
    /// <param name="payload">
    /// The raw request body, exactly as received. <strong>Do not</strong> parse and
    /// re-serialize the JSON — re-encoding may reorder keys or alter whitespace and
    /// invalidate the signature. In ASP.NET Core, call
    /// <c>HttpRequest.EnableBuffering()</c> and read the body via
    /// <c>new StreamReader(Request.Body).ReadToEndAsync()</c>, or bind directly to a
    /// <c>byte[]</c> / <see cref="System.IO.Stream"/> on the webhook endpoint so the
    /// body is preserved verbatim.
    /// </param>
    /// <param name="signature">
    /// The value of the <c>Mailtrap-Signature</c> HTTP header (lowercase hex string).
    /// </param>
    /// <param name="signingSecret">
    /// The webhook's <c>signing_secret</c>, returned by the Webhooks API on webhook creation.
    /// </param>
    ///
    /// <returns>
    /// <see langword="true"/> if <paramref name="signature"/> is valid for the given
    /// <paramref name="payload"/> and <paramref name="signingSecret"/>; <see langword="false"/>
    /// otherwise (including any <see langword="null"/> / empty input, wrong-length or
    /// non-hex signatures).
    /// </returns>
    public static bool Verify(string payload, string signature, string signingSecret)
    {
        if (string.IsNullOrEmpty(signature))
        {
            return false;
        }
        if (string.IsNullOrEmpty(signingSecret))
        {
            return false;
        }
        if (string.IsNullOrEmpty(payload))
        {
            return false;
        }
        if (signature.Length != SignatureHexLength)
        {
            return false;
        }

        if (!TryParseHex(signature, out var providedBytes))
        {
            return false;
        }

        byte[] expectedBytes;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signingSecret)))
        {
            expectedBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        }

        // Defensive: the hex-length check above already pins providedBytes to 32 bytes
        // (= expectedBytes.Length), but reassert before the constant-time compare so a
        // future change to SignatureHexLength can't accidentally introduce a timing leak.
        if (expectedBytes.Length != providedBytes.Length)
        {
            return false;
        }

        return FixedTimeEquals(expectedBytes, providedBytes);
    }

    /// <summary>
    /// Parses a lowercase/uppercase hex string into bytes without throwing on invalid input.
    /// </summary>
    private static bool TryParseHex(string hex, out byte[] bytes)
    {
        if ((hex.Length & 1) != 0)
        {
            bytes = [];
            return false;
        }

        var result = new byte[hex.Length / 2];
        for (var i = 0; i < result.Length; i++)
        {
            if (!TryParseHexDigit(hex[(i * 2) + 0], out var high) ||
                !TryParseHexDigit(hex[(i * 2) + 1], out var low))
            {
                bytes = [];
                return false;
            }

            result[i] = (byte)((high << 4) | low);
        }

        bytes = result;
        return true;
    }

    private static bool TryParseHexDigit(char c, out int value)
    {
        if (c is >= '0' and <= '9')
        {
            value = c - '0';
            return true;
        }
        if (c is >= 'a' and <= 'f')
        {
            value = c - 'a' + 10;
            return true;
        }
        if (c is >= 'A' and <= 'F')
        {
            value = c - 'A' + 10;
            return true;
        }

        value = 0;
        return false;
    }

    /// <summary>
    /// Constant-time byte-array equality. Equivalent to
    /// <c>CryptographicOperations.FixedTimeEquals</c>, which is unavailable on
    /// <c>netstandard2.0</c>.
    /// </summary>
    private static bool FixedTimeEquals(byte[] left, byte[] right)
    {
        // Lengths are checked up-front by the caller; equal-length guarantees a constant
        // number of loop iterations and avoids leaking length information via timing.
        var diff = 0;
        for (var i = 0; i < left.Length; i++)
        {
            diff |= left[i] ^ right[i];
        }

        return diff == 0;
    }
}
