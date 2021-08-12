using System.Reflection;

namespace NaukriEditor.BetterInspector.Core
{
    public abstract class InspectorMethodDrawer : InspectorMemberDrawer
    {
        public MethodInfo MethodInfo => memberInfo as MethodInfo;
    }
}