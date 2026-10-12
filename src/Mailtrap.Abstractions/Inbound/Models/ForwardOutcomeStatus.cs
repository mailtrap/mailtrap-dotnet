namespace Mailtrap.Inbound.Models;


/// <summary>
/// Outcome status of forwarding a message to a forward-rule destination.
/// </summary>
public sealed record ForwardOutcomeStatus : StringEnum<ForwardOutcomeStatus>
{
    /// <summary>
    /// The copy was accepted for sending.
    /// </summary>
    public static readonly ForwardOutcomeStatus Forwarded = Define("forwarded");

    /// <summary>
    /// The copy could not be sent.
    /// </summary>
    public static readonly ForwardOutcomeStatus Rejected = Define("rejected");
}
