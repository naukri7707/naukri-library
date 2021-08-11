using System;

namespace Naukri.Extensions
{
    public static class TypeMethods
    {
        public static bool IsSubclassOfRawGeneric(this Type self, Type generic)
        {
            while (self != null && self != typeof(object))
            {
                if(self.IsGenericType && generic == self.GetGenericTypeDefinition())
                {
                    return true;
                }
                self = self.BaseType;
            }
            return false;
        }
    }
}