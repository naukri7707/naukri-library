using System;
using System.Collections;

namespace Naukri.Extensions
{
    public static class IListMethods
    {
        public static bool IndexExist(this Array self, int index)
        {
            return (index >= 0) & (index < self.Length);
        }
        
        public static bool HasElement(this IList self)
        {
            return self.Count > 0;
        }
    }
}