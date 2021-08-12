using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public ReadOnlyAttribute()
        {
            order = 10000;
        }
    }
}