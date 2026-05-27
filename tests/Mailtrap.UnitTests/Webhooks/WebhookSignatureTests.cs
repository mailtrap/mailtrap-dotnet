using System.Security.Cryptography;

namespace Mailtrap.UnitTests.Webhooks;


[TestFixture]
internal sealed class WebhookSignatureTests
{
    // ---------------------------------------------------------------------
    // Cross-SDK shared fixture — DO NOT CHANGE.
    //
    // The same (payload, signing_secret, expected_signature) triple is
    // embedded verbatim in the test suites of every official Mailtrap SDK
    // (Ruby, Python, PHP, Node.js, Java, .NET) to guarantee byte-for-byte
    // compatibility of the verification algorithm across languages. Keep
    // these three strings in sync with the other SDKs.
    // ---------------------------------------------------------------------
    private const string FixturePayload =
        "{\"event\":\"delivery\",\"sending_stream\":\"transactional\",\"category\":\"welcome\","
        + "\"message_id\":\"a8b1d8f6-1f8d-4a3c-9b2e-1a2b3c4d5e6f\","
        + "\"email\":\"recipient@example.com\","
        + "\"event_id\":\"f1e2d3c4-b5a6-7890-1234-567890abcdef\","
        + "\"timestamp\":1716070000}";

    private const string FixtureSigningSecret = "8d9a3c0e7f5b2d4a6c1e9f8b3a7d5c2e";

    private const string FixtureExpectedSignature =
        "6d262e2611cd09be1f948382b5c611d63b0e585c4c9c5e40139d6ac3876d5433";


    // ---------------------------------------------------------------------
    // 1. Valid signature → true
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithValidSignature_ReturnsTrue()
    {
        WebhookSignature
            .Verify(FixturePayload, FixtureExpectedSignature, FixtureSigningSecret)
            .Should().BeTrue();
    }

    // ---------------------------------------------------------------------
    // 2. Wrong secret → false
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithWrongSecret_ReturnsFalse()
    {
        WebhookSignature
            .Verify(FixturePayload, FixtureExpectedSignature, "wrong_secret_value")
            .Should().BeFalse();
    }

    // ---------------------------------------------------------------------
    // 3. Payload tampered (one byte changed) → false
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithTamperedPayload_ReturnsFalse()
    {
        // Flip "delivery" to "delivere" — same length, different bytes.
        var tampered = FixturePayload.Replace("\"delivery\"", "\"delivere\"", StringComparison.Ordinal);

        WebhookSignature
            .Verify(tampered, FixtureExpectedSignature, FixtureSigningSecret)
            .Should().BeFalse();
    }

    // ---------------------------------------------------------------------
    // 4. Signature with wrong length → false (no throw)
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithSignatureOfWrongLength_ReturnsFalse()
    {
        var tooShort = FixtureExpectedSignature[..63];
        var tooLong = FixtureExpectedSignature + "a";

        WebhookSignature
            .Verify(FixturePayload, tooShort, FixtureSigningSecret)
            .Should().BeFalse();

        WebhookSignature
            .Verify(FixturePayload, tooLong, FixtureSigningSecret)
            .Should().BeFalse();
    }

    // ---------------------------------------------------------------------
    // 5. Signature with non-hex characters → false (no throw)
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithNonHexCharactersInSignature_ReturnsFalse()
    {
        // Same length (64), but contains 'z' which is not a hex digit.
        var nonHex = string.Concat("z", FixtureExpectedSignature.AsSpan(1));
        nonHex.Should().HaveLength(WebhookSignature.SignatureHexLength);

        WebhookSignature
            .Verify(FixturePayload, nonHex, FixtureSigningSecret)
            .Should().BeFalse();
    }

    // ---------------------------------------------------------------------
    // 6. Empty signature string → false
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithEmptySignature_ReturnsFalse()
    {
        WebhookSignature
            .Verify(FixturePayload, string.Empty, FixtureSigningSecret)
            .Should().BeFalse();
    }

    // ---------------------------------------------------------------------
    // 7. Empty signingSecret → false
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithEmptySigningSecret_ReturnsFalse()
    {
        WebhookSignature
            .Verify(FixturePayload, FixtureExpectedSignature, string.Empty)
            .Should().BeFalse();
    }

    // ---------------------------------------------------------------------
    // 8. Empty payload with non-empty signature → false
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithEmptyPayload_ReturnsFalse()
    {
        WebhookSignature
            .Verify(string.Empty, FixtureExpectedSignature, FixtureSigningSecret)
            .Should().BeFalse();
    }

    // ---------------------------------------------------------------------
    // 9. Known-good fixture round-trip — independently recompute the HMAC
    //    in the test (not via the helper) and assert it matches both the
    //    embedded expected signature AND the helper's verdict.
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_FixtureRoundTrip_MatchesIndependentlyComputedHmac()
    {
        // Recompute the HMAC-SHA256 independently of the helper, using BCL
        // primitives directly. If this drifts from FixtureExpectedSignature,
        // either the fixture is wrong or the algorithm/encoding has changed.
        byte[] digest;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(FixtureSigningSecret)))
        {
            digest = hmac.ComputeHash(Encoding.UTF8.GetBytes(FixturePayload));
        }

        var computedHex = ToLowerHex(digest);

        computedHex.Should().Be(
            FixtureExpectedSignature,
            "independently computed HMAC must equal embedded fixture signature");

        WebhookSignature
            .Verify(FixturePayload, FixtureExpectedSignature, FixtureSigningSecret)
            .Should().BeTrue("helper must agree the fixture is valid");
    }

    // ---------------------------------------------------------------------
    // Bonus: null inputs → false (no NullReferenceException)
    // ---------------------------------------------------------------------
    [Test]
    public void Verify_WithNullPayload_ReturnsFalse()
    {
        WebhookSignature
            .Verify(null!, FixtureExpectedSignature, FixtureSigningSecret)
            .Should().BeFalse();
    }

    [Test]
    public void Verify_WithNullSignature_ReturnsFalse()
    {
        WebhookSignature
            .Verify(FixturePayload, null!, FixtureSigningSecret)
            .Should().BeFalse();
    }

    [Test]
    public void Verify_WithNullSigningSecret_ReturnsFalse()
    {
        WebhookSignature
            .Verify(FixturePayload, FixtureExpectedSignature, null!)
            .Should().BeFalse();
    }


    private static string ToLowerHex(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
        }
        return sb.ToString();
    }
}
