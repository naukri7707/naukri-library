using System.Collections.Generic;

namespace Naukri.Extensions
{
    public static class DeconstructMethods
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> self, out TKey key, out TValue value)
        {
            key = self.Key;
            value = self.Value;
        }
    }
}