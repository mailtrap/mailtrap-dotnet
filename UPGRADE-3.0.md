UPGRADE FROM 2.x TO 3.0 (.NET)
=======================

### Summary

This release contains a **BC BREAK** refactor of the email client surface:

- `IEmailClient` has been refactored to be a **base interface** `IEmailClient<TRequest, TResponse>` and defines a common contract for both single and batch email clients.
- Two specialized interfaces were introduced:
  - `ISendEmailClient` — for single-send (`SendEmailRequest` / `SendEmailResponse`).
  - `IBatchEmailClient` — for batch sends (`BatchEmailRequest` / `BatchEmailResponse`).
- `IMailtrapClient` email entry points changed:
  - `Email()`, `Test()`, `Bulk()`, `Transactional()` now return `ISendEmailClient`.
  - New methods `BatchEmail()`, `BatchTest()`, `BatchBulk()`, `BatchTransactional()` return `IBatchEmailClient`.

### Why this change

The split clarifies **single-send vs batch-send** behavior and request/response types, enabling **stronger typing** and a **clearer API surface** for bulk operations.

---

### Email Client Refactor

- **BC BREAK**: The `IEmailClient` interface has been **refactored** into a **generic base interface** to unify all email sending operations.

  It now defines a common contract for both single and batch email clients.
  ```csharp
  public interface IEmailClient<TRequest, TResponse>
      where TRequest : class
      where TResponse : class
  {
      Task<TResponse> Send(TRequest request, CancellationToken cancellationToken = default);
  }
  ```

  ___Before (v2.x)___:

  ```csharp
  public interface IEmailClient
  {
      Task<SendEmailResponse> Send(SendEmailRequest request);
  }
  ```

  ___After (v3.0)___:

  ```csharp
  public interface ISendEmailClient : IEmailClient<SendEmailRequest, SendEmailResponse> { }

  public interface IBatchEmailClient : IEmailClient<BatchEmailRequest, BatchEmailResponse> { }
  ```
  > **Migration note:** All direct usages of `IEmailClient` should now be replaced with `ISendEmailClient` or `IBatchEmailClient` depending on your use case.

---

### MailtrapClient Interface Changes

- **BC BREAK**: Methods in `IMailtrapClient` that previously returned `IEmailClient` now return `ISendEmailClient`.

  #### Affected methods:

  - `Email()`
  - `Transactional()`
  - `Bulk()`
  - `Test(long inboxId)`

  ___Before (v2.x)___:

  ```csharp
  IMailtrapClient client = new MailtrapClient(apiKey);
  IEmailClient emailClient = client.Email();
  await emailClient.Send(new SendEmailRequest());
  ```

  ___After (v3.0)___:

  ```csharp
  IMailtrapClient client = new MailtrapClient(apiKey);
  ISendEmailClient sendClient = client.Email();
  await sendClient.Send(new SendEmailRequest());
  ```

- **NEW**: Introduced dedicated batch email methods returning `IBatchEmailClient`.

  | Method                    | Description                                                  |
  | ------------------------- | ------------------------------------------------------------ |
  | `BatchEmail()`            | Returns a default batch email client based on configuration. |
  | `BatchTransactional()`    | Returns a client for sending batch transactional emails.     |
  | `BatchBulk()`             | Returns a client for sending batch bulk emails.              |
  | `BatchTest(long inboxId)` | Returns a client for sending batch test emails.              |

  ___Example___:

  ```csharp
  IBatchEmailClient batchClient = client.BatchEmail();
  await batchClient.Send(new BatchEmailRequest
  {
      Base = EmailRequest.Create() { ... },
      Requests = new List<SendEmailRequest> { ... }
  });
  ```

---

### Batch Email Builder

- **NEW**: Added `BatchEmailRequestBuilder` — a fluent helper for constructing batch email requests. Similar in design to existing builders: `EmailRequestBuilder` and `SendEmailRequestBuilder`.

  ___Example___:

  ```csharp
  using Mailtrap.Emails.Requests;

  var batchRequest = new BatchEmailRequest()
      .Base(b => b
          .From("sender@example.com")
          .Subject("Greetings"))
      .Requests(
          r => r.To("user1@example.com").Html("<p>Hello User 1!</p>")
      );

  await client.BatchEmail().Send(batchRequest);
  ```

---

### Summary of Changes

