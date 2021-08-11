using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Naukri.AwaitCoroutine
{
    public struct YieldAwaiter<TInstruction> : INotifyCompletion
    {
        public bool IsCompleted { get; private set; }

        private readonly TInstruction instruction;

        private Action continuation;

        public YieldAwaiter(TInstruction instruction)
        {
            IsCompleted = false;
            this.instruction = instruction;
            continuation = null;
        }

        private IEnumerator AwaitCoroutine
        {
            get
            {
                yield return instruction;
                IsCompleted = true;
                continuation?.Invoke();
            }
        }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
            AwaitCoroutineManager.StartCoroutineAwaiter(AwaitCoroutine);
        }
    }
}