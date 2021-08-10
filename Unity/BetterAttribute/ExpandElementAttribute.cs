using UnityEngine;

namespace Naukri.Unity.BetterAttribute
{
    public class ExpandElementAttribute : PropertyAttribute
    {
        public ExpandElementAttribute() 
        {
            order = -9999;
        }
    }
}