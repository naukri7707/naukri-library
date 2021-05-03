using UnityEngine;

namespace Naukri.Unity.BetterAttribute
{
    public class ElementNameAttribute : PropertyAttribute
    {
        public readonly string name;

        public ElementNameAttribute(string name) => this.name = name;
    }
}