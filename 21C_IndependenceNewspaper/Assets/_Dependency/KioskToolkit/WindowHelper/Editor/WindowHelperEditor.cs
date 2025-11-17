using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arc9.Unity.KioskToolkit.Service;
using UnityEditor;

namespace Arc9.Unity.KioskToolkit
{
    [CustomEditor(typeof(WindowsHelper))]
    public class WindowHelperEditor : Editor
    {
        public WindowsHelper selected;

        private void OnEnable()
        {
            if (target == null)
            {
                return;
            }

            if (AssetDatabase.Contains(target))
            { 
                selected = null;
            }
            else
            {
                selected = (WindowsHelper)target;

                
            }
        }

        
        public override void OnInspectorGUI()
        {
            if (selected == null)
            {
                return;
            }

            selected.TopMost = EditorGUILayout.Toggle("항상 위", selected.TopMost);
            selected.EnableSetWindowPos = EditorGUILayout.Toggle("윈도우 창 위치 조절", selected.EnableSetWindowPos);

            selected.WindowStartupPosition = EditorGUILayout.Vector2Field("윈도우 시작 위치", selected.WindowStartupPosition);
            selected.WindowSize = EditorGUILayout.Vector2Field("윈도우 창 크기", selected.WindowSize);

            if (GUILayout.Button("설정 저장"))
            {
                if (selected != null)
                {
                    AppData.Window.TopMost = selected.TopMost;
                    AppData.Window.Left = selected.WindowStartupPosition.x;
                    AppData.Window.Top = selected.WindowStartupPosition.y;
                    AppData.Window.Width = selected.WindowSize.x;
                    AppData.Window.Height = selected.WindowSize.y;

                    AppData.SaveInstance();
                }
            }
        }
    }
}

