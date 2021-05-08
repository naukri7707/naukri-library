using System;
using System.Collections.Generic;
using System.Text;

namespace Naukri.Extensions
{
    public static class StringMethods
    {
        public static bool Substring(this string self, string left, string right, out string substring)
        {
            substring = self.Substring(left, right);
            return substring != null;
        }

        public static string Substring(this string self, string left, string right)
        {
            var start = self.IndexOf(left, StringComparison.Ordinal) + left.Length;
            if (self.Length < start)
                return null;
            var end = self.IndexOf(right, start, StringComparison.Ordinal);
            var length = end - start;
            return length < 0 ? null : self.Substring(start, length);
        }

        public static StringBuilder Append(this StringBuilder self, params object[] values)
        {
            foreach (var value in values)
            {
                self.Append(value);
            }

            return self;
        }

        public static StringBuilder AppendEach<T>(this StringBuilder self, IEnumerable<T> values, Func<T, string> setter, string separator = null)
        {
            foreach (var value in values)
            {
                self.Append(setter(value));
                self.Append(separator ?? "");
            }

            return self;
        }
    }
}