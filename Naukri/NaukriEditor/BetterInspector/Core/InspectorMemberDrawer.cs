using System.Reflection;
using UnityEngine;

namespace NaukriEditor.BetterInspector.Core
{
    public abstract class InspectorMemberDrawer
    {
        private Object target;

        public Object Target { get => target; internal set => target = value; }

        internal MemberInfo memberInfo;

        public virtual void OnInit() { }

        public virtual void OnBeforeGUILayout() { }

        public virtual bool OnGUILayout() => false;

        public virtual void OnAfterGUILayout() { }
    }
}