using Naukri.BetterAttribute;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Naukri
{
    public class EventInvoker : MonoBehaviour
    {
        public bool runtimeHotKey;

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
            if (runtimeHotKey || !Application.isPlaying)
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
}
