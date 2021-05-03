using System;
using UnityEngine;

namespace Naukri.Unity
{
    public enum InterpolationType
    {
        Linear,

        //
        InSine,
        OutSine,
        InOutSine,

        //
        InQuad,
        OutQuad,
        InOutQuad,

        //
        InCubic,
        OutCubic,
        InOutCubic,

        //
        InQuart,
        OutQuart,
        InOutQuart,

        //
        InQuint,
        OutQuint,
        InOutQuint,

        //
        InExpo,
        OutExpo,
        InOutExpo,

        //
        InCirc,
        OutCirc,
        InOutCirc,

        //
        InBack,
        OutBack,
        InOutBack,

        //
        InElastic,
        OutElastic,
        InOutElastic,

        //
        InBounce,
        OutBounce,
        InOutBounce
    }

    public static class Interpolation
    {
        public static float EPS = float.Epsilon;

        public static float ByMethod(float start, float end, float ratio, InterpolationType interpolationType)
        {
            return interpolationType switch
            {
                InterpolationType.Linear => Linear(start, end, ratio),
                InterpolationType.InSine => InSine(start, end, ratio),
                InterpolationType.OutSine => OutSine(start, end, ratio),
                InterpolationType.InOutSine => InOutSine(start, end, ratio),
                InterpolationType.InQuad => InQuad(start, end, ratio),
                InterpolationType.OutQuad => OutQuad(start, end, ratio),
                InterpolationType.InOutQuad => InOutQuad(start, end, ratio),
                InterpolationType.InCubic => InCubic(start, end, ratio),
                InterpolationType.OutCubic => OutCubic(start, end, ratio),
                InterpolationType.InOutCubic => InOutCubic(start, end, ratio),
                InterpolationType.InQuart => InQuart(start, end, ratio),
                InterpolationType.OutQuart => OutQuart(start, end, ratio),
                InterpolationType.InOutQuart => InOutQuart(start, end, ratio),
                InterpolationType.InQuint => InQuint(start, end, ratio),
                InterpolationType.OutQuint => OutQuint(start, end, ratio),
                InterpolationType.InOutQuint => InOutQuint(start, end, ratio),
                InterpolationType.InExpo => InExpo(start, end, ratio),
                InterpolationType.OutExpo => OutExpo(start, end, ratio),
                InterpolationType.InOutExpo => InOutExpo(start, end, ratio),
                InterpolationType.InCirc => InCirc(start, end, ratio),
                InterpolationType.OutCirc => OutCirc(start, end, ratio),
                InterpolationType.InOutCirc => InOutCirc(start, end, ratio),
                InterpolationType.InBack => InBack(start, end, ratio),
                InterpolationType.OutBack => OutBack(start, end, ratio),
                InterpolationType.InOutBack => InOutBack(start, end, ratio),
                InterpolationType.InElastic => InElastic(start, end, ratio),
                InterpolationType.OutElastic => OutElastic(start, end, ratio),
                InterpolationType.InOutElastic => InOutElastic(start, end, ratio),
                InterpolationType.InBounce => InBounce(start, end, ratio),
                InterpolationType.OutBounce => OutBounce(start, end, ratio),
                InterpolationType.InOutBounce => InOutBounce(start, end, ratio),
                _ => 0F
            };
        }

        public static float Linear(float start, float end, float ratio)
        {
            return Mathf.Lerp(start, end, ratio);
        }

        public static float InSine(float start, float end, float ratio)
        {
            end -= start;
            return -end * Mathf.Cos(ratio * (Mathf.PI * 0.5F)) + end + start;
        }

        public static float OutSine(float start, float end, float ratio)
        {
            end -= start;
            return end * Mathf.Sin(ratio * (Mathf.PI * 0.5F)) + start;
        }

        public static float InOutSine(float start, float end, float ratio)
        {
            end -= start;
            return -end * 0.5F * (Mathf.Cos(Mathf.PI * ratio) - 1) + start;
        }

