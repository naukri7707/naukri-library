using System.Reflection;

namespace Naukri.UnityEditor.BetterInspector.Core
{
    public abstract class InspectorMethodDrawer : InspectorMemberDrawer
    {
        public MethodInfo MethodInfo => memberInfo as MethodInfo;
    }
}