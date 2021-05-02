using System;
using System.Collections.Generic;
using System.Linq;
using SysAssembly = System.Reflection.Assembly;

namespace Naukri.Reflection
{
    public static class Assembly
    {
        public static SysAssembly GetAssembly(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().ToArray())
            {
                if (assembly.GetName().Name == name)
                    return assembly;
            }

            return null;
        }

        public static Type[] GetDerivedTypesOf<T>()
        {
            return GetDerivedTypesOf(typeof(T));
        }

        public static Type[] GetDerivedTypesOf(this Type baseType)
        {
            return GetAllTypes().Where(baseType.IsAssignableFrom).ToArray();
        }

        public static IEnumerable<Type> GetAllTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    yield return type;
                }
            }
        }
    }
}