using System;
using UnityEngine;

namespace Naukri.BetterAttribute
{
    public class LongFlagAttribute : PropertyAttribute
    {
        public readonly Type enumType;

        public LongFlagAttribute(Type enumType)
        {
            this.enumType = enumType;
            order = 10000;
        }
    }
}