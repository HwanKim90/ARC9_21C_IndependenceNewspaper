using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit.Language
{
    public class ChangeLanguageText : MonoBehaviour, IChangeLanguage
    {
        [SerializeField] Text textField;
        private LanguageEnums current = LanguageEnums.UNKOWN;
        public string[] messages;

        public void Change(LanguageEnums language)
        {
            if (messages != null && (int)language <= messages.Length - 1)
            {
                textField.text = messages[(int)language];
            }
        }

        public LanguageEnums CurrentLanguage()
        {
            return current;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
