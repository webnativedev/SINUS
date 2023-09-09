// <copyright file="RunStore.cs" company="WebNativeDEV">
// Copyright (c) Daniel Kienböck. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace WebNativeDEV.SINUS.Core.FluentAPI.Contracts
{
    public interface IRunStore
    {
        object? this[string key] { get; set; }

        object? Actual { get; set; }
        object? Sut { get; set; }

        void DisposeAllDisposables();
        RunStore InitActual();
        RunStore PrintAdditional(string key, object? value);
        RunStore PrintStore();
        T Read<T>();
        T Read<T>(string key);
        T ReadActual<T>();
        object? ReadActualObject();
        object? ReadObject(string key);
        T ReadSut<T>();
        object? ReadSutObject();
        RunStore Store(object? item);
        RunStore Store(string key, object? item);
        RunStore StoreActual(object? item);
        RunStore StoreActual<TSut>(Func<TSut, object?> calculationFunction);
        RunStore StoreSut(object? systemUnderTest);
    }
}