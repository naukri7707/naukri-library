using System;
using UnityEngine;

namespace Naukri.BetterAttribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public ReadOnlyAttribute()
        {
            order = 10000;
        }
    }
}