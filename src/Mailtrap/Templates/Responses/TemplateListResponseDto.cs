namespace Mailtrap.Templates.Responses;


/// <summary>
/// Response DTO that unwraps the page of templates from the <c>data</c> envelope
/// and its pagination metadata.
/// </summary>
internal sealed record TemplateListResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<Template> Data { get; } = [];

    [JsonPropertyName("pagination")]
    [JsonPropertyOrder(2)]
    public Pagination? Pagination { get; set; }


    public TemplateList FromDto()
    {
        var result = new TemplateList { Pagination = Pagination };

        foreach (var template in Data)
        {
            result.Data.Add(template);
        }

        return result;
    }
}
