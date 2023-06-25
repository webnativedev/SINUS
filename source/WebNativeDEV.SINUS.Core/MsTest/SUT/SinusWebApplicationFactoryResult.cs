// <copyright file="SinusWebApplicationFactoryResult.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest.SUT;

using Microsoft.AspNetCore.Mvc.Testing;

/// <summary>
/// Represents the result of a created system under test.
/// </summary>
/// <typeparam name="TProgram">Class that bootstraps the system under test.</typeparam>
internal sealed class SinusWebApplicationFactoryResult<TProgram> : IDisposable, ISinusWebApplicationFactoryResult
    where TProgram : class
{
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="SinusWebApplicationFactoryResult{TProgram}"/> class.
    /// </summary>
    /// <param name="webApplicationFactory">Factory that creates a system under test.</param>
    /// <param name="httpClient">Accessing http client of the web application.</param>
    public SinusWebApplicationFactoryResult(WebApplicationFactory<TProgram> webApplicationFactory, HttpClient httpClient)
    {
        this.WebApplicationFactory = webApplicationFactory;
        this.HttpClient = httpClient;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="SinusWebApplicationFactoryResult{TProgram}"/> class.
    /// </summary>
    ~SinusWebApplicationFactoryResult()
    {
        this.Dispose(disposing: false);
    }

    /// <summary>
    /// Gets the WebApplicationFactory.
    /// </summary>
    public WebApplicationFactory<TProgram> WebApplicationFactory { get; }

    /// <summary>
    /// Gets the HttpClient accessing the WebApplication.
    /// </summary>
    public HttpClient HttpClient { get; }

    /// <summary>
    /// Method satisfying the IDisposable interface.
    /// </summary>
    public void Dispose()
    {
        // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the current object.
    /// </summary>
    /// <param name="disposing">If true called by Dispose, if false called by destructor.</param>
    private void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.HttpClient.CancelPendingRequests();
                this.HttpClient.Dispose();
                this.WebApplicationFactory.Dispose();
            }

            this.disposedValue = true;
        }
    }
}
