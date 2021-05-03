using UnityEngine;

namespace Naukri.Unity.BetterAttribute
{
    public class DisplayUnityObjectFieldsAttribute : PropertyAttribute
    {
        public string name;

        public bool defaultExpanded = false;

        public bool skipScriptField = false;

        public string[] skipFieldNames = new string[0];
    }
}