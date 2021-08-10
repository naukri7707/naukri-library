using UnityEngine;

namespace Naukri.Unity.BetterAttribute
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public ReadOnlyAttribute()
        {
            order = 10000;
        }
    }
}