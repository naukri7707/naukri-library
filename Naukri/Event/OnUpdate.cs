using UnityEngine;
using UnityEngine.Events;

namespace Naukri.Event
{
    public class OnUpdate : MonoBehaviour
    {
        public UnityEvent onUpdate;
        public UnityEvent onLateUpdate;
        public UnityEvent onFixedUpdate;

        protected virtual void Update() => onUpdate.Invoke();
        protected virtual void LateUpdate() => onLateUpdate.Invoke();
        protected virtual void FixedUpdate() => onFixedUpdate.Invoke();
    }
}