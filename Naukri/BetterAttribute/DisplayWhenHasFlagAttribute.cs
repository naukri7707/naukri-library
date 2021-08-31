using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class DisplayWhenHasFlagAttribute : PropertyAttribute
    {
        public readonly string fieldName;

        public readonly Flag flag;

        public DisplayWhenHasFlagAttribute(string fieldName, Flag flag)
        {
            order = -10000;
            this.fieldName = fieldName;
            this.flag = flag;
        }
    }
}
