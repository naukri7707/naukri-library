using UnityEngine;

namespace Naukri.Unity.AwaitCoroutine
{
    public class WaitForUpdate : CustomYieldInstruction
    {
        public override bool keepWaiting => false;
    }
}