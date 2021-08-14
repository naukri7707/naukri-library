using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class DisplayObjectFieldsAttribute : PropertyAttribute
    {
        public bool readOnlyFields;

        public bool defaultExpanded = false;

        public bool skipScriptField = false;

        public string[] skipFieldNames = new string[0];

        public DisplayObjectFieldsAttribute()
        {
            order = -9999;
        }
    }
}