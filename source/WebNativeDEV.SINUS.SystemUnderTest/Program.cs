// <copyright file="Program.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest;

using System.Diagnostics.CodeAnalysis;
using WebNativeDEV.SINUS.SystemUnderTest.Services;
using WebNativeDEV.SINUS.SystemUnderTest.Services.Abstractions;
using WebNativeDEV.SINUS.SystemUnderTest.Services.Mock;

/// <summary>
/// This class is used to bootstrap the webapi project.
/// </summary>
public partial class Program
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    protected Program()
    {
    }

    /// <summary>
    /// Gets a value indicating whether the service runs as test-double.
    /// </summary>
    public static bool ShouldMock { get; private set; }

    /// <summary>
    /// Entry point into the application.
    /// </summary>
    /// <param name="args">OS arguments.</param>
    [ExcludeFromCodeCoverage(Justification = "framework bootstrap code, no logic included")]
    private static void Main(string[] args)
    {
        args ??= Array.Empty<string>();

        Console.WriteLine("    +-----------------------------");
        Console.WriteLine("    | Start: ");
        Console.WriteLine("    |      Args: ");
        args.ToList().ForEach(x => Console.WriteLine($"    |          * {x}"));
        ShouldMock = args.Contains("--ExecutionMode=Mock");
        Console.WriteLine("    |      Mocking: " + (ShouldMock ? "activated" : "deactivated"));
        Console.WriteLine("    +-----------------------------");

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        if (ShouldMock)
        {
            builder.Services.AddSingleton<ITimeProvider, MockTimeProvider>();
        }
        else
        {
            builder.Services.AddSingleton<ITimeProvider, TimeProvider>();
        }

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        Console.WriteLine("    +-----------------------------");
        Console.WriteLine("    | Shutdown: ");
        Console.WriteLine("    |      Args: ");
        args.ToList().ForEach(x => Console.WriteLine($"    |          * {x}"));
        ShouldMock = args.Contains("--ExecutionMode=Mock");
        Console.WriteLine("    |      Mocking: " + (ShouldMock ? "activated" : "deactivated"));
        Console.WriteLine("    +-----------------------------");
    }
}