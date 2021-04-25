using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return GetAllTypes().Where(it => baseType.IsAssignableFrom(it)).ToArray();
        }

        public static Type[] GetAllTypes()
        {
            List<Type> types = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes());
            }
            return types.ToArray();
        }
    }
}
