using Naukri.BetterAttribute;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Naukri.Toast
{
    [Serializable]
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class Toast : MonoBehaviour
    {
        [NonSerialized]
        public ToastState state;

        public CanvasGroup canvasGroup;

        public RectTransform RectTransform => transform as RectTransform;

        public Text message;

        private float startStateTime;

        private float startActiveTime = float.MaxValue;

        [Header("Alpha"), DisplayName("Ease Type")]
        public InterpolationType alphaEaseType;

        [Header("Moving"), DisplayName("Ease Type")]
        public InterpolationType movingEaseType;

        [DisplayName("Position Addition")]
        public Vector2 movingPosAddition;

        [Header("FadeIn"), DisplayName("Ease Type")]
        public InterpolationType fadeInEaseType;

        [DisplayName("Velocity")]
        public Vector2 fadeInVelocity;

        [Header("FadeOut"), DisplayName("Ease Type")]
        public InterpolationType fadeOutEaseType;

        [DisplayName("Velocity")]
        public Vector2 fadeOutVelocity;

        public float Alpha { get => canvasGroup.alpha; set => canvasGroup.alpha = value; }

        public Vector2 CurrentPosition { get; set; }

        public Vector2 StartPosition { get; set; }

        public Vector2 EndPosition { get; set; }

        internal Toast CreateToast(ToastManager manager, ToastMessage message)
        {
            var res = Instantiate(this, manager.transform, true);
            var anchorV = manager.ToastAnchor.ToVector2();
            res.RectTransform.anchorMin = anchorV;
            res.RectTransform.anchorMax = anchorV;
            res.RectTransform.pivot = anchorV;
            res.message.text = message.text;
            res.message.color = message.color;
            return res;
        }

        internal void ReSetAnchor(ToastAnchor anchor)
        {
            var nextV = anchor.ToVector2();
            RectTransform.anchorMin = nextV;
            RectTransform.anchorMax = nextV;
            RectTransform.pivot = nextV;
        }

        public virtual void OnFadeInEnter(ToastManager manager)
        {
            StartPosition = -fadeInVelocity * manager.toastTransitionTime;
            EndPosition = Vector2.zero;
            FadeAlpha(0, 1, manager.toastTransitionTime);
            ResetStateTime();
        }

        public virtual bool OnFadeInStay(ToastManager manager)
        {
            var ratio = (Time.time - startStateTime) / manager.toastTransitionTime;
            CurrentPosition = new Vector2
            {
                x = Interpolation.HandleByType(StartPosition.x, EndPosition.x, ratio, fadeInEaseType),
                y = Interpolation.HandleByType(StartPosition.y, EndPosition.y, ratio, fadeInEaseType)
            };
            return ratio > 1;
        }

        public virtual void OnFadeInExit(ToastManager manager)
        {
        }

        public virtual void OnDrawEnter(ToastManager manager)
        {
            startActiveTime = Time.time;
        }

        public virtual bool OnDrawStay(ToastManager manager)
        {
            var ratio = (Time.time - startStateTime) / manager.toastTransitionTime;
            if (ratio > 1F)
                ratio = 1F;
            CurrentPosition = new Vector2
            {
                x = Interpolation.HandleByType(StartPosition.x, EndPosition.x, ratio, movingEaseType),
                y = Interpolation.HandleByType(StartPosition.y, EndPosition.y, ratio, movingEaseType)
            };
            return startActiveTime + manager.toastDuration < Time.time;
        }

        public virtual void OnDrawExit(ToastManager manager)
        {
        }

        public virtual void OnFadeOutEnter(ToastManager manager)
        {
            StartPosition = CurrentPosition;
            EndPosition += fadeOutVelocity * manager.toastTransitionTime;
            FadeAlpha(1, 0, manager.toastTransitionTime);
            ResetStateTime();
        }

        public virtual bool OnFadeOutStay(ToastManager manager)
        {
            var ratio = (Time.time - startStateTime) / manager.toastTransitionTime;
            CurrentPosition = new Vector2
            {
                x = Interpolation.HandleByType(StartPosition.x, EndPosition.x, ratio, fadeOutEaseType),
                y = Interpolation.HandleByType(StartPosition.y, EndPosition.y, ratio, fadeOutEaseType)
            };
            return ratio > 1;
        }

        public virtual void OnFadeOutExit(ToastManager manager)
        {
        }

        public void FadeAlpha(float start, float end, float time)
        {
            IEnumerator fadeAlpha(float s, float e, float t)
            {
                var startTime = Time.time;
                var ratio = 0F;
                while (ratio < 1)
                {
                    ratio = (Time.time - startTime) / t;
                    Alpha = Interpolation.HandleByType(s, e, ratio, alphaEaseType);
                    yield return new WaitForEndOfFrame();
                }
                Alpha = e;
            }
            StopCoroutine(nameof(fadeAlpha));
            StartCoroutine(fadeAlpha(start, end, time));
        }

        public void UpdatePositionByAnchorAndPaddingEdge(ToastAnchor anchor, Vector2 paddingEdge)
        {
            var anchorVector = anchor.ToVector2();
            var position = CurrentPosition;
            position.x += anchorVector.x switch
            {
                0F => paddingEdge.x,
                1F => -paddingEdge.x,
                _ => 0F
            };
            position.y = anchorVector.y is 0
                ? position.y + paddingEdge.y
                : -position.y - paddingEdge.y;
            RectTransform.anchoredPosition = position;
        }

        public void ResetStateTime()
        {
            startStateTime = Time.time;
        }

        public void MakeToastExpire()
        {
            startActiveTime = float.MinValue;
        }
    }
}