// <copyright file="SinusWebApplicationFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Sut;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebNativeDEV.SINUS.Core.UITesting;
using WebNativeDEV.SINUS.MsTest;

/// <summary>
/// Spawns a publicly available application that can be tested via Selenium.
/// </summary>
/// <typeparam name="TEntryPoint">Generic pointing to the class that initializes the system under test.</typeparam>
/// <remarks>
///     - https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
///     - https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
///     - https://stackoverflow.com/questions/71541576/using-webapplicationfactory-to-do-e2e-testing-with-net-6-minimal-api
///     - https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests
///
/// ... these links helped me to find a solution to spawn a WebApplication (and not only in-memory).
/// </remarks>
#pragma warning disable CA1812 // remove not instanciated internal class; but this one is created via reflection.
internal sealed class SinusWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>, ISinusWebApplicationFactory
    where TEntryPoint : class
{
    private readonly string id;
    private readonly string[] args;
    private IHost? customHost;

    /// <summary>
    /// Initializes a new instance of the <see cref="SinusWebApplicationFactory{TEntryPoint}"/> class.
    /// </summary>
    /// <param name="hostUrl">The endpoint of the system under test.</param>
    /// <param name="id">Id of the test (test name).</param>
    /// <param name="args">Arguments handed over to the application.</param>
    public SinusWebApplicationFactory(string? hostUrl, string? id, string[] args)
    {
        this.HostUrl = hostUrl;
        this.InMemory = string.IsNullOrWhiteSpace(this.HostUrl);
        this.id = id ?? "<no id>";
        this.args = args;
        if (id != null)
        {
            SinusWafUsageStatisticsManager.TestsIncludingWaf.Add(id);
        }
    }

    /// <summary>
    /// Gets the endpoint of the system.
    /// </summary>
    public string? HostUrl { get; }

    /// <summary>
    /// Gets a value indicating whether the Web Application will be run in-memory or public via kestrel.
    /// </summary>
    public bool InMemory { get; }

    /// <inheritdoc/>
    public void CloseCreatedHost()
    {
        if (this.customHost != null)
        {
            this.customHost.StopAsync().GetAwaiter().GetResult();
            this.customHost.Dispose();
            this.customHost = null;
        }
    }

    /// <summary>
    /// Sets the urls to use.
    /// </summary>
    /// <param name="builder">Injected by the framework to configure the host.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (!this.InMemory && !string.IsNullOrWhiteSpace(this.HostUrl))
        {
            builder.UseUrls(this.HostUrl);
        }

        builder.UseSetting("ExecutionMode", "Mock");
        for (int i = 0; i < this.args.Length; i++)
        {
            builder.UseSetting($"arg{i}", this.args[i]);
        }
    }

    /// <summary>
    /// Creates the host. Here we add Kestrel and use the <see cref="HostUrl"/> to make the host
    /// available from outside the process (e.g. for Selenium).
    /// </summary>
    /// <param name="builder">Injected by the framework to configure the host.</param>
    /// <returns>The configure host.</returns>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        if (this.InMemory)
        {
            this.customHost = base.CreateHost(builder);
            return this.customHost;
        }

        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var dummyHost = builder.Build();

        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        this.customHost = builder.Build();
        this.customHost.Start();
        return dummyHost;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        this.CloseCreatedHost();
        if (!SinusWafUsageStatisticsManager.TestsDisposingWaf.Contains(this.id))
        {
            SinusWafUsageStatisticsManager.TestsDisposingWaf.Add(this.id);
        }
    }
}
