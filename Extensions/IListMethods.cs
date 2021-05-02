using System.Collections;

namespace Naukri.Extensions
{
    public static class IListMethods
    {
        public static bool HasElement(this IList self)
        {
            return self.Count > 0;
        }
    }
}