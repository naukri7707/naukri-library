using System;

namespace Naukri.BetterInspector.Core
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class InspectorAttribute : Attribute
    {
        public int order;
    }
}