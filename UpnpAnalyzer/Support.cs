// ---------------------------------------------------------------------------
// <copyright file="Support.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

namespace UpnpAnalyzer
{
    using System.Collections.Generic;

    /// <summary>
    /// Support methods.
    /// </summary>
    public class Support
    {
        /// <summary>
        /// Converts a list to a comma separated list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>A string with the comma separated list values.</returns>
        public static string ListToCommaSeparatedList(IReadOnlyList<string> list)
        {
            var joined = string.Join(",", list);
            return joined;
        } // ListToCommaSeparatedList()

        /// <summary>
        /// Converts the given number to a file size value (in KB, MB, GB).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string.</returns>
        public static string ToFileSize(long value)
        {
            const long BytesInKilobyte = 1024;
            const long BytesInMegabyte = 1048576;
            const long BytesInGigabyte = 1073741824;
            const long BytesInTerabyte = 1099511627776;

            const string ByteSymbol = "B";
            const string KilobyteSymbol = "KB";
            const string MegabyteSymbol = "MB";
            const string GigabyteSymbol = "GB";
            const string TerabyteSymbol = "TB";

            float result;
            var symbol = ByteSymbol;
            if (value <= BytesInKilobyte)
            {
                return $"{value} {symbol}";
            } // if

            if ((value > BytesInKilobyte) && (value < BytesInMegabyte))
            {
                result = (float)value / BytesInKilobyte;
                symbol = KilobyteSymbol;
            }
            else if ((value > BytesInMegabyte) && (value < BytesInGigabyte))
            {
                result = (float)value / BytesInMegabyte;
                symbol = MegabyteSymbol;
            }
            else if ((value > BytesInGigabyte) && (value < BytesInTerabyte))
            {
                result = (float)value / BytesInGigabyte;
                symbol = GigabyteSymbol;
            }
            else
            {
                result = (float)value / BytesInTerabyte;
                symbol = TerabyteSymbol;
            } // if

            return $"{result:N} {symbol}";
        } // ToFileSize()
    } // Support
}
