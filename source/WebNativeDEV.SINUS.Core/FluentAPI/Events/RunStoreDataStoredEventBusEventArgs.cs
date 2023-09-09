using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNativeDEV.SINUS.Core.Events;

namespace WebNativeDEV.SINUS.Core.FluentAPI.Events
{
    public class RunStoreDataStoredEventBusEventArgs : EventBusEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunStoreDataStoredEventBusEventArgs"/> class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isNew"></param>
        /// <param name="oldValue"></param>
        public RunStoreDataStoredEventBusEventArgs(string key, object? value, bool isNew, object? oldValue)
        {
            this.Key = key;
            this.Value = value;
            this.IsNew = isNew;
            this.OldValue = oldValue;
        }

        public string Key { get; }
        public object? Value { get; }
        public bool IsNew { get; }
        public object? OldValue { get; }
    }
}
