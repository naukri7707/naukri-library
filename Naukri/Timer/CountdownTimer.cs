using System;
using UnityEngine;
using UnityEngine.Events;

namespace Naukri.Timer
{
    [Serializable]
    public struct CountdownTimer : ITimer
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

        public CountdownTimer(float alertTime) : this(alertTime, new UnityEvent()) { }

        public CountdownTimer(float alertTime, UnityEvent onAlert)
        {
            this.alertTime = alertTime;
            this.onAlert = onAlert;
            isCounting = false;
            counter = alertTime;
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
            counter = alertTime;
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
            counter -= Time.deltaTime;
            if (counter <= 0F)
            {
                OnAlert.Invoke();
                Stop();
            }
        }
    }
}
