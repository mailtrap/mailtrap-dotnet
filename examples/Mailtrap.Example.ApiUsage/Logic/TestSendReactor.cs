namespace Mailtrap.Example.ApiUsage.Logic;


internal sealed class TestSendReactor : Reactor
{
    public TestSendReactor(IMailtrapClient mailtrapClient, ILogger<InboxReactor> logger)
        : base(mailtrapClient, logger) { }


    public async Task Send(long inboxId)
    {
        ISendEmailClient emailClient = _mailtrapClient.Test(inboxId);

        SendEmailRequest sendEmailRequest = SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .To("star.lord@galaxy.net")
            .Subject("Greetings")
            .Text("Hi guys,\n\nWelcome to our Milky Way Galaxy.\n\nBest regards, John.");

        await emailClient.Send(sendEmailRequest);

        sendEmailRequest = SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Cc("star.lord@galaxy.net")
            .Subject("Invitation to Earth")
            .Html("<h3>Dear Bill</h3>,<p>It will be a great pleasure to see you on our blue planet next weekend.</p><p>Best regards, <b>John.</b></p>");

        await emailClient.Send(sendEmailRequest);

        sendEmailRequest = SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("fury.nicolas@avengers.gov")
            .Subject("Supervision request")
            .Text("Dear Mr. Fury,\n\nKindly assist with supervision on few new guests that are arriving to Earth next weekend.\n\nBest regards, John.");

        var file = "Attachment.txt";
        var bytes = await File.ReadAllBytesAsync(file);
        var fileContent = Convert.ToBase64String(bytes);

        sendEmailRequest.Attach(
            content: fileContent,
            fileName: file,
            disposition: DispositionType.Attachment,
            mimeType: MediaTypeNames.Text.Plain);

        await emailClient.Send(sendEmailRequest);
    }
}
