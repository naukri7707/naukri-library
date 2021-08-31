using System;

namespace Naukri.Scopes
{
    public readonly struct TempReplaceScope<T> : IDisposable
    {
        private readonly RefGetter<T> reference;

        private readonly T originalValue;

        public TempReplaceScope(RefGetter<T> refGetter, T replaceValue)
        {
            reference = refGetter;
            originalValue = reference();
            refGetter() = replaceValue;
        }

        public void Dispose()
        {
            reference() = originalValue;
        }
    }
}
