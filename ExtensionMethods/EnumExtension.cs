using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Naukri
{
    public static partial class ExtensionMethods
    {
        public static bool IndexExist(this Array self, int index)
        {
            return (index >= 0) & (index < self.Length);
        }

        public static T AddFlag<T>(this T self, T addFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<int>.From(self) | CastTo<int>.From(addFlag));
        }

        public static T RemoveFlag<T>(this T self, T removeFlag) where T : Enum
        {
            return CastTo<T>.From(CastTo<int>.From(self) & ~CastTo<int>.From(removeFlag));
        }

        public static T SetFlag<T>(this T self, bool state, T targetFlag) where T : Enum
        {
            if (state)
                return self.AddFlag(targetFlag);
            else
                return self.RemoveFlag(targetFlag);
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
                self = self.SetFlag(state, flag);
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
    }
}
