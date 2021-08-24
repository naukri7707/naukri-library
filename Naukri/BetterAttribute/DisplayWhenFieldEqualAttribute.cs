using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class DisplayWhenFieldEqualAttribute : PropertyAttribute
    {
        public readonly string fieldName;

        public readonly object[] values;

        public DisplayWhenFieldEqualAttribute(string fieldName, params object[] values)
        {
            order = -10000;
            this.fieldName = fieldName;
            this.values = values;
        }
    }
}
