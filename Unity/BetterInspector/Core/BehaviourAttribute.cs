using System;

namespace Naukri.Unity.BetterInspector.Core
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class BehaviourAttribute : Attribute
    {
        public int order;
    }
}