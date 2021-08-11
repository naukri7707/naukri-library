using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class ForkNameAttribute : PropertyAttribute
    {
        public readonly string fieldName;
        
        public readonly object value;
        
        public readonly string trueForkName;
        
        public readonly string falseForkName;

        public ForkNameAttribute(string fieldName, object value, string trueForkName, string falseForkName)
        {
            this.fieldName = fieldName;
            this.value = value;
            this.trueForkName = trueForkName;
            this.falseForkName = falseForkName;
        }
    }
}