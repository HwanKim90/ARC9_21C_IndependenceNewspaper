using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit.Keyboard
{
    public class KeyboardSelector : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform LetterKeyboardLayout;
        public Transform NumbericKeyboardLayout;
        public VirtualKeyboard Keyboard;

        private Text mKeyText = null;

        private void Awake()
        {
            mKeyText = transform.Find("Text").GetComponent<Text>();

        }
        void Start()
        {
            UnityEngine.UI.Button _button = gameObject.GetComponent<UnityEngine.UI.Button>();
            if (_button != null)
            {
                _button.onClick.AddListener(OnKeyboardSwitch);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (mKeyText != null)
            {
                if (LetterKeyboardLayout.gameObject.activeSelf)
                {
                    mKeyText.text = "&123";
                }
                else
                {
                    if (Keyboard.Language == VirtualKeyboard.kLanguage.kKorean)
                    {
                        mKeyText.text = "丑中之";
                    }
                    else
                    {
                        mKeyText.text = "ABC";
                    }
                }
            }
        }

        private void OnEnable()
        {
            LetterKeyboardLayout.gameObject.SetActive(true);
            NumbericKeyboardLayout.gameObject.SetActive(false);

            if (mKeyText != null)
            {
                mKeyText.text = "&123";
            }
        }

        public void OnKeyboardSwitch()
        {
            if (LetterKeyboardLayout.gameObject.activeSelf)
            {
                LetterKeyboardLayout.gameObject.SetActive(false);
                NumbericKeyboardLayout.gameObject.SetActive(true);

                if (Keyboard.Language == VirtualKeyboard.kLanguage.kKorean)
                {
                    mKeyText.text = "丑中之";
                }
                else
                {
                    mKeyText.text = "ABC";
                }
            }
            else
            {
                LetterKeyboardLayout.gameObject.SetActive(true);
                NumbericKeyboardLayout.gameObject.SetActive(false);
                mKeyText.text = "&123";
            }
        }
    }
}
