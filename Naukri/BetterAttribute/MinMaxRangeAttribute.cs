using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public string limitFieldName;

        public float min;

        public float max;

        public MinMaxRangeAttribute(string limitFieldName)
        {
            this.limitFieldName = limitFieldName;
        }

        public MinMaxRangeAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}