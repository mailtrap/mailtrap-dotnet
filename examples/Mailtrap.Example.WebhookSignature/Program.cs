using System.Security.Cryptography;
using System.Text;
using Mailtrap.Webhooks;

var payload = "{\"event\":\"delivery\",\"message_id\":\"abc-123\"}";
var signingSecret = "8d9a3c0e7f5b2d4a6c1e9f8b3a7d5c2e";

string signature;
using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signingSecret)))
{
    signature = Convert.ToHexStringLower(hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));
}

if (!WebhookSignature.Verify(payload, signature, signingSecret))
{
    throw new InvalidOperationException("Signature verification failed!");
}
