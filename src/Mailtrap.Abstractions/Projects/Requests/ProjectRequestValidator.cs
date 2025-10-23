namespace Mailtrap.Projects.Requests;


/// <summary>
/// Validator for Create/Update project requests.<br />
/// Ensures project's name length is within the allowed range.
/// </summary>
public sealed class ProjectRequestValidator : AbstractValidator<ProjectRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static ProjectRequestValidator Instance { get; } = new();


    /// <summary>
    /// Primary constructor.
    /// </summary>
    public ProjectRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty().Length(2, 100);
    }
}
