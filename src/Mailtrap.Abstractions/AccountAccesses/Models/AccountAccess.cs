﻿// -----------------------------------------------------------------------
// <copyright file="AccountAccess.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.AccountAccesses.Models;


/// <summary>
/// Represents account access details.
/// </summary>
public sealed record AccountAccess
{
    /// <summary>
    /// Gets the account access identifier.
    /// </summary>
    ///
    /// <value>
    /// Account access identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets the specifier type that has access to account resources.
    /// </summary>
    ///
    /// <value>
    /// Specifier type that has access to account resources.
    /// </value>
    [JsonPropertyName("specifier_type")]
    [JsonPropertyOrder(2)]
    public SpecifierType? SpecifierType { get; set; }

    /// <summary>
    /// Gets the specifier details.
    /// </summary>
    ///
    /// <value>
    /// Specifier details.
    /// </value>
    [JsonPropertyName("specifier")]
    [JsonPropertyOrder(3)]
    public Specifier? Specifier { get; set; }

    /// <summary>
    /// Gets the list of resources with permissions, the specifier has access to.
    /// </summary>
    ///
    /// <value>
    /// Contains a list of resources with permissions, the specifier has access to.
    /// </value>
    [JsonPropertyName("resources")]
    [JsonPropertyOrder(4)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<ResourceAccess> Resources { get; } = [];

    /// <summary>
    /// Gets specifier permissions for the account.
    /// </summary>
    ///
    /// <value>
    /// Specifier permissions for account.
    /// </value>
    [JsonPropertyName("permissions")]
    [JsonPropertyOrder(5)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public AccountAccessPermissions Permissions { get; } = new();
}
