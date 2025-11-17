using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 사운드 데이터 로드 서비스
/// StreamingAsset에 있는 데이터만 처리한다.
/// 에디터를 통해서 편집 한다.
/// 
/// Author : ychans
/// Release date : 2022-03-25
/// </summary>

namespace Arc9.Unity.KioskToolkit.Sound
{
    public class SoundDataService : MonoBehaviour
    {
        [SerializeField]
        StreamingAssetSoundData[] bgmStreamingAssetSoundDataList;

        [SerializeField]
        StreamingAssetSoundData[] audioStreamingAssetSoundDataList;

        [SerializeField]
        private List<AudioClip> loadedBgmClips;
        public List<AudioClip> LoadedBgmClips
        {
            get { return loadedBgmClips; }
            private set
            {
                loadedBgmClips = value;
            }
        }

        [SerializeField]
        private List<AudioClip> loadedAudioClips;

        public List<AudioClip> LoadedAudioClips
        {
            get { return loadedAudioClips; }
            private set
            {
                loadedAudioClips = value;
            }
        }

        StreamingAssetSoundData[] targetDataList;
        List<AudioClip> targetClips;

        AudioSource audioSource;

        public bool isLoadedComplete = false;

        public bool ResourceLoad()
        {
            bool result = false;
            LoadedBgmClips = new List<AudioClip>();
            LoadedAudioClips = new List<AudioClip>();
            audioSource = gameObject.AddComponent<AudioSource>();

            StartCoroutine(ResourceLoadCoroutine());

            audioSource.clip = null;
            Destroy(audioSource);
            return result;
        }

        IEnumerator ResourceLoadCoroutine()
        {
            targetClips = LoadedBgmClips;
            targetDataList = bgmStreamingAssetSoundDataList;
            foreach (var data in targetDataList)
            {
                string path = data.filePath;
                Debug.Log(path);
                AudioType audioType = (path.ToLower().Contains(".mp3")) ? AudioType.MPEG : (path.ToLower().Contains(".wav")) ? AudioType.WAV : AudioType.UNKNOWN;

                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(GetFileLocation(path), audioType))
                {
                    ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                        myClip.name = Path.GetFileName(path);
                        if (targetClips != null)
                        {
                            targetClips.Add(myClip);
                            Debug.Log($"LoadComplete {path}");
                        }
                    }
                }
            }

            targetClips = LoadedAudioClips;
            targetDataList = audioStreamingAssetSoundDataList;
            foreach (var data in targetDataList)
            {
                string path = data.filePath;
                AudioType audioType = (path.ToLower().Contains(".mp3")) ? AudioType.MPEG : (path.ToLower().Contains(".wav")) ? AudioType.WAV : AudioType.UNKNOWN;

                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(GetFileLocation(path), audioType))
                {
                    ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                        myClip.name = Path.GetFileName(path);
                        if (targetClips != null)
                        {
                            targetClips.Add(myClip);
                            Debug.Log($"LoadComplete {path}");
                        }
                    }
                }
            }
            isLoadedComplete = true;
        }

        public static string GetFileLocation(string relativePath)
        {
            return "file:///" + Application.streamingAssetsPath + relativePath;
        }
    }

    [System.Serializable]
    public class StreamingAssetSoundData
    {
        public Object soundObject;
        public string filePath;

        public StreamingAssetSoundData()
        {

        }
    }
}

