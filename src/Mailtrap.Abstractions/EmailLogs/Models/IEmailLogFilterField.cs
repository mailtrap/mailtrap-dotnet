namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Contract for a single email log filter field so it can be emitted as query parameters.
/// </summary>
internal interface IEmailLogFilterField
{
    public IEnumerable<KeyValuePair<string, string>> ToQueryParameters(string prefix, string field);
}
