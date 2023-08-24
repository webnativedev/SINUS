// <copyright file="TestNamingConventionManager.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.MsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public static class TestNamingConventionManager
{
    public static string DynamicDataDisplayNameAddValueFromLastArgument(MethodInfo methodInfo, object[] data)
    {
        var originalName = methodInfo?.Name
            ?? throw new ArgumentException("methodName is null", nameof(methodInfo));
        var newName = originalName.Replace(
                "_Then",
                "WithValue" + data.LastOrDefault(string.Empty) + "_Then",
                StringComparison.InvariantCulture);
        return newName;
    }
}
