using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arc9.Unity.KioskToolkit.Language
{
    public class ChangeLanguageGraphic : MonoBehaviour, IChangeLanguage
    {
        private LanguageEnums current = LanguageEnums.UNKOWN;
        [SerializeField]
        CanvasGroup[] graphics;

        public void Change(LanguageEnums select)
        {
            if (current != select)
            {
                if (gameObject.activeInHierarchy)
                {
                    graphics[(int)select].DOFade(1.0f, 0.4f);
                    if (current != LanguageEnums.UNKOWN)
                    {
                        graphics[(int)current].DOFade(0.0f, 0.4f);
                    }
                }
                else
                {
                    graphics[(int)select].alpha = 1.0f;
                    if (current != LanguageEnums.UNKOWN)
                    {
                        graphics[(int)current].alpha = 0.0f;
                    }
                }
                current = select;
            }
        }

        public LanguageEnums CurrentLanguage()
        {
            return current;
        }

        void Awake()
        {
            //Transform[] children = GetComponentsInChildren<Transform>();
            Transform[] children = transform.Cast<Transform>().ToArray(); // 1단계 하위 오브젝트만 반환
            List<CanvasGroup> tempList = new List<CanvasGroup>();
            foreach (Transform child in children)
            {
                var cg = child.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    tempList.Add(cg);
                }
            }

            graphics = tempList.ToArray();

            foreach (var graphic in graphics)
            {
                graphic.alpha = 0.0f;
            }
        }
    }

}
