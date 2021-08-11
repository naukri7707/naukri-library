using System;

namespace Naukri.Unity.BetterInspector.Core
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class InspectorAttribute : Attribute
    {
        public int order;
    }
}