using Naukri.UnityEditor.Factory;
using UnityEditor;

namespace Naukri.UnityEditor.ScriptTemplates
{
    public static class ScriptTemplates
    {
        [MenuItem("Assets/Create/N# Script", false, 79)]
        public static void CreateNaukriBehaviourTemplate()
        {
            ScriptFactory.Create("NaukriBehaviour.cs", "NewBehaviourScript.cs");
        }
    }
}
