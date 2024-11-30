﻿// -----------------------------------------------------------------------
// <copyright file="AccountAccessResource.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.AccountAccesses;


internal sealed class AccountAccessResource : RestResource, IAccountAccessResource
{
    private const string UpdatePermissionsSegment = "permissions/bulk";


    public AccountAccessResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<UpdatePermissionsResponse> UpdatePermissions(UpdatePermissionsRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        var uri = ResourceUri.Append(UpdatePermissionsSegment);

        var result = await RestResourceCommandFactory
            .CreatePut<UpdatePermissionsRequest, UpdatePermissionsResponse>(uri, request)
            .Execute(cancellationToken)
            .ConfigureAwait(false);

        return result;
    }

    public async Task<DeleteAccountAccessResponse> Delete(CancellationToken cancellationToken = default)
        => await Delete<DeleteAccountAccessResponse>(cancellationToken).ConfigureAwait(false);
}
