using System;

namespace Naukri.Scopes
{
    public readonly struct SetValueScope<T> : IDisposable
    {
        private readonly RefGetter<T> reference;

        private readonly T leaveValue;

        public SetValueScope(RefGetter<T> refGetter, T enterValue, T leaveValue)
        {
            reference = refGetter;
            this.leaveValue = leaveValue;
            refGetter() = enterValue;
        }

        public void Dispose()
        {
            reference() = leaveValue;
        }
    }
}