        public static float InQuad(float start, float end, float ratio)
        {
            end -= start;
            return end * ratio * ratio + start;
        }

        public static float OutQuad(float start, float end, float ratio)
        {
            end -= start;
            return -end * ratio * (ratio - 2) + start;
        }

        public static float InOutQuad(float start, float end, float ratio)
        {
            ratio /= .5F;
            end -= start;
            if (ratio < 1)
                return end * 0.5F * ratio * ratio + start;
            ratio--;
            return -end * 0.5F * (ratio * (ratio - 2) - 1) + start;
        }

        public static float InCubic(float start, float end, float ratio)
        {
            end -= start;
            return end * ratio * ratio * ratio + start;
        }

        public static float OutCubic(float start, float end, float ratio)
        {
            ratio--;
            end -= start;
            return end * (ratio * ratio * ratio + 1) + start;
        }

        public static float InOutCubic(float start, float end, float ratio)
        {
            ratio /= .5F;
            end -= start;
            if (ratio < 1)
                return end * 0.5F * ratio * ratio * ratio + start;
            ratio -= 2;
            return end * 0.5F * (ratio * ratio * ratio + 2) + start;
        }

        public static float InQuart(float start, float end, float ratio)
        {
            end -= start;
            return end * ratio * ratio * ratio * ratio + start;
        }

        public static float OutQuart(float start, float end, float ratio)
        {
            ratio--;
            end -= start;
            return -end * (ratio * ratio * ratio * ratio - 1) + start;
        }

        public static float InOutQuart(float start, float end, float ratio)
        {
            ratio /= .5F;
            end -= start;
            if (ratio < 1)
                return end * 0.5F * ratio * ratio * ratio * ratio + start;
            ratio -= 2;
            return -end * 0.5F * (ratio * ratio * ratio * ratio - 2) + start;
        }

        public static float InQuint(float start, float end, float ratio)
        {
            end -= start;
            return end * ratio * ratio * ratio * ratio * ratio + start;
        }

        public static float OutQuint(float start, float end, float ratio)
        {
            ratio--;
            end -= start;
            return end * (ratio * ratio * ratio * ratio * ratio + 1) + start;
        }

        public static float InOutQuint(float start, float end, float ratio)
        {
            ratio /= .5F;
            end -= start;
            if (ratio < 1)
                return end * 0.5F * ratio * ratio * ratio * ratio * ratio + start;
            ratio -= 2;
            return end * 0.5F * (ratio * ratio * ratio * ratio * ratio + 2) + start;
        }

        public static float InExpo(float start, float end, float ratio)
        {
            end -= start;
            return end * Mathf.Pow(2, 10 * (ratio - 1)) + start;
        }

