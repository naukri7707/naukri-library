using System;
using UnityEngine;

namespace Naukri.Unity.BetterAttribute
{
    public class PropertyUsageAttribute : PropertyAttribute
    {
        public readonly Type type;

        public readonly bool allowSceneObject = false;

        public PropertyUsageAttribute(Type type, bool allowSceneObject = false)
        {
            this.type = type;
            this.allowSceneObject = allowSceneObject;
        }
    }
}