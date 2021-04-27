using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

namespace Naukri.AwaitCoroutine
{
    public struct CustomYieldInstructionAwaiter : INotifyCompletion
    {
        public bool IsCompleted { get; private set; }

        private readonly CustomYieldInstruction instruction;

        private Action continuation;

        public CustomYieldInstructionAwaiter(CustomYieldInstruction instruction)
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