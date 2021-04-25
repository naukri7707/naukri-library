using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Naukri
{
    public class EventInvoker : MonoBehaviour
    {
        [System.Serializable]
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
