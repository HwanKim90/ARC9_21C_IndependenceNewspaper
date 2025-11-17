using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit.Keyboard
{
    public class VirtualTextInputBox : MonoBehaviour
    {

        public Image VirtualCursor;
        public float maxCusorPosX;
        AutomateKR mAutomateKR = new AutomateKR();
        protected UnityEngine.UI.InputField mTextField = null;


        public Color cursorActive;
        public Color cursorDeactive;
        public string TextField
        {
            set
            {
                if (mTextField != null)
                {
                    mTextField.text = value;
                }
            }
            get
            {
                if (mTextField != null)
                {
                    return mTextField.text;
                }
                return "";
            }
        }

        void Start()
        {
            mTextField = GetComponent<UnityEngine.UI.InputField>();
        }

        void Update()
        {
            if (mTextField != null)
            {
                if (TextField.Length > 0)
                {
                    if (VirtualCursor != null)
                    {
                        if (!VirtualCursor.gameObject.activeSelf)
                        {
                            VirtualCursor.gameObject.SetActive(true);
                        }

                        if (Time.time % 1 > 0.5)
                        {
                            VirtualCursor.color = cursorActive;
                        }
                        else
                        {
                            VirtualCursor.color = cursorDeactive;
                        }
                        if (maxCusorPosX > mTextField.preferredWidth)
                        {
                            VirtualCursor.rectTransform.anchoredPosition = new Vector2(mTextField.preferredWidth, 0);
                        }
                    }
                }
                else
                {
                    if (VirtualCursor != null)
                    {
                        if (VirtualCursor.gameObject.activeSelf)
                        {
                            VirtualCursor.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            mAutomateKR.Clear();

            TextField = mAutomateKR.completeText + mAutomateKR.ingWord;
        }


        public void KeyDownHangul(char _key)
        {
            mAutomateKR.SetKeyCode(_key);

            TextField = mAutomateKR.completeText + mAutomateKR.ingWord;
        }

        public void KeyDown(char _key)
        {
            mAutomateKR.SetKeyString(_key);

            TextField = mAutomateKR.completeText + mAutomateKR.ingWord;
        }

        public void KeyDown(VirtualKey _key)
        {
            switch (_key.KeyType)
            {
                case VirtualKey.kType.kBackspace:
                    {
                        mAutomateKR.SetKeyCode(AutomateKR.KEY_CODE_BACKSPACE);

                    }
                    break;
                case VirtualKey.kType.kSpace:
                    {
                        mAutomateKR.SetKeyCode(AutomateKR.KEY_CODE_SPACE);
                    }
                    break;
            }

            TextField = mAutomateKR.completeText + mAutomateKR.ingWord;
        }

        public AutomateKR.HAN_STATUS GetStatus()
        {
            return mAutomateKR.GetStatus();
        }
    }

}

