// <copyright file="Actions.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Utils;

/// <summary>
/// Util class for actions.
/// </summary>
public static class Actions
{
    /// <summary>
    /// Ensures that no exception is thrown in a block.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The exception if thrown or null.</returns>
    public static Exception? Safe(Action? action)
    {
#pragma warning disable CA1031 // Keine allgemeinen Ausnahmetypen abfangen
        try
        {
            action?.Invoke();
            return null;
        }
        catch (Exception exception)
        {
            return exception;
        }
#pragma warning restore CA1031 // Keine allgemeinen Ausnahmetypen abfangen
    }

    /// <summary>
    /// Ensures that no exception is thrown in a block.
    /// </summary>
    /// <param name="actions">The actions to execute.</param>
    /// <returns>The exception list.</returns>
    public static IEnumerable<Exception> Safe(params Action?[] actions)
    {
        var exceptions = new List<Exception>();

        foreach (var action in actions ?? [])
        {
            var exception = Safe(action);
            if (exception != null)
            {
                exceptions.Add(exception);
            }
        }

        return exceptions;
    }
}
