// <copyright file="SinusWebApplicationFactory.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.Sut
{
    internal interface ISinusWebApplicationFactory : IDisposable
    {
        HttpClient? CreateClient();
    }
}