using UnityEngine;

namespace Naukri.Toast
{
    [System.Serializable]
    public enum ToastAnchor
    {
        LeftTop,
        CenterTop,
        RightTop,
        LeftBottom,
        CenterBottom,
        RightBottom
    }

    public static partial class ExtensionMethods
    {
        public static bool IsLeft(this ToastAnchor self)
        {
            return self.ToVector2().x == -1F;
        }

        public static bool IsRight(this ToastAnchor self)
        {
            return self.ToVector2().x == 1F;
        }

        public static bool IsTop(this ToastAnchor self)
        {
            return self.ToVector2().y == 1F;
        }

        public static bool IsBottom(this ToastAnchor self)
        {
            return self.ToVector2().y == -1F;
        }

        public static Vector2 ToVector2(this ToastAnchor self)
        {
            switch (self)
            {
                case ToastAnchor.LeftTop:
                    return new Vector2(0, 1F);
                case ToastAnchor.CenterTop:
                    return new Vector2(0.5F, 1F);
                case ToastAnchor.RightTop:
                    return new Vector2(1F, 1F);
                case ToastAnchor.LeftBottom:
                    return new Vector2(0, 0);
                case ToastAnchor.CenterBottom:
                    return new Vector2(0.5F, 0);
                case ToastAnchor.RightBottom:
                    return new Vector2(1F, 0);
                default:
                    return Vector2.zero;
            }
        }
    }
}