using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arc9.Unity.KioskToolkit.Service;
using UnityEditor;

namespace Arc9.Unity.KioskToolkit
{
    [CustomEditor(typeof(TcpClientHost))]
    public class TcpClientHostEditor : Editor
    {
        public TcpClientHost selected;

        private void OnEnable()
        {
            if(target == null)
            {
                return;
            }

            if (AssetDatabase.Contains(target))
            {
                selected = null;
            }
            else
            {
                selected = (TcpClientHost)target; 
            }
        }

        public override void OnInspectorGUI()
        {
            if (selected == null)
            {
                return;
            }

            

            selected.TcpServerAddress = EditorGUILayout.TextField("TCP 서버 주소", selected.TcpServerAddress);
            selected.TcpServerPortNumber = (ushort)EditorGUILayout.IntField("TCP 서버 포트 번호", selected.TcpServerPortNumber);
            selected.PacketDelimiter = EditorGUILayout.TextField("패킷 구분자", selected.PacketDelimiter);
            //selected.ConnectionIndigator = EditorGUILayout.TextField("패킷 구분자", selected.ConnectionIndigator);
            selected.ConnectionIndigator = (GameObject)EditorGUILayout.ObjectField("연결 상태 표시", selected.ConnectionIndigator, typeof(GameObject), true);
            //EditorGUILayout.ObjectField

            EditorGUILayout.LabelField("연결 상태", selected.Connected ? "연결됨" : "연결안됨");
            /*
            //Event Handlers
            SerializedProperty packetEventHandler = serializedObject.FindProperty("OnPacketEventHandler");
            EditorGUILayout.PropertyField(packetEventHandler);
            //serializedObject.ApplyModifiedProperties();

            SerializedProperty hostEventHandler = serializedObject.FindProperty("OnHostEventHandler");
            EditorGUILayout.PropertyField(hostEventHandler);
            serializedObject.ApplyModifiedProperties();
            
            */
            SerializedProperty eventHandler = serializedObject.FindProperty("OnHostEventHandler");
            EditorGUILayout.PropertyField(eventHandler);
            serializedObject.ApplyModifiedProperties();
            
            if (GUILayout.Button("설정 저장"))
            {
                if (selected != null)
                {
                    AppData.Tcp.IpAddress = selected.TcpServerAddress;
                    AppData.Tcp.PortNumber= selected.TcpServerPortNumber;

                    AppData.SaveInstance();
                }
            }


        }
    }
}
