using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace NaukriEditor
{
    public partial class ScriptTemplates
    {
        [MenuItem("Assets/Create/N# Script", false, 79)]
        public static void CreateNaukriBehaviourTemplate()
        {
            ScriptFactory.Create("NaukriBehaviour.cs", "NewBehaviourScript.cs");
        }
    }
}
