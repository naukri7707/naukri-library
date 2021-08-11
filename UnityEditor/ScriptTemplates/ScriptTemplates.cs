using Naukri.UnityEditor.Factory;
using UnityEditor;

namespace Naukri.UnityEditor.ScriptTemplates
{
    public static class ScriptTemplates
    {
        [MenuItem("Assets/Create/Naukri/Behaviour", false, 79)]
        public static void CreateBehaviourTemplate()
        {
            ScriptFactory.Create("NaukriBehaviour.cs", "NewBehaviourScript.cs");
        }

        [MenuItem("Assets/Create/Naukri/ScriptableObject", false, 80)]
        public static void CreateScriptableObjectTemplate()
        {
            ScriptFactory.Create("NaukriScriptableObject.cs", "NewScriptableObjectScript.cs");
        }

        [MenuItem("Assets/Create/Naukri/Singleton/Behaviour", false, 81)]
        public static void CreateSingletonBehaviourTemplate()
        {
            ScriptFactory.Create("SingletonBehaviour.cs", "NewSingletonBehaviourScript.cs");
        }

        [MenuItem("Assets/Create/Naukri/Singleton/Resource", false, 82)]
        public static void CreateSingletonResourceTemplate()
        {
            ScriptFactory.Create("SingletonResource.cs", "NewSingletonResourceScript.cs");
        }
    }
}
