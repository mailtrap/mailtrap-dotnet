namespace Mailtrap.UnitTests.ContactExports.Model;


[TestFixture]
internal sealed class ContactExportFilterBaseTests
{
    private readonly JsonSerializerOptions _options = MailtrapJsonSerializerOptions.Default;

    [Test]
    public void Serialize_Should_ProduceExpectedJson()
    {
        // Arrange
        // lang=json
        const string expectedJsonRaw = """
        {
            "filters": [
                { "name": "list_id", "operator": "equal", "value": [123, 456] },
                { "name": "subscription_status", "operator": "equal", "value": "subscribed" }
            ]
        }
        """;

        var request = new CreateContactExportRequest(
            [
                new ContactExportListIdFilter(new List<int> { 123, 456 })
                    { Operator = ContactExportFilterOperator.Equal },
                new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.Subscribed)
                    { Operator = ContactExportFilterOperator.Equal }
            ]
        );

        // Act
        // Serialize the request object
        var result = JsonSerializer.Serialize(request, _options);

        // Normalize the expected JSON using the *same* serializer options
        using var expectedDoc = JsonDocument.Parse(expectedJsonRaw);
        var expectedJson = JsonSerializer.Serialize(expectedDoc, _options);

        // Assert
        result.Should().Be(expectedJson);
    }

    [Test]
    public void Deserialize_Should_CreateCorrectTypes()
    {
        // Arrange
        // lang=json
        const string json = """
        {
            "filters": [
                { "name": "list_id", "operator": "equal", "value": [ 1, 2, 3 ] },
                { "name": "subscription_status", "operator": "equal", "value": "unsubscribed" }
            ]
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<CreateContactExportRequest>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result.Filters.Should().HaveCount(2);
        result.Filters[0].Should().BeOfType<ContactExportListIdFilter>();
        result.Filters[1].Should().BeOfType<ContactExportSubscriptionStatusFilter>();

        var listFilter = (ContactExportListIdFilter)result.Filters[0];
        listFilter.Value.Should().ContainInOrder(1, 2, 3);

        var statusFilter = (ContactExportSubscriptionStatusFilter)result.Filters[1];
        statusFilter.Value.Should().Be(ContactExportFilterSubscriptionStatus.Unsubscribed);
    }

    [Test]
    public void SerializeDeserialize_Should_PreserveDataIntegrity()
    {
        // Arrange
        var original = new CreateContactExportRequest(
            new List<ContactExportFilterBase>
            {
                new ContactExportListIdFilter(new List<int> { 789, 101 }),
                new ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus.Unsubscribed)
            }
        );

        // Act
        var json = JsonSerializer.Serialize(original, _options);
        var deserialized = JsonSerializer.Deserialize<CreateContactExportRequest>(json, _options);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(original);
    }
}
