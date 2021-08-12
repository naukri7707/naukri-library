using System;

namespace NaukriEditor.BetterInspector.Core
{
    public class CustomInspectorDrawer : Attribute
    {
        public Type targetType;

        public bool useForChildren;

        public CustomInspectorDrawer(Type targetType) : this(targetType, false) { }

        public CustomInspectorDrawer(Type targetType, bool useForChildren)
        {
            this.targetType = targetType;
            this.useForChildren = useForChildren;
        }
    }
}