using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Utils
{
    public static class StringExtensions
    {
        public static string GetStringBetween(this string source, string keyStart, string keyEnd)
        {
            var startIndex = source.IndexOf(keyStart, StringComparison.Ordinal) + keyStart.Length;
            var endIndex = source.IndexOf(keyEnd, startIndex, StringComparison.Ordinal) - startIndex;
            return source.Substring(startIndex, endIndex);
        }

        public static string GetSafeStringBetween(this string source, string keyStart, string keyEnd, string defaultOut = null)
            => !source.Contains(keyStart)
               || !source.Contains(keyEnd)
               || keyStart.Length > source.Length
               || keyEnd.Length > source.Length
                ? defaultOut
                : GetStringBetween(source, keyStart, keyEnd);
    }
}
