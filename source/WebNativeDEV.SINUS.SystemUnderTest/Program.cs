// <copyright file="Program.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.SystemUnderTest;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
    /// Entry point into the application.
    /// </summary>
    /// <param name="args">OS arguments.</param>
    [ExcludeFromCodeCoverage(Justification = "framework bootstrap code, no logic included")]
    private static void Main(string[] args)
    {
        args ??= [];

        var shouldMock = args.Contains("--ExecutionMode=Mock");

        var externalArgs =
            args
                .Where(x => x.StartsWith("--arg") && x.Contains('='))
                .Select(x => x[5..])
                .Where(x =>
                {
                    return int.TryParse(x.Split("=")[0], out int result);
                })
                .Select(x => x.Split("=", 2)[1])
                .ToList();
        var shouldStartWithError = externalArgs.Contains("start-with-exception");

        Console.WriteLine("    +-----------------------------");
        var taskId = Task.CurrentId?.ToString(CultureInfo.InvariantCulture) ?? " <null>";
        Console.WriteLine($"    | Start: (TaskId: {taskId}, ThreadId: {Environment.CurrentManagedThreadId})");
        Console.WriteLine("    |      Args: ");
        args.ToList().ForEach(x => Console.WriteLine($"    |          * {x}"));
        Console.WriteLine("    |      Mocking: " + (shouldMock ? "activated" : "deactivated"));
        Console.WriteLine("    |      Start Failing: " + (shouldStartWithError ? "activated" : "deactivated"));
        Console.WriteLine("    |      ExternalArgs: ");
        externalArgs.ToList().ForEach(x => Console.WriteLine($"    |          * {x}"));
        Console.WriteLine("    +-----------------------------");

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        if (shouldMock)
        {
            builder.Services.AddSingleton<ITimeProvider, MockTimeProvider>();
        }
        else
        {
            builder.Services.AddSingleton<ITimeProvider, TimeProvider>();
        }

        // throw exception if requested
        if (shouldStartWithError)
        {
            throw new InvalidOperationException("start-with-exception request received, so throw");
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
        taskId = Task.CurrentId?.ToString(CultureInfo.InvariantCulture) ?? " <null>";
        Console.WriteLine($"    | Shutdown: (TaskId: {taskId}, ThreadId: {Environment.CurrentManagedThreadId})");
        Console.WriteLine("    |      Args: ");
        args.ToList().ForEach(x => Console.WriteLine($"    |          * {x}"));
        shouldMock = args.Contains("--ExecutionMode=Mock");
        Console.WriteLine("    |      Mocking: " + (shouldMock ? "activated" : "deactivated"));
        Console.WriteLine("    +-----------------------------");
    }
}