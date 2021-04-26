using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Naukri
{
    public static partial class ExtensionMethods
    {
        public static bool Substring(this string self, string left, string right, out string substring)
        {
            substring = self.Substring(left, right);
            return substring != null;
        }

        public static string Substring(this string self, string left, string right)
        {
            var start = self.IndexOf(left) + left.Length;
            if (self.Length < start)
                return null;
            var end = self.IndexOf(right, start);
            var length = end - start;
            return length < 0 ? null : self.Substring(start, length);
        }
    }
}
