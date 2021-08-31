using Naukri.Scopes;

namespace Naukri
{
    public delegate ref T RefGetter<T>();

    public static class Scope
    {
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
