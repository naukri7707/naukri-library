using UnityEngine;
using UnityEngine.Events;

namespace Naukri.Event
{
    public class OnInit : MonoBehaviour
    {
        public UnityEvent onAwake;
        public UnityEvent onStart;

        protected virtual void Awake() => onAwake.Invoke();
        protected virtual void Start() => onStart.Invoke();
    }
}
