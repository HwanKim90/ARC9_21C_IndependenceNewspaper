using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace Arc9.Unity.KioskToolkit
{
    public class GameManagerBase : MonoBehaviourSingleton<GameManagerBase>
    {

        public class CommandProtocol
        {
            public string protocol { get; set; }
            public Dictionary<string, dynamic> parameters { get; set; }
        }

        public class JsonProperityLowerCase : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }

        public SerializableDictionary<string, PageTransitionBase> PageDic;
        public TcpClientHost Client;

        [SerializeField] protected string StartupPageName = "";
        [SerializeField] protected string CurrentPageName = "";
        protected NLog.Logger mNLogger;
        protected PageTransitionBase mCurrentTransitionPage = null;
        protected NLog.Logger NLogger => mNLogger;
        protected float mPrevUserInputTime = 0;

        virtual protected void Awake()
        {
            //mNLogger = LogHandler.GetCurrentClassLogger();
            //mNLogger.Info("Startup GameManager");

            mPrevUserInputTime = Time.time;
        }


        virtual public void PageNavigate(string pageName)
        {
            if (CurrentPageName == pageName)
            {
                return;
            }

            if (PageDic.ContainsKey(pageName))
            {
                foreach (var p in PageDic)
                {
                    if (p.Key == pageName)
                    {
                        p.Value.ExecuteShowTransition();
                    }
                    else
                    {
                        p.Value.ExecuteHideTransition();
                    }
                }

                mCurrentTransitionPage = PageDic[pageName];

                CurrentPageName = pageName;
            }
            else
            {
                Debug.Log("<color=yellow>[GameManagerBase] 페이지 이름을 찾을 수 없음 : " + pageName + "</color>");
            }
        }

        protected void NavigateStartupPage()
        {
            if(!string.IsNullOrEmpty(StartupPageName))
            {
                if(PageDic.ContainsKey(StartupPageName))
                {
                    PageNavigate(StartupPageName);
                }
            }
        }

        bool ExecuteShows(string[] pages)
        {
            bool bExecute = false;
            foreach (var p in PageDic)
            {
                if (pages.Where(x => x == p.Key).Count() > 0)
                {
                    p.Value.ExecuteShowTransition();
                    bExecute = true;
                }
                else
                {
                    p.Value.ExecuteHideTransition();
                }
            }
            return bExecute;
        }

        protected bool WriteTcpMessage(string msg)
        {
            bool bSuccess = false;
            //mNLogger.Info("TCP 메시지 전송");
            //mNLogger.Info(msg);
            if (Client != null)
            {
                if (Client.Connected)
                {
                    bSuccess = Client.WriteMessage(msg);
                }
                else
                {
                    //mNLogger.Info("메시지 전송 실패(접속 되어 있지 않음)");
                    Debug.Log("메시지 전송 실패(접속 되어 있지 않음)");
                }
            }
            else
            {
                //mNLogger.Info("메시지 전송 실패(클라이언트 할당 않됨)");
                Debug.Log("메시지 전송 실패(클라이언트 할당 않됨)");
            }

            return bSuccess;
        }
        
        virtual public void OnTcpClientHost(TcpClientHost.HostEventEnum e, string message)
        {
        }

        protected float _UserInputDiffTime => Time.time - mPrevUserInputTime;
        private void _ResetUserInputTime()
        {
            mPrevUserInputTime = Time.time;
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                mPrevUserInputTime = Time.time;
            }
        }
        public void OnAppQuit()
        {
            Application.Quit();

        }

        public static void ResetUserInputTime()
        {
            if(Instance!= null)
            {
                Instance._ResetUserInputTime();
            }
        }

        public static float UserInputDiffTime
        {
            get
            {
                if (Instance != null)
                {
                    return Instance._UserInputDiffTime;
                }
                return 0;
            }
        }
    }
}