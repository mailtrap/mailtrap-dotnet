using System.Net;
using System.Text;
using Mailtrap.Webhooks;

var signingSecret = Environment.GetEnvironmentVariable("MAILTRAP_WEBHOOK_SIGNING_SECRET") ?? string.Empty;

using var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:9292/webhooks/mailtrap/");
listener.Start();

while (true)
{
    var context = await listener.GetContextAsync();
    var request = context.Request;
    var response = context.Response;

    // Use the raw request body — parsing and re-serializing the JSON may
    // reorder keys or alter whitespace and invalidate the signature.
    string payload;
    using (var reader = new StreamReader(request.InputStream, Encoding.UTF8))
    {
        payload = await reader.ReadToEndAsync();
    }
    var signature = request.Headers["Mailtrap-Signature"] ?? string.Empty;

    if (!WebhookSignature.Verify(payload, signature, signingSecret))
    {
        response.StatusCode = (int)HttpStatusCode.Unauthorized;
        response.Close();
        continue;
    }

    response.StatusCode = (int)HttpStatusCode.OK;
    response.Close();
}
