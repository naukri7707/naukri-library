using UnityEngine;

namespace Naukri.AwaitCoroutine
{
    public class WaitForUpdate : CustomYieldInstruction
    {
        public override bool keepWaiting => false;
    }
}