using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class ElementNameAttribute : PropertyAttribute
    {
        public readonly string elementName;

        public ElementNameAttribute(string elementName)
        {
            order = 10000;
            this.elementName = elementName;
        }
    }
}