using System;
using UnityEngine;

namespace Naukri.Toast
{
    [Serializable]
    public enum ToastAnchor
    {
        LeftTop,
        CenterTop,
        RightTop,
        LeftBottom,
        CenterBottom,
        RightBottom
    }

    public static class ToastAnchorMethods
    {
        private const float EPS = 0.0001F;

        public static bool IsLeft(this ToastAnchor self)
        {
            return Math.Abs(self.ToVector2().x - (-1F)) < EPS;
        }

        public static bool IsRight(this ToastAnchor self)
        {
            return Math.Abs(self.ToVector2().x - 1F) < EPS;
        }

        public static bool IsTop(this ToastAnchor self)
        {
            return Math.Abs(self.ToVector2().y - 1F) < EPS;
        }

        public static bool IsBottom(this ToastAnchor self)
        {
            return Math.Abs(self.ToVector2().y - (-1F)) < EPS;
        }

        public static Vector2 ToVector2(this ToastAnchor self)
        {
            return self switch
            {
                ToastAnchor.LeftTop => new Vector2(0, 1F),
                ToastAnchor.CenterTop => new Vector2(0.5F, 1F),
                ToastAnchor.RightTop => new Vector2(1F, 1F),
                ToastAnchor.LeftBottom => new Vector2(0, 0),
                ToastAnchor.CenterBottom => new Vector2(0.5F, 0),
                ToastAnchor.RightBottom => new Vector2(1F, 0),
                _ => Vector2.zero
            };
        }
    }
}