| Area                                               | Change                                                                     | Type         |
| -------------------------------------------------- | -------------------------------------------------------------------------- | ------------ |
| `IEmailClient`                                     | Refactored into generic base interface `IEmailClient<TRequest, TResponse>` | **BC BREAK** |
| `ISendEmailClient`                                 | New interface for single email sending                                     | **New**      |
| `IBatchEmailClient`                                | New interface for batch email sending                                      | **New**      |
| `IMailtrapClient.Email()`, `Transactional()`, etc. | Now return `ISendEmailClient`                                              | **BC BREAK** |
| `IMailtrapClient` batch methods                    | Added `BatchEmail()`, `BatchTransactional()`, `BatchBulk()`, `BatchTest()` | **New**      |
| `BatchEmailRequestBuilder`                         | Fluent builder for batch requests                                          | **New**      |

---

## Migration examples

___Before (v2.x)___:

```csharp
// Example using old IEmailClient directly
var client = new MailtrapClient(apiKey);
var emailClient = client.Email();

await emailClient.Send(new SendEmailRequest
{
    To = new List<string> { "user@example.com" },
    Subject = "Welcome!",
    Html = "<p>Hello!</p>"
});
```

___After (v3.0)___:

```csharp
// Prefer the specialized interface

await emailClient.Send(sendRequest);
var client = new MailtrapClient(apiKey);

// Single email
var sendClient = client.Email(); // Transactional(), Bulk(), Test() -> ISendEmailClient
await sendClient.Send(new SendEmailRequest
{
    To = new List<string> { "user@example.com" },
    Subject = "Welcome!",
    Html = "<p>Hello!</p>"
});

// Batch email
var batchRequest = new BatchEmailRequest()
    .Requests(
        r => r.To("first@example.com").Subject("Hi 1"),
        r => r.To("second@example.com").Subject("Hi 2")
    );

await client
    .BatchEmail() // BatchTransactional(), BatchBulk(), BatchTest() -> IBatchEmailClient
    .Send(batchRequest);
```

---

### If you implemented your own email client types (custom DI registrations)

- Modify implementations to implement the new specialized interfaces:
  - Single-send implementation:
    ```csharp
    public class MySendEmailClient : ISendEmailClient { ... }
    ```
  - Batch-send implementation:
    ```csharp
    public class MyBatchEmailClient : IBatchEmailClient { ... }
    ```

- Update dependency injection registrations to register the new interfaces:
  - Example (Microsoft DI):
    ```csharp
    services.AddSingleton<ISendEmailClient, MySendEmailClient>();
    services.AddSingleton<IBatchEmailClient, MyBatchEmailClient>();
    ```

- If you registered or resolved the generic `IEmailClient<...>` at runtime, replace those registrations/resolutions with the specialized interfaces.

---

### Common compilation errors and fixes

- CS0311 or generic constraint errors referencing `IEmailClient<TRequest,TResponse>`:
  - Cause: code still references the old generic type or a wrong generic constraint.
  - Fix: replace references with `ISendEmailClient` or `IBatchEmailClient`, and update any generic constraints to match the new interfaces.

- "Missing method" or "method does not exist" errors for batch methods:
  - Cause: using `Email()`/`Bulk()`/`Transactional()`/`Test()` where a batch operation was intended.
  - Fix: switch to the new `BatchEmail()` / `BatchBulk()` / `BatchTransactional()` / `BatchTest()` methods which return `IBatchEmailClient`.

- Test/mocking failures due to interface changes:
  - Cause: mocks/stubs implement the old `IEmailClient` type.
  - Fix: update mocks to implement `ISendEmailClient` or `IBatchEmailClient`. Example (Moq):
    ```csharp
    var mockSend = new Mock<ISendEmailClient>();
    var mockBatch = new Mock<IBatchEmailClient>();
    ```

- DI resolution errors (unable to resolve service):
  - Cause: DI registration still targets the old generic interface.
  - Fix: register the concrete implementation under `ISendEmailClient` / `IBatchEmailClient` as needed.

---

### Quick migration checklist (2.x → 3.x)
- [ ] Update NuGet package to 3.0.0 (or the appropriate prerelease).
- [ ] Search codebase for `IEmailClient` and replace wit either `ISendEmailClient` or `IBatchEmailClient`.
- [ ] Replace usages of batch sends that used `Email()`/`Bulk()`/`Transactional()`/`Test()` with the new Batch* methods.
- [ ] Update DI registrations & factories to return the new interfaces.
- [ ] Use `BatchEmailRequestBuilder` for building batch requests.
- [ ] Test all email sending paths (single and batch) after migration.

> Runtime behavior is unchanged for single sends; This refactor improves SDK consistency, providing a clear separation between single and batch email operations while maintaining a unified interface structure for future extensibility.
