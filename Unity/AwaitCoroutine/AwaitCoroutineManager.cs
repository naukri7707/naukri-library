using Naukri;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Naukri.AwaitCoroutine
{
    public class AwaitCoroutineManager : SingletonBehaviour<AwaitCoroutineManager>
    {
        public static SynchronizationContext UnitySynchronizationContext { get; private set; }

        public static int UnityThreadId { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnInitial()
        {
            UnitySynchronizationContext = SynchronizationContext.Current;
            UnityThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public static void StartCoroutineAwaiter(IEnumerator routine)
        {
            DoOnUnityThread(() =>
            {
                Instance.StartCoroutine(routine);
            });
        }

        public static void StopCoroutineAwaiter(IEnumerator routine)
        {
            DoOnUnityThread(() =>
            {
                Instance.StopCoroutine(routine);
            });
        }

        public static void DoOnUnityThread(Action action)
        {
            if (SynchronizationContext.Current == UnitySynchronizationContext)
            {
                action();
            }
            else
            {
                UnitySynchronizationContext.Post(state => action(), null);
            }
        }
    }
}