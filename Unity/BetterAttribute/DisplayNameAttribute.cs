using UnityEngine;

namespace Naukri.Unity.BetterAttribute
{
    public class DisplayNameAttribute : PropertyAttribute
    {
        public readonly string name;

        public DisplayNameAttribute(string name)
        {
            order = 10000;
            this.name = name;
        }
    }
}