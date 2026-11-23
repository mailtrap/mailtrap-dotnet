namespace Mailtrap.SubscriptionTopics;


internal sealed class SubscriptionTopicResource : RestResource, ISubscriptionTopicResource
{
    public SubscriptionTopicResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<SubscriptionTopic> GetDetails(CancellationToken cancellationToken = default)
    {
        var response = await Get<SubscriptionTopicResponseDto>(cancellationToken).ConfigureAwait(false);

        return response.SubscriptionTopic;
    }
}
