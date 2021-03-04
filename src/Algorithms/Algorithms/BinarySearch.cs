using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public static class BinarySearch
    {
        /// <summary>
        /// Get the same value in ascending ordered datum, return index.
        /// If the value is not in the datum, return -1.
        /// </summary>
        /// <param name="datum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetSameValue(List<int> datum, int value)
        {
            var startIdx = 0;
            var endIdx = datum.Count - 1;
            while (true)
            {
                if (startIdx > endIdx) return -1;

                var midIdx = startIdx + ((endIdx - startIdx) >> 1); // Shift operation
                if (datum[midIdx] == value)
                {
                    return midIdx;
                }
                
                if(datum[midIdx] > value)
                {
                    endIdx = midIdx - 1;
                    continue;
                }

                startIdx = midIdx + 1;
            }
        }

    }
}
