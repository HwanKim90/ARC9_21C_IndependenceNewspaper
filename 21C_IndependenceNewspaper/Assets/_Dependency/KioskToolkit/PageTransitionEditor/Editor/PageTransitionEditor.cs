using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arc9.Unity.KioskToolkit
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PageTransitionBase), true)]
    public class PageTransitionEditor : Editor
    {
        public PageTransitionBase selected;

        private List<GUILayoutOption> mButtonLayout = new List<GUILayoutOption>();
            
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
                selected = (PageTransitionBase)target;
            }

            mButtonLayout.Add(GUILayout.Height(35));
        }

        public override void OnInspectorGUI()
        {
            if (selected == null)
            {
                return;
            }

            GUILayout.Label("페이지 트랜지션 편집 도구");
            GUILayout.BeginVertical();
            {
                //GUILayout.BeginVertical("Box");
                //selected.DisableAfterHide = EditorGUILayout.TextField("TCP 서버 주소", selected.DisableAfterHide);

                DrawDefaultInspector();
                
                EditorGUILayout.Space();

                GUILayout.Label("편집 가능 한 요소");
                GUILayout.BeginVertical();
                if (GUILayout.Button("요소 가져 오기", mButtonLayout.ToArray()))
                {
                    selected.GetTransferableElement();
                }

                GUILayout.EndVertical();

                EditorGUILayout.Space();
                GUILayout.Label("전체 위치 설정");
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
                    GUI.color = Color.green;
                    if (GUILayout.Button("보이기", mButtonLayout.ToArray()))
                    {
                        selected.ExecuteShowTransition();
                    }

                    if (GUILayout.Button("숨기기", mButtonLayout.ToArray()))
                    {
                        selected.ExecuteHideTransition();
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();

            }
            GUILayout.EndVertical();
        }


    }

}
