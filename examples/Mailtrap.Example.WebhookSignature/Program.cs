using System.Security.Cryptography;
using System.Text;
using Mailtrap.Webhooks;

// --- Direct verification (e.g. for unit tests or custom routers) ----------
var payload = "{\"event\":\"delivery\",\"message_id\":\"abc-123\"}";
var signingSecret = "8d9a3c0e7f5b2d4a6c1e9f8b3a7d5c2e";

string signature;
using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signingSecret)))
{
    signature = Convert.ToHexStringLower(hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));
}

if (!WebhookSignature.Verify(payload, signature, signingSecret))
{
    throw new InvalidOperationException("Expected valid signature to verify");
}

// Bad input never throws — it returns false:
if (WebhookSignature.Verify(payload, "not-hex", signingSecret) ||
    WebhookSignature.Verify(payload, "", signingSecret))
{
    throw new InvalidOperationException("Expected bad signature to fail verification");
}
