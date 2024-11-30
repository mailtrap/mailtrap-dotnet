﻿// -----------------------------------------------------------------------
// <copyright file="IRestResourceCommandFactory.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.Rest;


internal interface IRestResourceCommandFactory
{
    public IRestResourceCommand<TResponse> CreateGet<TResponse>(Uri resourceUri, params string[] additionalAcceptContentTypes);
    public IRestResourceCommand<string> CreatePlainText(Uri resourceUri, params string[] additionalAcceptContentTypes);
    public IRestResourceCommand<TResponse> CreatePatch<TResponse>(Uri resourceUri);
    public IRestResourceCommand<TResponse> CreateDelete<TResponse>(Uri resourceUri);

    public IRestResourceCommand<HttpStatusCode> CreatePostWithStatusCodeResult<TRequest>(Uri resourceUri, TRequest request) where TRequest : class;

    public IRestResourceCommand<TResponse> CreatePost<TRequest, TResponse>(Uri resourceUri, TRequest request) where TRequest : class;
    public IRestResourceCommand<TResponse> CreatePut<TRequest, TResponse>(Uri resourceUri, TRequest request) where TRequest : class;
    public IRestResourceCommand<TResponse> CreatePatchWithContent<TRequest, TResponse>(Uri resourceUri, TRequest request) where TRequest : class;
}
