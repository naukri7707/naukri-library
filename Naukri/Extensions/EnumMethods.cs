using System;
using System.Collections.Generic;
using System.Linq;
using Naukri.Reflection;

namespace Naukri.Extensions
{
    public static class EnumMethods
    {
        public static bool NoneFlag<T>(this T self) where T : Enum
        {
            return CastTo<ulong>.From(self) == 0UL;
        }

        public static T AddFlag<T>(this T self, T addFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<ulong>.From(self) | CastTo<ulong>.From(addFlag));
        }

        public static T RemoveFlag<T>(this T self, T removeFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<ulong>.From(self) & ~CastTo<ulong>.From(removeFlag));
        }

        public static T SetFlag<T>(this T self, T targetFlag, bool targetState) where T : Enum
        {
            return targetState ? self.AddFlag(targetFlag) : self.RemoveFlag(targetFlag);
        }

        public static bool HasFlag<T>(this T self, T checkFlag) where T : Enum
        {
            return (CastTo<ulong>.From(self) & CastTo<ulong>.From(checkFlag)) != 0UL;
        }

        public static T SwitchFlag<T>(this T self, T switchFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<ulong>.From(self) ^ CastTo<ulong>.From(switchFlag));
        }

        public static T ReverseFlag<T>(this T self) where T : Enum
        {
            return CastTo<T>.From(~CastTo<ulong>.From(self));
        }

        public static T AddFlags<T>(this T self, params T[] addFlags) where T : Enum
        {
            foreach (var flag in addFlags)
            {
                self = self.AddFlag(flag);
            }

            return self;
        }

        public static T RemoveFlags<T>(this T self, params T[] removeFlags) where T : Enum
        {
            foreach (var flag in removeFlags)
            {
                self = self.RemoveFlag(flag);
            }

            return self;
        }

        public static T SetFlags<T>(this T self, bool state, params T[] targetFlags) where T : Enum
        {
            foreach (var flag in targetFlags)
            {
                self = self.SetFlag(flag, state);
            }

            return self;
        }

        public static bool HasAnyFlag<T>(this T self, params T[] checkFlags) where T : Enum
        {
            foreach (var flag in checkFlags)
            {
                if (self.HasFlag(flag))
                    return true;
            }

            return false;
        }

        public static bool HasAllFlag<T>(this T self, params T[] checkFlags) where T : Enum
        {
            foreach (var flag in checkFlags)
            {
                if (!self.HasFlag(flag))
                    return false;
            }

            return true;
        }

        public static T SwitchFlags<T>(this T self, params T[] switchFlags) where T : Enum
        {
            foreach (var flag in switchFlags)
            {
                self = self.SwitchFlag(flag);
            }

            return self;
        }

        public static IEnumerable<T> GetAllFlags<T>(this T self) where T : Enum
        {
            var flag = CastTo<ulong>.From(self);
            for (var i = 0; i < 64; i++)
            {
                var currentFlag = 1UL << i;
                if ((flag & currentFlag) != 0)
                {
                    yield return CastTo<T>.From(currentFlag);
                }
            }
        }

        public static int GetMinFlagBit<T>(this T self) where T : Enum
        {
            return GetAllFlagBits(self).FirstOrDefault(-1);
        }

        public static int GetMaxFlagBit<T>(this T self) where T : Enum
        {

            return GetAllFlagBits(self).LastOrDefault(-1);
        }

        public static IEnumerable<int> GetAllFlagBits<T>(this T self) where T : Enum
        {
            return GetAllFlagBits(self, ulong.MaxValue);
        }

        public static IEnumerable<int> GetAllFlagBits<T>(this T self, T everythingFlag) where T : Enum
        {
            return GetAllFlagBits(self, CastTo<ulong>.From(everythingFlag));
        }

        public static IEnumerable<int> GetAllFlagBits<T>(this T self, ulong everythingFlag) where T : Enum
        {
            var flag = Math.Min(
                CastTo<ulong>.From(self),
                everythingFlag
                );

            for (var i = 0; i < 64; i++)
            {
                var currentFlag = 1UL << i;
                if ((flag & currentFlag) != 0)
                {
                    yield return i;
                }
            }
        }
    }
}