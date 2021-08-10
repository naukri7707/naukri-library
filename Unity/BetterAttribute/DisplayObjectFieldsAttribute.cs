using UnityEngine;

namespace Naukri.Unity.BetterAttribute
{
    public class DisplayObjectFieldsAttribute : PropertyAttribute
    {
        public bool defaultExpanded = false;

        public bool skipScriptField = false;

        public string[] skipFieldNames = new string[0];

        public DisplayObjectFieldsAttribute()
        {
            order = -9999;
        }
    }
}