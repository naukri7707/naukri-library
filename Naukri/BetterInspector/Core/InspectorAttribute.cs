using System;
using UnityEngine;

namespace Naukri.BetterInspector.Core
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class InspectorAttribute : PropertyAttribute
    {

    }
}