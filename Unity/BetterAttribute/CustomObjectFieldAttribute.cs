using System;
using UnityEngine;

namespace Naukri.Unity.BetterAttribute
{
    public class CustomObjectFieldAttribute : PropertyAttribute
    {
        public readonly Type type;

        public readonly bool allowSceneObject = false;

        public CustomObjectFieldAttribute(Type type, bool allowSceneObject = false)
        {
            order = -9999;
            this.type = type;
            this.allowSceneObject = allowSceneObject;
        }
    }
}