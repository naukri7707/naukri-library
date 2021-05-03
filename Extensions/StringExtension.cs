using System;

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
    }
}
