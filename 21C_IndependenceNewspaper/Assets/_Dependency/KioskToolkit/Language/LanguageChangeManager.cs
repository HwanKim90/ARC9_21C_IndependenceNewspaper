using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arc9.Unity.KioskToolkit.Language
{
    public enum LanguageEnums
    {
        UNKOWN = -1,
        KOR,
        ENG
    }

    public class LanguageChangeManager : MonoBehaviour
    {
        private IChangeLanguage[] changeLanguageUnits;

        private void Awake()
        {
            changeLanguageUnits = gameObject.GetComponentsInChildren<IChangeLanguage>();
        }

        public void Change(LanguageEnums select)
        {
            foreach (var unit in changeLanguageUnits)
            {
                unit.Change(select);
            }
        }

        public void Change(int select)
        {
            foreach (var unit in changeLanguageUnits)
            {
                unit.Change((LanguageEnums)select);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Change(LanguageEnums.KOR); // default
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Change(LanguageEnums.KOR);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Change(LanguageEnums.ENG);
            }
        }
    }

}
