#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Arc9.Unity.KioskToolkit
{
    public class KioskToolkitMenuItem
    {
        static string baseObjectName = "ARC9 Kiosk Toolkit";

        [MenuItem("ARC9 Tools/KioskToolKit/윈도우즈 헬퍼 추가")]
        private static void AddWindowSetup()
        {
            GameObject go = CreateToolkitObject("Windows Helper");

            if (go.GetComponent<WindowsHelper>() == null)
            {
                go.AddComponent<WindowsHelper>();
            }
        }

        [MenuItem("ARC9 Tools/KioskToolKit/Serial 통신 모듈 추가")]
        private static void AddSerialCommModule()
        {
            GameObject go = CreateToolkitObject("Serial Comm");

            if (go.GetComponent<SerialComm>() == null)
            {
                go.AddComponent<SerialComm>();
            }
        }

        [MenuItem("ARC9 Tools/KioskToolKit/TCP 클라이언트 모듈 추가")]
        private static void AddTcpClientModule()
        {
            GameObject go = CreateToolkitObject("TcpClient Host");

            if (go.GetComponent<TcpClientHost>() == null)
            {
                go.AddComponent<TcpClientHost>();
            }
        }


        private static GameObject CreateToolkitObject(string name)
        {
            GameObject go = GetKioskToolkit();

            Transform t = go.transform.Find(name);

            if (t == null)
            {
                GameObject tk = new GameObject(name);

                tk.transform.parent = go.transform;

                return tk;
            }

            return t.gameObject;
        }

        private static GameObject GetKioskToolkit()
        {
            GameObject go = GameObject.Find(baseObjectName);

            if(go == null)
            {
                go = new GameObject(baseObjectName);
            }

            return go;
        }
    }

}
#endif