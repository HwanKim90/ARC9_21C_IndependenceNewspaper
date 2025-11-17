using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Arc9.Unity.KioskToolkit.Sound;

namespace Arc9.Unity.KioskToolkit
{
    [CustomEditor(typeof(SoundDataService))]
    public class SoundDataServiceEditor : Editor
    {
        SerializedProperty bgmProp;
        SerializedProperty audioProp;
        SerializedProperty bgmClipListProp;
        SerializedProperty audioClipListProp;

        ReorderableList bgmReorderableList;
        ReorderableList audioReorderableList;

        void OnEnable()
        {
            bgmProp = serializedObject.FindProperty("bgmStreamingAssetSoundDataList");
            bgmReorderableList = new ReorderableList(serializedObject, bgmProp);
            bgmReorderableList.elementHeight = (EditorGUIUtility.singleLineHeight * 2) + 10;
            bgmReorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = bgmProp.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };

            audioProp = serializedObject.FindProperty("audioStreamingAssetSoundDataList");
            audioReorderableList = new ReorderableList(serializedObject, audioProp);
            audioReorderableList.elementHeight = (EditorGUIUtility.singleLineHeight * 2) + 10;
            audioReorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = audioProp.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };

            var defaultColor = GUI.backgroundColor;

            bgmReorderableList.drawHeaderCallback = (rect) =>
              EditorGUI.LabelField(rect, bgmProp.displayName);

            audioReorderableList.drawHeaderCallback = (rect) =>
              EditorGUI.LabelField(rect, audioProp.displayName);

            bgmClipListProp = serializedObject.FindProperty("loadedBgmClips");
            audioClipListProp = serializedObject.FindProperty("loadedAudioClips");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (bgmReorderableList != null)
            {
                bgmReorderableList.DoLayoutList();
            }
            if (audioReorderableList != null)
            {
                audioReorderableList.DoLayoutList();
            }
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.PropertyField(bgmClipListProp);
            EditorGUILayout.PropertyField(audioClipListProp);
        }
    }


    [CustomPropertyDrawer(typeof(StreamingAssetSoundData))]
    public class StreamingAssetSoundDataPD : PropertyDrawer
    {
        public bool showPosition = false;
        const string editorAssetPrefix = "Assets/StreamingAssets";
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            SerializedProperty streamingAssetProp = property.FindPropertyRelative("soundObject");
            SerializedProperty filePathProp = property.FindPropertyRelative("filePath");

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position.height = EditorGUIUtility.singleLineHeight;
                Rect pathPos = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 4, position.width, position.height);
                EditorGUI.PropertyField(position, streamingAssetProp);
                EditorGUI.PropertyField(pathPos, filePathProp);
                /*
                EditorGUI.DrawRect(new Rect(pathPos)
                {
                    y = pathPos.y + EditorGUIUtility.singleLineHeight + 3,
                    height = 0.5f
                }, Color.gray);
                */
            }

            Object streamingAsset = streamingAssetProp.objectReferenceValue;

            if (streamingAsset == null)
            {
                streamingAssetProp.objectReferenceValue = null;
                filePathProp.stringValue = "";
                return;
            }
            string assetPath = AssetDatabase.GetAssetPath(streamingAsset.GetInstanceID());
            
            if (streamingAsset == null || !SoundFileValidation(assetPath, out var audioType))
            {
                streamingAssetProp.objectReferenceValue = null;
                filePathProp.stringValue = "";
                return;
            }            

            if (assetPath.Contains(editorAssetPrefix))
            {
                if (assetPath.StartsWith(editorAssetPrefix))
                {
                    assetPath = assetPath.Substring(editorAssetPrefix.Length);
                }
                filePathProp.stringValue = assetPath;
            }
            else
            {
                streamingAssetProp.objectReferenceValue = null;
                filePathProp.stringValue = "";
                Debug.Log($"<color=cyan>스트리밍어셋의 사운드 파일을 넣어 주세요.</color>");
            }
        }

        private bool SoundFileValidation(string path, out AudioType audioType)
        {
            bool result = true;
            var absolutePath = Directory.GetCurrentDirectory() + "\\" + path.Replace('/', '\\');
            string extension = Path.GetExtension(absolutePath).ToLower();

            if (extension == ".mp3")
            {
                audioType = AudioType.MPEG;
            }
            else if (extension == ".wav")
            {
                audioType = AudioType.WAV;
            }
            else
            {
                audioType = AudioType.UNKNOWN;
                result = false;
                Debug.Log($"<color=cyan>사운드 파일을 넣어 주세요. mp3 or wav</color>");
            }

            return result;
        }
    }
}


