using System;
using UnityEngine;
using UnityEngine.Events;

namespace Naukri
{
    public class EventInvoker : MonoBehaviour
    {
        [Serializable]
        public struct Caller
        {
            public string tag;

            public KeyCode hotKey;

            public UnityEvent TargetMethod;
        }

        public Caller[] Callers = new Caller[1];

        public void Update()
        {
            foreach (var caller in Callers)
            {
                if (Input.GetKeyDown(caller.hotKey))
                {
                    caller.TargetMethod.Invoke();
                }
            }
        }
    }
}
