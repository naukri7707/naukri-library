using UnityEngine;

namespace Naukri.Unity.AwaitCoroutine
{
    public static class InstructionAwaiter
    {
        public static YieldAwaiter<CustomYieldInstruction> GetAwaiter(this WaitForUpdate instruction)
        {
            return new YieldAwaiter<CustomYieldInstruction>(instruction);
        }

        public static YieldAwaiter<YieldInstruction> GetAwaiter(this WaitForEndOfFrame instruction)
        {
            return new YieldAwaiter<YieldInstruction>(instruction);
        }

        public static YieldAwaiter<YieldInstruction> GetAwaiter(this WaitForFixedUpdate instruction)
        {
            return new YieldAwaiter<YieldInstruction>(instruction);
        }

        public static YieldAwaiter<YieldInstruction> GetAwaiter(this WaitForSeconds instruction)
        {
            return new YieldAwaiter<YieldInstruction>(instruction);
        }

        public static YieldAwaiter<CustomYieldInstruction> GetAwaiter(this WaitForSecondsRealtime instruction)
        {
            return new YieldAwaiter<CustomYieldInstruction>(instruction);
        }

        public static YieldAwaiter<CustomYieldInstruction> GetAwaiter(this WaitUntil instruction)
        {
            return new YieldAwaiter<CustomYieldInstruction>(instruction);
        }

        public static YieldAwaiter<CustomYieldInstruction> GetAwaiter(this WaitWhile instruction)
        {
            return new YieldAwaiter<CustomYieldInstruction>(instruction);
        }
        
        public static YieldAwaiter<YieldInstruction> GetAwaiter(this AsyncOperation instruction)
        {
            return new YieldAwaiter<YieldInstruction>(instruction);
        }
    }
}