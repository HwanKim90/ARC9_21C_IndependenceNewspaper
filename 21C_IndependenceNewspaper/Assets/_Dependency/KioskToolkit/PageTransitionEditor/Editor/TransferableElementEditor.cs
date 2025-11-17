using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

namespace Arc9.Unity.KioskToolkit
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TransferableElement))]
    public class TransferableElementEditor : Editor
    {
        public TransferableElement selected;

        private List<GUILayoutOption> mButtonLayout = new List<GUILayoutOption>();
        // Start is called before the first frame update
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
                selected = (TransferableElement)target;
            }

            //mGuiButtonStyle.fixedHeight = 50;
            //mGuiButtonStyle.normal.textColor = Color.blue;
            //mGuiButtonStyle.fontSize = 24;
            mButtonLayout.Add(GUILayout.Height(35));
        }

        public override void OnInspectorGUI()
        {
            if (selected == null)
            {
                return;
            }

            GUILayout.Label("이동 가능한 요소");
            GUILayout.BeginVertical();
            {
                DrawDefaultInspector();
            }
            GUILayout.EndVertical();

            EditorGUILayout.Space();

            GUILayout.Label("위치 설정");
            GUILayout.BeginHorizontal();
            {

                GUI.color = Color.white;
                if (GUILayout.Button("보이기 위치 설정", mButtonLayout.ToArray()))
                {
                    selected.SetShowRect();
                }

                if (GUILayout.Button("숨기기 위치 설정", mButtonLayout.ToArray()))
                {
                    selected.SetHideRect();
                }
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.Label("위치 바로 이동");

            GUILayout.BeginHorizontal();
            {
                GUI.color = Color.white;
                if (GUILayout.Button("보이기 위치로 이동", mButtonLayout.ToArray()))
                {
                    selected.MoveShowPosition();
                }

                if (GUILayout.Button("숨기기 위치로 이동", mButtonLayout.ToArray()))
                {
                    selected.MoveHidePosition(false);
                }
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUI.color = Color.white;
            GUILayout.Label("시뮬레이션(실행환경)");
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("보이기", mButtonLayout.ToArray()))
                {
                    Sequence s = DOTween.Sequence();
                    selected.ExecuteShowTransition(s);
                }

                if (GUILayout.Button("숨기기", mButtonLayout.ToArray()))
                {
                    Sequence s = DOTween.Sequence();
                    selected.ExecuteHideTransition(s);
                }
                GUI.color = Color.white;
            }
            GUILayout.EndHorizontal();
        }

        void OnSceneGUI()
        {
#if false
            // get the chosen game object
            TransferableElement t = selected as TransferableElement;

            if (t == null /*||  t.gameObjects == null*/)
                return;
            /*
            // grab the center of the parent
            Vector3 center = t.transform.position;

            // iterate over game objects added to the array...
            for (int i = 0; i < t.GameObjects.Length; i++)
            {
                // ... and draw a line between them
                if (t.GameObjects[i] != null)
                    Handles.DrawLine(center, t.GameObjects[i].transform.position);
            }
            */

            Handles.DrawLine(t.transform.position, t.transform.position);
#endif
        }
    }

    

}
