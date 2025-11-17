#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arc9.Unity.KioskToolkit.Service;
using Arc9.Unity.KioskToolkit.DebugView;
using UnityEditor;

namespace Arc9.Unity.KioskToolkit
{
    [CustomEditor(typeof(SerialComm))]
    public class SerialObjectEditor : Editor
    {
        public SerialComm selected;

        private SerializedProperty _propPatternType;
        private SerializedProperty _propPatterns;
        private SerializedProperty _propEvent;
        private void OnEnable()
        {
            if (AssetDatabase.Contains(target))
            {
                selected = null;
            }
            else
            {
                selected = (SerialComm)target;
            }


            _propPatternType = base.serializedObject.FindProperty("PatternType");
            _propPatterns = base.serializedObject.FindProperty("Patterns");
            _propEvent = base.serializedObject.FindProperty("OnEvent");
        }

        
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            if (selected == null)
            {
                return;
            }

            this.serializedObject.Update();

            selected.PortNumber = EditorGUILayout.TextField("포트 번호", selected.PortNumber);
            selected.Baudrate = (ushort)EditorGUILayout.IntField("통신 속도", selected.Baudrate);
            selected.ConnectionIndigator = (GameObject)EditorGUILayout.ObjectField("연결 상태 표시", selected.ConnectionIndigator, typeof(GameObject), true);
            selected.debugViewControl = (DebugViewControl)EditorGUILayout.ObjectField("디버그모드", selected.debugViewControl, typeof(DebugViewControl), true);
            EditorGUILayout.LabelField("연결 상태", selected.Opened ? "연결됨" : "연결안됨");

            //GUILayout.Label("Fallback Media Hints", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_propPatternType, new GUIContent("패킷 타입"));
            EditorGUILayout.PropertyField(_propPatterns, new GUIContent("패킷 패턴"));
            EditorGUILayout.PropertyField(_propEvent, new GUIContent("패킷 수신 이벤트"));
             


            //selected.PortNumber = EditorGUILayout.TextField("COM 포트 번호", selected.PortNumber);
            //selected.Baudrate = EditorGUILayout.IntField("통신 속도", selected.Baudrate);
            //selected.onReceive = EditorGUILayout.field("Callback", selected.Baudrate);
            //EditorGUILayout.PropertyField(selected.onReceive);
            //EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onReceive"), true);



            if (GUILayout.Button("설정 저장"))
            {
                if (selected != null)
                {
                    AppData.Serial.PortNumber = selected.PortNumber;
                    AppData.Serial.Baudrate = selected.Baudrate;
                    AppData.SaveInstance();
                }
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif

