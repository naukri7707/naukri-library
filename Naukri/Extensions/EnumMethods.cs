using System;
using System.Collections.Generic;
using Naukri.Reflection;

namespace Naukri.Extensions
{
    public static class EnumMethods
    {
        public static T AddFlag<T>(this T self, T addFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<int>.From(self) | CastTo<int>.From(addFlag));
        }

        public static T RemoveFlag<T>(this T self, T removeFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<int>.From(self) & ~CastTo<int>.From(removeFlag));
        }

        public static T SetFlag<T>(this T self, T targetFlag, bool targetState) where T : Enum
        {
            return targetState ? self.AddFlag(targetFlag) : self.RemoveFlag(targetFlag);
        }

        public static T HasFlag<T>(this T self, T checkFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<int>.From(self) & CastTo<int>.From(checkFlag));
        }

        public static T SwitchFlag<T>(this T self, T switchFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<int>.From(self) ^ CastTo<int>.From(switchFlag));
        }

        public static T ReverseFlag<T>(this T self) where T : Enum
        {
            return CastTo<T>.From(~CastTo<int>.From(self));
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

        public static T[] GetAllFlags<T>(this T self) where T : Enum
        {
            var res = new List<T>();
            var flag = CastTo<int>.From(self);
            for (var i = 0; i < 32; i++)
            {
                var currentFlag = 1 << i;
                if ((flag & currentFlag) != 0)
                {
                    res.Add(CastTo<T>.From(currentFlag));
                }
            }

            return res.ToArray();
        }
    }
}