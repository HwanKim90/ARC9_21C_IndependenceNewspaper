using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit
{
    public class ShowHideControl : MonoBehaviour
    {
        public bool autoElementSet = true;

        public TransferableElement[] elementList;
        public Ease ShowEaseType = Ease.InOutQuad;
        public Ease HideEaseType = Ease.InOutQuad;
        public float TransitionDuration = 1.0f;

        // 방식 및 버튼의 뎁스에 따라서 작동 안 할수 있음
        public bool isAdjustRaycastTarget = true;

        protected Sequence[] sequenceList;

        // Start is called before the first frame update
        private void Awake()
        {
            if (autoElementSet) elementList = GetComponentsInChildren<TransferableElement>();

            foreach (var element in elementList)
            {
                element.ShowEaseType = this.ShowEaseType;
                element.HideEaseType = this.HideEaseType;
                element.TransitionDuration = this.TransitionDuration;
            }
            sequenceList = new Sequence[elementList.Length];
        }

        // stop 기능 추가

        private void Start()
        {
            MoveHidePose(true);
        }

        public void MoveShowPose(bool alpha)
        {
            foreach (var element in elementList)
            {
                element.MoveShowPosition(alpha);
            }
        }

        public void MoveHidePose(bool alpha)
        {
            foreach (var element in elementList)
            {
                element.MoveHidePosition(alpha);
            }
        }

        public void ShowElement(int select = -1)
        {
            if (select != -1) // 단일
            {
                ShowElementUnit(elementList[select], ref sequenceList[select]);
            }
            else // 전체
            {
                int length = elementList.Length;
                for (int i = 0; i < elementList.Length; i++)
                {
                    ShowElementUnit(elementList[i], ref sequenceList[i]);
                }
            }
        }

        public void HideElement(int select = -1)
        {

            if (select != -1) // 단일
            {
                HideElementUnit(elementList[select], ref sequenceList[select]);
            }
            else // 전체
            {
                int length = elementList.Length;
                for (int i=0; i< length; i++)
                {
                    HideElementUnit(elementList[i], ref sequenceList[i]);
                }
            }
        }

        /// <summary>
        /// 시퀀스를 리턴 받을수 있음
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public Sequence ShowElementWS(int select = -1)
        {
            Sequence result = null;

            if (select != -1) // 단일
            {
                ShowElementUnit(elementList[select], ref sequenceList[select]);
                result = sequenceList[select];
            }
            else // 전체
            {
                int length = elementList.Length;
                for (int i = 0; i < elementList.Length; i++)
                {
                    ShowElementUnit(elementList[i], ref sequenceList[i]);
                }
                result = sequenceList[length - 1]; // 마지막 Sequence
            }
            return result;
        }

        /// <summary>
        /// 시퀀스를 리턴 받을수 있음
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public Sequence HideElementWS(int select = -1)
        {
            Sequence result = null;

            if (select != -1) // 단일
            {
                HideElementUnit(elementList[select], ref sequenceList[select]);
                result = sequenceList[select];
            }
            else // 전체
            {
                int length = elementList.Length;
                for (int i = 0; i < length; i++)
                {
                    HideElementUnit(elementList[i], ref sequenceList[i]);
                }
                result = sequenceList[length - 1]; // 마지막 Sequence
            }

            return result;
        }

        private void ShowElementUnit(TransferableElement element, ref Sequence sequence)
        {
            if (sequence != null)
            {
                sequence.Kill();
            }
            sequence = DOTween.Sequence();
            if (!element.gameObject.activeSelf) element.gameObject.SetActive(true);
            element.ExecuteShowTransition(sequence).OnComplete(() => {
                if(isAdjustRaycastTarget)
                {
                    Image img = element.GetComponent<Image>();
                    if (img != null)
                    {
                        element.GetComponent<Image>().raycastTarget = true;
                    }
                }
            });
        }

        private void HideElementUnit(TransferableElement element, ref Sequence sequence)
        {
            if (sequence != null)
            {
                sequence.Kill();
            }
            sequence = DOTween.Sequence();            
            if (!element.gameObject.activeSelf) element.gameObject.SetActive(true);
            if(isAdjustRaycastTarget)
            {
                Image img = element.GetComponent<Image>();
                if (img != null)
                {
                    element.GetComponent<Image>().raycastTarget = false;
                }
            }

            element.ExecuteHideTransition(sequence).OnComplete(() => {
                element.gameObject.SetActive(false);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}