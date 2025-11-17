using Arc9.Unity.KioskToolkit;
using Arc9.Unity.KioskToolkit.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사운드 매니저
/// 싱글톤 형태로 사운드 재생을 담당
/// 유니티 기본 API를 기반으로 작성(특수 기능은 외부 라이브러리 사용 권장)
/// 
/// 업데이트 예정
/// 1. AudioSourceFade FadeTime 외부값으로 노출
/// 2. 디버그 창을 통한 볼륨 조절 기능 
/// 3. 사운드 기본 출력 변경하여 저장
/// 
/// Author : ychans
/// Release date : 2022-03-25
/// </summary>

namespace Arc9.Unity.KioskToolkit.Sound
{
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {
        [SerializeField]
        private SoundDataService soundDataService;

        [SerializeField]
        private bool playBgm = false;

        [SerializeField]
        private AudioClip[] bgmClips;

        [SerializeField]
        private int selectedBgm = 0;
        [SerializeField]
        private AudioClip[] audioClips;

        private Dictionary<string, AudioSourceFade> bgmSourcesDic;
        private Dictionary<string, AudioSource> audioSourcesDic;

        private AudioSource audioSourceOneShot;

        [SerializeField]
        private AudioSource[] exAudioSources;

        private bool isReady = false;
        public bool IsReady
        {
            get { return isReady; }
            private set { isReady = value; }
        }

        private bool isDebug = false;
        public bool IsDebug
        {
            get { return isDebug; }
            set
            {
                isDebug = value;
                // 디버그 모드 실행시 볼륨 조정 기능
            }
        }
        void Awake()
        {
            // SoundManager는 (isReady == true) 이후에 사용 가능
            // 사운드 매니저는 isReady 이후에 사용 가능
            // (SoundDataService == null) 일 경우 에디터에서 등록된 사운드 클립 재생
            // (SoundDataService != null) 일 경우 스트리밍어셋 폴더에서 로드된 사운드 클립 재생
            // 최초 SoundDataService 설정시 arc9의 기존 SoundSetupData.xml을 삭제해 주세요.
        }


        // Start is called before the first frame update
        IEnumerator Start()
        {
            if (soundDataService != null)
            {
                Debug.Log("SoundDataService Load Start");

                soundDataService.ResourceLoad();
                while (!soundDataService.isLoadedComplete)
                {
                    yield return null;
                };
                Debug.Log("SoundDataService Load Complete");

                bgmClips = soundDataService.LoadedBgmClips.ToArray();
                audioClips = soundDataService.LoadedAudioClips.ToArray();
            }

            audioSourceOneShot = gameObject.AddComponent<AudioSource>();

            bgmSourcesDic = new Dictionary<string, AudioSourceFade>();
            audioSourcesDic = new Dictionary<string, AudioSource>();
            // Setup Scene Object
            GameObject bgmContainer = new GameObject();
            bgmContainer.name = "BgmSourceContainer";

            GameObject audioContainer = new GameObject();
            audioContainer.name = "AudioSourceContainer";

            bgmContainer.transform.parent = transform;
            audioContainer.transform.parent = transform;

            int index = 0;

            bool isBgmLoop = SoundSetupData.GetValue("IsBgmLoop", true);
            bool isBgmAutoPlay = playBgm = SoundSetupData.GetValue("IsBgmAutoPlay", true);
            selectedBgm = SoundSetupData.GetValue("selectedBgmIndex", 0);

            foreach (var clip in bgmClips)
            {
                GameObject go = new GameObject();
                go.name = "Bgm_AudioSource_" + index.ToString();
                go.transform.parent = bgmContainer.transform;
                AudioSourceFade audioSourceFade = go.AddComponent<AudioSourceFade>();
                audioSourceFade.Clip = clip;
                if (index == selectedBgm)
                {
                    audioSourceFade.PlayOnAwake = isBgmAutoPlay;
                }
                else
                {
                    audioSourceFade.PlayOnAwake = false;
                }

                float bgmVolume = SoundSetupData.GetValue($"Bgm{index:00}_Volume", 1.0f);

                audioSourceFade.targetVolume = bgmVolume;
                audioSourceFade.Loop = isBgmLoop; // bgm은 기본 루프
                bgmSourcesDic.Add(clip.name, audioSourceFade);
                index++;
            }

            index = 0;

            foreach (var clip in audioClips)
            {
                GameObject go = new GameObject();
                go.name = "AudioSource_" + index.ToString();
                go.transform.parent = audioContainer.transform;
                AudioSource audioSource = go.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.playOnAwake = false;
                float effectVolume = SoundSetupData.GetValue($"Effect{index:00}_Volume", 1.0f);
                audioSource.volume = effectVolume;
                audioSource.loop = false;
                audioSourcesDic.Add(clip.name, audioSource);
                index++;
            }

            isReady = true;

            if (playBgm)
            {
                bgmSourcesDic[bgmClips[selectedBgm].name].Play();
            }
        }

        public void BGMPlay()
        {
            if (isReady && selectedBgm >= 0 && selectedBgm < bgmClips.Length)
            {
                bgmSourcesDic[bgmClips[selectedBgm].name].Play();
            }
        }

        public void BGMStop()
        {
            if (isReady && selectedBgm >= 0 && selectedBgm < bgmClips.Length)
            {
                bgmSourcesDic[bgmClips[selectedBgm].name].Stop();
                //selectedBgm = -1; // default
            }

        }

        public void ChangeBgm(int bgmClipIndex)
        {
            if (isReady && bgmClipIndex >= 0 && bgmClipIndex < bgmClips.Length)
            {
                if (selectedBgm == bgmClipIndex)
                {
                    Debug.Log("[Change Result] : bgm index is equal !");
                    return;
                }

                // 추후 BGM 클래스 별도 만들어서 BGM 자연스럽게 페이드 되는 기능 넣기
                BGMStop();
                selectedBgm = bgmClipIndex;
                BGMPlay();
            }
        }

        public void SoundPlay(string clipName)
        {
            if (isReady) audioSourcesDic[clipName].Play();
        }

        public void SoundPlay(int clipIndex, float delay = 0.0f)
        {
            if (isReady)
            {
                if (clipIndex < audioClips.Length && clipIndex > -1)
                {
                    if (delay != 0.0f)
                    {
                        audioSourcesDic[audioClips[clipIndex].name].PlayDelayed(delay);
                    }
                    else
                    {
                        audioSourcesDic[audioClips[clipIndex].name].Play();
                    }

                }
            }
        }


        public void SoundPlayOneShot(int clipIndex)
        {
            if (isReady) audioSourceOneShot.PlayOneShot(audioClips[clipIndex]);
        }


        public void SoundStop(int clipIndex)
        {
            if (isReady)
            {
                if (clipIndex < audioClips.Length && clipIndex > -1)
                {
                    audioSourcesDic[audioClips[clipIndex].name].Stop();
                }
            }
        }

        public AudioClip GetAudioClip(int clipIndex)
        {
            AudioClip audioClip = null;
            if (isReady)
            {
                if (clipIndex < audioClips.Length && clipIndex > -1)
                {
                    audioClip = audioClips[clipIndex];
                }
            }
            return audioClip;
        }

        /// <summary>
        /// Ex Audiosource Control
        /// </summary>
        public void ExAudioSourcePlay(int clipIndex)
        {
            if (clipIndex < exAudioSources.Length && clipIndex > -1)
            {
                if (!exAudioSources[clipIndex].isPlaying)
                {
                    {
                        exAudioSources[clipIndex].Play();
                    }
                }
            }
        }

        public void ExAudioSourceStop(int clipIndex)
        {
            if (clipIndex < exAudioSources.Length && clipIndex > -1)
            {
                exAudioSources[clipIndex].Stop();
            }
        }

        public void ExAudioSourceStopAll()
        {
            foreach (var source in exAudioSources)
            {
                source.Stop();
            }
        }

        // Update is called once per frame
        void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SoundPlay(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SoundPlay(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SoundPlay(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SoundPlay(3);
            }
            */
        }
    }

}
