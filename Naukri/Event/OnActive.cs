using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Naukri.Event
{
    public class OnActive : MonoBehaviour
    {
        public UnityEvent onEnable;
        public UnityEvent onDisable;

        protected virtual void OnEnable() => onEnable.Invoke();
        protected virtual void OnDisable() => onDisable.Invoke();
    }
}