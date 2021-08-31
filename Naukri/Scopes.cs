using Naukri.Scopes;
using System;

namespace Naukri
{
    public delegate ref T RefGetter<T>();

    public static class Scope
    {
        public static EventScope Event(Action onEnter, Action onLeave)
        {
            return new EventScope(onEnter, onLeave);
        }

        public static SetValueScope<T> SetValue<T>(RefGetter<T> refGetter, T enterValue, T leaveValue)
        {
            return new SetValueScope<T>(refGetter, enterValue, leaveValue);
        }

        public static TempReplaceScope<T> TempReplace<T>(RefGetter<T> refGetter, T replaceValue)
        {
            return new TempReplaceScope<T>(refGetter, replaceValue);
        }
    }
}
