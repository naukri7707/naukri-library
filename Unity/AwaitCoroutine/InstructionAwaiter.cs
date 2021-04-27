using Naukri.AwaitCoroutine;
using UnityEngine;

namespace Naukri
{
    public static class InstructionAwaiter
    {
        public static YieldInstructionAwaiter GetAwaiter(this WaitForEndOfFrame instruction)
        {
            return new YieldInstructionAwaiter(instruction);
        }

        public static YieldInstructionAwaiter GetAwaiter(this WaitForFixedUpdate instruction)
        {
            return new YieldInstructionAwaiter(instruction);
        }

        public static YieldInstructionAwaiter GetAwaiter(this WaitForSeconds instruction)
        {
            return new YieldInstructionAwaiter(instruction);
        }

        public static CustomYieldInstructionAwaiter GetAwaiter(this WaitForSecondsRealtime instruction)
        {
            return new CustomYieldInstructionAwaiter(instruction);
        }

        public static CustomYieldInstructionAwaiter GetAwaiter(this WaitUntil instruction)
        {
            return new CustomYieldInstructionAwaiter(instruction);
        }

        public static CustomYieldInstructionAwaiter GetAwaiter(this WaitWhile instruction)
        {
            return new CustomYieldInstructionAwaiter(instruction);
        }
    }
}