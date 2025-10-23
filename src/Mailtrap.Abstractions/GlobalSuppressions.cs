
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
