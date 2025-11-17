using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arc9.Unity.KioskToolkit.Language
{
    public interface IChangeLanguage
    {
        LanguageEnums CurrentLanguage();
        void Change(LanguageEnums language);
    }

}
