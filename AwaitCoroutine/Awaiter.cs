using System;
using UnityEngine;

namespace Naukri.AwaitCoroutine
{
    public static class Awaiter
    {
        private static readonly WaitForUpdate waitForUpdate = new WaitForUpdate();

        private static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        public static WaitForUpdate WaitForUpdate()
        {
            return waitForUpdate;
        }

        public static WaitForEndOfFrame WaitForEndOfFrame()
        {
            return waitForEndOfFrame;
        }

        public static WaitForFixedUpdate WaitForFixedUpdate()
        {
            return waitForFixedUpdate;
        }

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            return new WaitForSeconds(seconds);
        }

        public static WaitForSecondsRealtime WaitForSecondsRealtime(float time)
        {
            return new WaitForSecondsRealtime(time);
        }

        public static WaitUntil WaitUntil(Func<bool> predicate)
        {
            return new WaitUntil(predicate);
        }

        public static WaitWhile WaitWhile(Func<bool> predicate)
        {
            return new WaitWhile(predicate);
        }
    }
}