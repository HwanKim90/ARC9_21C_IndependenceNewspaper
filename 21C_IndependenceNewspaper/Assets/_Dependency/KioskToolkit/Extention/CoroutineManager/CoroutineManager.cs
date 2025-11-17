using System;
using System.Collections;
using UnityEngine;

namespace Arc9.Unity.KioskToolkit
{
    public class CoroutineManager : Singleton<CoroutineManager>
    {
        MonoBehaviour m_mono;
        override protected void init()
        {
            m_mono = GameObject.Find("GameManager").GetComponent<GameManagerBase>();
        }

        public void _CallWaitForOneFrame(Action act)
        {
            m_mono.StartCoroutine(DoCallWaitForOneFrame(act));

        }

        public void _CallWaitForSeconds(float seconds, Action act)
        {
            m_mono.StartCoroutine(DoCallWaitForSeconds(seconds, act));
        }

        private IEnumerator DoCallWaitForOneFrame(Action act)
        {
            yield return 0;

            act();
        }

        private IEnumerator DoCallWaitForSeconds(float seconds, Action act)
        {
            yield return new WaitForSeconds(seconds);

            act();
        }

        static public void CallWaitForOneFrame(Action act)
        {
            Instance._CallWaitForOneFrame(act);
        }

        static public void CallWaitForSeconds(float seconds, Action act)
        {
            Instance._CallWaitForSeconds(seconds, act);
        }

    }

}