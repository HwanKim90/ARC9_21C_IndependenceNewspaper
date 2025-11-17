using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Arc9.Unity.KioskToolkit
{
    public class PagePopup : MonoBehaviour
    {
        [Header("팝업 목록")]
        public SerializableDictionary<string, RectTransform> Popups;
        public float Duration = 0.3f;
        public AudioSource EffectAudioSource;
 
        private bool mNowTransition = false;
        private CanvasGroup mCanvasGroup;
        private string mCurrentPopupName = "";
        private Dictionary<string, Vector2> mBasePosition = new Dictionary<string, Vector2>();
        private Vector2 mHidePosOffset = new Vector2(0, -30);
        void Awake()
        {
            GetCanvasGroup();
            mBasePosition.Clear();
            foreach(var p in Popups)
            {
                mBasePosition.Add(p.Key, p.Value.anchoredPosition);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            mCurrentPopupName = "";
            foreach (var p in Popups)
            {
                //p.Value.anchoredPosition = mShowPos[p.Key] + mHidePosOffset;
                p.Value.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private CanvasGroup GetCanvasGroup()
        {
            if(mCanvasGroup == null)
            {
                mCanvasGroup = GetComponent<CanvasGroup>();
            }

            return mCanvasGroup;


        }

        public bool IsShowingPopup()
        {
            return mCurrentPopupName != "";
        }

        public void ShowPopupWithTransition(string name, Action post = null)
        {
            if(!gameObject.activeSelf)
            {
                GetCanvasGroup().alpha = 0;
                gameObject.SetActive(true);
            }

            if (mCurrentPopupName == name)
            {
                return;
            }

            if (!Popups.ContainsKey(name))
            {
                return;
            }

            if (!mNowTransition)
            {
                if (string.IsNullOrEmpty(mCurrentPopupName.Trim()))
                {
                    
                    mNowTransition = true;
                    mCurrentPopupName = name;
                    RectTransform popup = Popups[name];
                    popup.gameObject.SetActive(true);

                    Vector2 pos = mBasePosition[name];

                    popup.anchoredPosition = pos + mHidePosOffset;
                    CanvasGroup cg = popup.GetComponent<CanvasGroup>();
                    cg.alpha = 0;

                    PlayAudioClip();

                    Sequence s = DOTween.Sequence();
                    s.Append(mCanvasGroup.DOFade(1, Duration))
                    .Join(cg.DOFade(1, Duration))
                    .Join(popup.DOAnchorPos(pos, Duration)).OnComplete(()=>
                    {
                        mNowTransition = false;
                        post?.Invoke();

                    });
                }
                else
                {
                    HideOnlyPopup(() =>
                    {
                        mNowTransition = true;
                        mCurrentPopupName = name;
                        RectTransform popup = Popups[name];
                        popup.gameObject.SetActive(true);

                        Vector2 pos = mBasePosition[name];

                        popup.anchoredPosition = pos + mHidePosOffset;
                        CanvasGroup cg = popup.GetComponent<CanvasGroup>();
                        cg.alpha = 0;

                        PlayAudioClip();

                        Sequence s = DOTween.Sequence();
                        s.Append(cg.DOFade(1, Duration))
                        .Join(popup.DOAnchorPos(pos, Duration)).OnComplete(() =>
                        {
                            mNowTransition = false;
                            post?.Invoke();

                        });
                    });
                }
            }
        }

        private void HideOnlyPopup(Action post = null)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (!mNowTransition)
            {
                mNowTransition = true;

                Sequence s = DOTween.Sequence();

                foreach (var p in Popups)
                {
                    if (p.Value.gameObject.activeSelf)
                    {
                        CanvasGroup cg = p.Value.GetComponent<CanvasGroup>();
                        Vector2 showPos = mBasePosition[p.Key];
                        if (cg != null)
                        {
                            s.Join(cg.DOFade(0, Duration));
                        }
                        s.Join(p.Value.DOAnchorPos(showPos + mHidePosOffset, Duration))
                        .OnComplete(() =>
                        {
                            p.Value.gameObject.SetActive(false);
                        });
                    }
                }

                s.OnComplete(() =>
                {
                    mNowTransition = false;
                    mCurrentPopupName = "";
                    post?.Invoke();
                });
            }
        }
        public void HidePopupWithTransition(float dealy = 0, Action post = null)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (!mNowTransition)
            {
                mNowTransition = true;

                Sequence s = DOTween.Sequence();

                s.Append(mCanvasGroup.DOFade(0, Duration));

                foreach(var p in Popups)
                {
                   if(p.Value.gameObject.activeSelf)
                    {
                        CanvasGroup cg = p.Value.GetComponent<CanvasGroup>();
                        Vector2 showPos = mBasePosition[p.Key];
                        if (cg != null)
                        {
                            s.Join(cg.DOFade(0, Duration));
                        }
                        s.Join(p.Value.DOAnchorPos(showPos + mHidePosOffset, Duration))
                        .OnComplete(() =>
                        {
                            p.Value.gameObject.SetActive(false);
                        });
                    }
                }

                if (dealy > 0)
                {
                    s.SetDelay(dealy);
                }

                s.OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    mNowTransition = false;
                    mCurrentPopupName = "";

                    post?.Invoke();

                    EffectAudioSource = null;
                });
            }
        }

        public void ForceHide()
        {
            gameObject.SetActive(false);
            mNowTransition = false;
        }

        private void PlayAudioClip()
        {
            if(EffectAudioSource != null)
            {
                EffectAudioSource.time = 0;
                EffectAudioSource.Play();
            }
        }
    }
}