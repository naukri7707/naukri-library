using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naukri.Toast
{
    [RequireComponent(typeof(Canvas))]
    public class ToastManager : MonoBehaviour
    {
        [DisplayName("Maximum Toast"), Tooltip("Number of toasts which can be displayed at a once, 0 is unlimit")]
        public int maxToastCount = 10;

        [SerializeField, Header("Style"), DisplayName("Anchor")]
        private ToastAnchor _toastAnchor = ToastAnchor.LeftBottom;

        public ToastAnchor ToastAnchor
        {
            get => _toastAnchor;
            set
            {
                _toastAnchor = value;
                ReAnchor();
            }
        }

        [DisplayName("Padding Edge")]
        public Vector2 toastPaddingEdge;

        [DisplayName("Duration"), Tooltip("Time of toast display after fadeIn and before fadeOut")]
        public float toastDuration = 5;

        [DisplayName("Transition Time"), Tooltip("Time of toast fadeIn, fadeOut and moving")]
        public float toastTransitionTime = 2;

        [DisplayUnityObjectFields(
            name = "Template",
            defaultExpanded = true,
            skipScriptField = true,
            skipFieldNames = new[] { 
                nameof(Naukri.Toast.Toast.state),
                nameof(Naukri.Toast.Toast.canvasGroup),
                nameof(Naukri.Toast.Toast.message)
            }
            )]
        public Toast toastTemplate;

        private Queue<Toast> toastQueue;

        private Queue<ToastMessage> messageQueue;

        private void Awake()
        {
            toastQueue = new Queue<Toast>();
            messageQueue = new Queue<ToastMessage>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            ReAnchor();
#endif
            var isNewToast = false;
            while (messageQueue.Count > 0 && (maxToastCount is 0 || toastQueue.Count < maxToastCount))
            {
                var msg = messageQueue.Dequeue();
                var newToast = toastTemplate.CreateToast(this, msg);
                foreach (var toast in toastQueue)
                {
                    toast.StartPosition = toast.CurrentPosition;
                    toast.EndPosition += new Vector2(0, newToast.RectTransform.rect.height);
                    toast.ResetStateTime();
                }
                toastQueue.Enqueue(newToast);
                isNewToast = true;
            }

            foreach (var toast in toastQueue)
            {
                switch (toast.state)
                {
                    case ToastState.BeforeIn:
                        toast.state = ToastState.FadeIn;
                        toast.OnFadeInEnter(this);
                        break;
                    case ToastState.FadeIn:
                        if (toast.OnFadeInStay(this) || isNewToast)
                        {
                            toast.OnFadeInExit(this);
                            toast.state = ToastState.Draw;
                            toast.OnDrawEnter(this);
                        }
                        break;
                    case ToastState.Draw:
                        if (toast.OnDrawStay(this))
                        {
                            toast.OnDrawExit(this);
                            toast.state = ToastState.FadeOut;
                            toast.OnFadeOutEnter(this);
                        }
                        break;
                    case ToastState.FadeOut:
                        if (toast.OnFadeOutStay(this))
                        {
                            toast.OnFadeOutExit(this);
                            toast.state = ToastState.AfterOut;
                        }
                        break;
                    case ToastState.AfterOut:
                        break;
                    default:
                        break;
                }
                toast.UpdatePositionByAnchorAndPaddingEdge(ToastAnchor, toastPaddingEdge);
            }
            while (toastQueue.Count > 0 && toastQueue.Peek().state == ToastState.AfterOut)
            {
                var toast = toastQueue.Dequeue();
                Destroy(toast.gameObject);
            }
        }

        public void Toast(ToastMessage message)
        {
            messageQueue.Enqueue(message);
        }

        public void ClearToast()
        {
            foreach (var toast in toastQueue)
            {
                toast.MakeToastExpire();
            }
        }

        public void ClearMessageQuene()
        {
            messageQueue.Clear();
        }

        public void ClearAll()
        {
            ClearMessageQuene();
            ClearToast();
        }

        private void ReAnchor()
        {
            foreach (var toast in toastQueue)
            {
                toast.ReSetAnchor(ToastAnchor);
            }
        }
    }
}