        public static float OutExpo(float start, float end, float ratio)
        {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * ratio) + 1) + start;
        }

        public static float InOutExpo(float start, float end, float ratio)
        {
            ratio /= .5F;
            end -= start;
            if (ratio < 1)
                return end * 0.5F * Mathf.Pow(2, 10 * (ratio - 1)) + start;
            ratio--;
            return end * 0.5F * (-Mathf.Pow(2, -10 * ratio) + 2) + start;
        }

        public static float InCirc(float start, float end, float ratio)
        {
            end -= start;
            return -end * (Mathf.Sqrt(1 - ratio * ratio) - 1) + start;
        }

        public static float OutCirc(float start, float end, float ratio)
        {
            ratio--;
            end -= start;
            return end * Mathf.Sqrt(1 - ratio * ratio) + start;
        }

        public static float InOutCirc(float start, float end, float ratio)
        {
            ratio /= .5F;
            end -= start;
            if (ratio < 1)
                return -end * 0.5F * (Mathf.Sqrt(1 - ratio * ratio) - 1) + start;
            ratio -= 2;
            return end * 0.5F * (Mathf.Sqrt(1 - ratio * ratio) + 1) + start;
        }

        public static float InBack(float start, float end, float ratio)
        {
            end -= start;
            ratio /= 1;
            float s = 1.70158F;
            return end * (ratio) * ratio * ((s + 1) * ratio - s) + start;
        }

        public static float OutBack(float start, float end, float ratio)
        {
            float s = 1.70158F;
            end -= start;
            ratio = (ratio) - 1;
            return end * ((ratio) * ratio * ((s + 1) * ratio + s) + 1) + start;
        }

        public static float InOutBack(float start, float end, float ratio)
        {
            float s = 1.70158F;
            end -= start;
            ratio /= .5F;
            if ((ratio) < 1)
            {
                s *= (1.525F);
                return end * 0.5F * (ratio * ratio * (((s) + 1) * ratio - s)) + start;
            }

            ratio -= 2;
            s *= (1.525F);
            return end * 0.5F * ((ratio) * ratio * (((s) + 1) * ratio + s) + 2) + start;
        }

        public static float InElastic(float start, float end, float ratio)
        {
            const float d = 1F;
            const float p = d * .3F;
            float s;
            var a = 0F;

            if (ratio == 0F)
                return start;

            end -= start;

            if (Math.Abs((ratio /= d) - 1) < EPS)
                return start + end;

            if (a == 0F || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (ratio -= 1)) * Mathf.Sin((ratio * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float OutElastic(float start, float end, float ratio)
        {
            const float d = 1F;
            const float p = d * .3F;
            float s;
            var a = 0F;

            if (ratio == 0F)
                return start;

            end -= start;

            if (Math.Abs((ratio /= d) - 1) < EPS)
                return start + end;

            if (a == 0F || a < Mathf.Abs(end))
            {
                a = end;
                s = p * 0.25F;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * ratio) * Mathf.Sin((ratio * d - s) * (2 * Mathf.PI) / p) + end + start);
        }

        public static float InOutElastic(float start, float end, float ratio)
        {
            const float d = 1F;
            const float p = d * .3F;
            float s;
            var a = 0F;

            if (ratio == 0F)
                return start;

            end -= start;
            
            if (Math.Abs((ratio /= d * 0.5F) - 2) < EPS)
                return start + end;

            if (a == 0F || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (ratio < 1)
                return -0.5F * (a * Mathf.Pow(2, 10 * (ratio -= 1)) * Mathf.Sin((ratio * d - s) * (2 * Mathf.PI) / p)) +
                       start;
            return a * Mathf.Pow(2, -10 * (ratio -= 1)) * Mathf.Sin((ratio * d - s) * (2 * Mathf.PI) / p) * 0.5F + end +
                   start;
        }

        public static float InBounce(float start, float end, float ratio)
        {
            const float d = 1F;

            end -= start;

            return end - OutBounce(0, end, d - ratio) + start;
        }

        public static float OutBounce(float start, float end, float ratio)
        {
            ratio /= 1F;
            end -= start;
            if (ratio < (1 / 2.75F))
            {
                return end * (7.5625F * ratio * ratio) + start;
            }
            else if (ratio < (2 / 2.75F))
            {
                ratio -= (1.5F / 2.75F);
                return end * (7.5625F * (ratio) * ratio + .75F) + start;
            }
            else if (ratio < (2.5 / 2.75))
            {
                ratio -= (2.25F / 2.75F);
                return end * (7.5625F * (ratio) * ratio + .9375F) + start;
            }
            else
            {
                ratio -= (2.625F / 2.75F);
                return end * (7.5625F * (ratio) * ratio + .984375F) + start;
            }
        }

        public static float InOutBounce(float start, float end, float ratio)
        {
            end -= start;
            float d = 1F;
            if (ratio < d * 0.5F)
                return InBounce(0, end, ratio * 2) * 0.5F + start;
            else
                return OutBounce(0, end, ratio * 2 - d) * 0.5F + end * 0.5F + start;
        }
    }
}