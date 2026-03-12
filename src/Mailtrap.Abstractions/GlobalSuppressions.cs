
// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.




[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Contact Import collection nested to Contacts should be named Imports", Scope = "member", Target = "~M:Mailtrap.Contacts.IContactCollectionResource.Imports~Mailtrap.ContactImports.IContactImportCollectionResource")]
[assembly: SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Contact field data type should be named according to its value", Scope = "member", Target = "~F:Mailtrap.ContactFields.Models.ContactFieldDataType.Integer")]
[assembly: SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Contact field data type should be named according to its value", Scope = "member", Target = "~F:Mailtrap.ContactFields.Models.ContactFieldDataType.Float")]

[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO", Scope = "type", Target = "~T:Mailtrap.Emails.Requests.SendEmailRequest")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO", Scope = "type", Target = "~T:Mailtrap.Emails.Requests.EmailRequest")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO", Scope = "type", Target = "~T:Mailtrap.Emails.Requests.BatchEmailRequest")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Suppression Sending Stream type should be named according to its value", Scope = "type", Target = "~T:Mailtrap.Suppressions.Models.SuppressionSendingStream")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Email log sending stream type matches API value", Scope = "type", Target = "~T:Mailtrap.EmailLogs.Models.SendingStream")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO; setter required for JSON deserialization on netstandard2.0/pre-.NET 8", Scope = "member", Target = "~P:Mailtrap.EmailLogs.Models.EmailLogsListResponse.Messages")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO; setter required for JSON deserialization when API returns null", Scope = "member", Target = "~P:Mailtrap.EmailLogs.Models.EmailLogMessage.CustomVariables")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO; setter required for JSON deserialization when API returns null", Scope = "member", Target = "~P:Mailtrap.EmailLogs.Models.EmailLogMessage.TemplateVariables")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO; setter required for JSON deserialization when API returns null", Scope = "member", Target = "~P:Mailtrap.EmailLogs.Models.EmailLogMessage.Events")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "API returns string; signed/temporary URL", Scope = "member", Target = "~P:Mailtrap.EmailLogs.Models.EmailLogMessage.RawMessageUrl")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "API returns string; avoid Uri parsing for signed URLs", Scope = "member", Target = "~P:Mailtrap.EmailLogs.Models.EventDetailsClick.ClickUrl")]
