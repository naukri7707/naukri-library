using System;

namespace Naukri.Extensions
{
    public static class DelegateMethods
    {
        public static void AddUniqueListener(this Delegate self, Delegate call)
        {
            self = Delegate.RemoveAll(self, call);
            self = Delegate.Combine(self, call);
        }
    }
}