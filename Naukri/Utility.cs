using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naukri
{
    public static class Utility
    {
        public static void Swap<T>(ref T lhs, ref T rhs) where T : struct
        {
            T tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }

        public static T[] GetEnumValues<T>() where T : Enum
        {
            return GetEnumValues(typeof(T)) as T[];
        }

        public static Array GetEnumValues(Type enumType)
        {
            return Enum.GetValues(enumType);
        }

        public static string[] GetEnumNames<T>() where T : Enum
        {
            return GetEnumNames(typeof(T));
        }

        public static string[] GetEnumNames(Type enumType)
        {
            return Enum.GetNames(enumType);
        }
    }
}
