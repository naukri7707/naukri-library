using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class DisplayWhenFieldEqualAttribute : PropertyAttribute
    {
        public readonly string fieldName;

        public readonly object value;

        public DisplayWhenFieldEqualAttribute(string fieldName, object value)
        {
            order = -10000;
            this.fieldName = fieldName;
            this.value = value;
        }
    }
}
