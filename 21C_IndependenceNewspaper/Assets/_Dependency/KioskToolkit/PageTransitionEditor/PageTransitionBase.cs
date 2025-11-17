using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Arc9.Unity.KioskToolkit
{

    public class PageTransitionBase : MonoBehaviour
    {
        //static List<PageTransitionBase> Transitions = new List<PageTransitionBase>();
        //Start is called before the first frame update
        /*
        public enum TransitionMethod : uint
        {
            Unknown = 0,
            FadeInOut,
            ElementMove,
            ElementMoveWithFadeInOut,
        };
        */
        [Header("Base Class")]
        public List<TransferableElement> TransferableElementList = new List<TransferableElement>();
        public bool SelfDisableAfterHide = true;
        public bool EnableAlphaTransition = true;
        public float PageTransitionDuration = 1f;

        protected CanvasGroup mCanvasGroup;
        private bool mNowPageTransitionProgress = false;
        private Sequence mSequence = null;
        protected float mPageEnableTime = 0;
        public PageTransitionBase()
        {
            //Transitions.Add(this);
        }

        virtual protected void OnEnable()
        {
            mPageEnableTime = Time.time;
        }

        virtual protected void OnPreShowTransition() { }
        virtual protected void OnPreHideTransition() { }

        virtual protected void OnPostShowTransition() { }
        virtual protected void OnPostHideTransition() { }


        public bool IsPageTransitionProgress()
        {
            return mNowPageTransitionProgress;
        }

        protected CanvasGroup GetCanvasGroup()
        {
            if(mCanvasGroup == null)
            {
                mCanvasGroup = GetComponent<CanvasGroup>();
            }

            return mCanvasGroup;
        }

        [ExecuteInEditMode]
        public void GetTransferableElement()
        {
            TransferableElementList.Clear();
            foreach (Transform child in transform)
            {
                var transferable = child.GetComponent<TransferableElement>();
                if (transferable != null)
                {
                    TransferableElementList.Add(transferable);
                }
            }
        }

        [ExecuteInEditMode]
        public void SetShowRect()
        {
            foreach (var el in TransferableElementList)
            {
                el.SetShowRect();
            }
        }


        [ExecuteInEditMode]
        public void SetHideRect()
        {
            foreach (var el in TransferableElementList)
            {
                el.SetHideRect();
            }
        }

        //Editor 환경에서 실행 되는 함수이다
        [ExecuteInEditMode]
        public void MoveShowPosition()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            if (EnableAlphaTransition)
            {
                if (GetCanvasGroup() != null)
                {
                    mCanvasGroup.alpha = 1;
                }
            }

            foreach (var el in TransferableElementList)
            {
                el.MoveShowPosition();
            }
        }

        //Editor 환경에서 실행 되는 함수이다
        //Fade 처리나 Enable/Disable처리는 하지 않는다.
        [ExecuteInEditMode]
        public void MoveHidePosition(bool bWithPageFade = true)
        {
            if (EnableAlphaTransition && bWithPageFade)
            {
                if (GetCanvasGroup() != null)
                {
                    mCanvasGroup.alpha = 0;
                }
            }

            foreach (var el in TransferableElementList)
            {
                el.MoveHidePosition(bWithPageFade);
            }
        }

        public void MoveHidePositionWithFade()
        {
            MoveHidePosition(true);
            if (SelfDisableAfterHide)
            {
                gameObject.SetActive(false);
            }
        }

        public bool IsAllElementTransitionProgressDone()
        {
            foreach (var el in TransferableElementList)
            {
                if(el.IsTransitionProgress())
                {
                    return false;
                }
            }
            return true;
        }

        public void ExecuteShowTransition()
        {
            ExecuteShowTransition(null);
        }

        virtual public void ExecuteShowTransition(Action act = null)
        {
            OnPreShowTransition();

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            if (mSequence != null)
            {
                mSequence.Kill();
            }

            mSequence = DOTween.Sequence();

            mNowPageTransitionProgress = true;


            if (EnableAlphaTransition)
            {
                if (GetCanvasGroup() != null)
                {
                    mSequence.Prepend(mCanvasGroup.DOFade(1, PageTransitionDuration));
                }
            }

            foreach (var el in TransferableElementList)
            {
                if (el != null)
                {
                    el.ExecuteShowTransition(mSequence);
                }
            }

            mSequence.OnComplete(() =>
            {
                Debug.Log("<color=green>Show Transition Done(" + gameObject.name + ")</color>");
                mNowPageTransitionProgress = false;
                act?.Invoke();
                OnPostShowTransition();
            });
        }

        public void ExecuteHideTransition()
        {
            ExecuteHideTransition(null);
        }

        virtual public void ExecuteHideTransition(Action act = null)
        {
            OnPreHideTransition();

            mNowPageTransitionProgress = true;

            if(mSequence != null)
            {
                mSequence.Kill();
            }

            mSequence = DOTween.Sequence();

            if (EnableAlphaTransition)
            {
                if (GetCanvasGroup() != null)
                {
                    mSequence.Prepend(mCanvasGroup.DOFade(0, PageTransitionDuration));
                }
            }

            foreach (var el in TransferableElementList)
            {
                if (el != null)
                {
                    el.ExecuteHideTransition(mSequence);
                }
            }

            mSequence.OnComplete(() =>
            {
                Debug.Log("<color=green>Hide Transition Done(" + gameObject.name + ")</color>");
                mNowPageTransitionProgress = false;
                act?.Invoke();
                OnPostHideTransition();

                if (SelfDisableAfterHide)
                {
                    gameObject.SetActive(false);
                }
            });
        }

        public void SetElementAlpha(bool enable, int select = -1)
        {
            if (select == -1)
            {
                foreach (var element in TransferableElementList)
                {
                    element.EnableAlphaTransition = enable;
                }
            }
            else
            {
                TransferableElementList[select].EnableAlphaTransition = enable;
            }
        }
    }
}
