using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Naukri.Timer
{
    [Serializable]
    public struct StopwatchTimer : ITimer
    {
        [SerializeField]
        private float alertTime;

        public float AlertTime => alertTime;

        [SerializeField]
        private UnityEvent onAlert;

        public UnityEvent OnAlert => onAlert;

        private float counter;

        public float Counter => counter;

        private bool isCounting;

        public bool IsCounting => isCounting;

        public StopwatchTimer(float alertTime) : this(alertTime, new UnityEvent()) { }

        public StopwatchTimer(float alertTime, UnityEvent onAlert)
        {
            this.alertTime = alertTime;
            this.onAlert = onAlert;
            isCounting = false;
            counter = 0F;
        }

        public void Play()
        {
            isCounting = true;
        }

        public void Pause()
        {
            isCounting = false;
        }

        public void Reset()
        {
            counter = 0;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Restart()
        {
            Reset();
            Play();
        }

        public void Tick()
        {
            counter += Time.deltaTime;
            if (counter >= alertTime)
            {
                OnAlert.Invoke();
                Stop();
            }
        }
    }
}
