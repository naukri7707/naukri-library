using UnityEngine.Events;

namespace Naukri.Timer
{
    public interface ITimer
    {
        public float AlertTime { get; }

        public UnityEvent OnAlert { get; }

        public float Counter { get; }

        public bool IsCounting { get; }

        public void Play();

        public void Pause();

        public void Stop();

        public void Reset();

        public void Restart();

        public void Tick();
    }
}
