namespace Mailtrap.Templates;


/// <summary>
/// Represents the templates collection resource, served by the paginated <c>/api/templates</c> endpoints.
/// </summary>
public interface ITemplateCollectionResource : IRestResource
{
    /// <summary>
    /// Gets a page of template details.
    /// </summary>
    ///
    /// <param name="filter">
    /// Pagination parameters, or <see langword="null"/> to get the first page with the default page size.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Page of template details with pagination metadata.
    /// </returns>
    public Task<TemplateList> GetAll(TemplateListFilter? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new template with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Request containing template details for creation.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetAll(TemplateListFilter?, CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Created template details.
    /// </returns>
    public Task<Template> Create(CreateTemplateRequest request, CancellationToken cancellationToken = default);
}
