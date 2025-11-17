using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Arc9.Unity.KioskToolkit
{
    public class TransferableElement : MonoBehaviour
    {
        // Start is called before the first frame update
        public Rect ShowRect;
        public Rect HideRect;
        public Vector2 ShowScale = Vector2.one;
        public Vector2 HideScale = Vector2.one;
        public Ease ShowEaseType = Ease.Linear;
        public Ease HideEaseType = Ease.Linear;

        public float TransitionDuration = 1.0f;
        public float ShowTransitionDelayTime;
        public float HideTransitionDelayTime;
        public bool EnableAlphaTransition = true;
        protected bool mNowTransitionProgress = false;
        protected CanvasGroup mCanvasGroup;
        protected SpriteRenderer mSPRenderer = null;


        private void Awake()
        {
            GetCanvasGroup();
            mSPRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [ExecuteInEditMode]
        public void SetShowRect()
        {
            //DefaultRect = (transform as RectTransform).rect;
            ShowRect.position = (transform as RectTransform).anchoredPosition;
            ShowRect.size = (transform as RectTransform).sizeDelta;
            ShowScale = (transform as RectTransform).localScale;
        }

        [ExecuteInEditMode]
        public void SetHideRect()
        {
            HideRect.position = (transform as RectTransform).anchoredPosition;
            HideRect.size = (transform as RectTransform).sizeDelta;
            HideScale = (transform as RectTransform).localScale;
        }

        [ExecuteInEditMode]
        public void MoveShowPosition(bool alpha = false)
        {
            RectTransform rt = (transform as RectTransform);
            rt.anchoredPosition = ShowRect.position;
            rt.sizeDelta = ShowRect.size;

            rt.localScale = ShowScale;
            if (alpha && EnableAlphaTransition)
            {
                if (GetCanvasGroup() != null)
                {
                    mCanvasGroup.alpha = 1.0f;
                }
                else
                {
                    if(mSPRenderer != null)
                    {
                        mSPRenderer.color = new Color(mSPRenderer.color.r, mSPRenderer.color.g, mSPRenderer.color.b, 1.0f);
                    }
                }
            }
        }

        [ExecuteInEditMode]
        public void MoveHidePosition(bool alpha = false)
        {
            RectTransform rt = (transform as RectTransform);
            rt.anchoredPosition = HideRect.position;
            rt.sizeDelta = HideRect.size;

            rt.localScale = HideScale;
            if (alpha && EnableAlphaTransition)
            {
                if (GetCanvasGroup() != null)
                {
                    mCanvasGroup.alpha = 0.0f;
                }
                else
                {
                    if (mSPRenderer != null)
                    {
                        mSPRenderer.color = new Color(mSPRenderer.color.r, mSPRenderer.color.g, mSPRenderer.color.b, 0.0f);
                    }
                }
            }
        }

        CanvasGroup GetCanvasGroup()
        {
            if (mCanvasGroup == null)
            {
                mCanvasGroup = GetComponent<CanvasGroup>();
            }

            return mCanvasGroup;
        }

        public bool IsTransitionProgress()
        {
            return mNowTransitionProgress;
        }
        /*
        public Sequence ExecuteShowTransition(Sequence s)
        {
            RectTransform rt = (transform as RectTransform);
            return s.Join(rt.DOAnchorPos(BaseRect.position, TransitionDuration).SetDelay(ShowTransitionDelayTime))
                .Join(rt.DOSizeDelta(BaseRect.size, TransitionDuration))
                .Join(rt.DOScale(BaseScale, TransitionDuration));
        }
        */

        public Sequence ExecuteShowTransition(Sequence s)
        {
            RectTransform rt = (transform as RectTransform);

            mNowTransitionProgress = true;

            if (EnableAlphaTransition)
            {
                if (GetCanvasGroup() != null)
                {
                    s.Insert(ShowTransitionDelayTime, mCanvasGroup.DOFade(1, TransitionDuration));
                }
                else
                {
                    if(mSPRenderer != null)
                    {
                        s.Insert(ShowTransitionDelayTime, mSPRenderer.DOFade(1, TransitionDuration));
                        
                    }
                }
            }

            return s.Insert(ShowTransitionDelayTime, rt.DOAnchorPos(ShowRect.position, TransitionDuration).SetEase(ShowEaseType))
                    .Insert(ShowTransitionDelayTime, rt.DOSizeDelta(ShowRect.size, TransitionDuration).SetEase(ShowEaseType))
                    .Insert(ShowTransitionDelayTime, rt.DOScale(ShowScale, TransitionDuration).SetEase(ShowEaseType));
        }

        public Sequence ExecuteHideTransition(Sequence s)
        {
            RectTransform rt = (transform as RectTransform);

            mNowTransitionProgress = true;

            if (EnableAlphaTransition)
            {
                if (GetCanvasGroup() != null)
                {
                    s.Insert(HideTransitionDelayTime, mCanvasGroup.DOFade(0, TransitionDuration));
                }
                else
                {
                    if (mSPRenderer != null)
                    {
                        s.Insert(ShowTransitionDelayTime, mSPRenderer.DOFade(0, TransitionDuration));
                    }
                }
            }

            return s.Insert(HideTransitionDelayTime, rt.DOAnchorPos(HideRect.position, TransitionDuration).SetEase(HideEaseType))
                .Insert(HideTransitionDelayTime, rt.DOSizeDelta(HideRect.size, TransitionDuration))
                .Insert(HideTransitionDelayTime, rt.DOScale(HideScale, TransitionDuration));
        }


        void OnDrawGizmos()
        {
            /*    
         Gizmos.color = Color.yellow;
         Gizmos.DrawSphere(transform.position, 100);


        // if (target != null)
         {

             Gizmos.color = Color.blue;
             RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)(this.transform), HideRect.position, Camera.main, out Vector3 w);
             Gizmos.DrawLine(transform.position, w);
         }*/
        }

    }
}

