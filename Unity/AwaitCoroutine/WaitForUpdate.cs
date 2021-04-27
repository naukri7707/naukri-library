using UnityEngine;

namespace Naukri
{
    public class WaitForUpdate : CustomYieldInstruction
    {
        public override bool keepWaiting => false;
    }
}