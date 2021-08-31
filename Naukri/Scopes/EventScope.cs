using System;

namespace Naukri.Scopes
{
    public readonly struct EventScope : IDisposable
    {
        private readonly Action onLeave;

        public EventScope(Action onEnter, Action onLeave)
        {
            this.onLeave = onLeave;
            onEnter();
        }

        public void Dispose()
        {
            onLeave();
        }
    }
}
