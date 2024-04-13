using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNativeDEV.SINUS.Core.FluentAPI.Model
{
    public enum RunStorePrintOrder : int
    {
        /// <summary>
        /// No sorting.
        /// </summary>
        Unsorted = 0,

        /// <summary>
        /// Sort RunStore data after key before printing.
        /// </summary>
        KeySorted = 1,

        /// <summary>
        /// Sort RunStore data after value before printing.
        /// </summary>
        ValueSorted = 2,
    }
}
