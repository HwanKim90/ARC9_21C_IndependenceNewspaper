using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit.DebugView
{
    public class DebugViewControl : MonoBehaviour
    {
        public bool bView = false;

        [SerializeField]
        private GameObject debugPanel;
        [SerializeField]
        private Text stateText;
        [SerializeField]
        private Text netStateText;
        [SerializeField]
        private Text comPortText;
        [SerializeField]
        private Text baudrateText;
        [SerializeField]
        private Text serialMsgText;
        [SerializeField]
        private Text tcpMsgText;

        private TextQueue serialMsgQueue;
        private TextQueue tcpMsgQueue;

        private void Awake()
        {
            serialMsgQueue = new TextQueue(serialMsgText);
            tcpMsgQueue = new TextQueue(tcpMsgText);
        }

        private void SetView(bool value)
        {
            Debug.Log($"Set Debug: {value}");
            bView = value;
            debugPanel.SetActive(value);
            serialMsgQueue.UpdateText();
            tcpMsgQueue.UpdateText();
        }

        public void ChangeStateText(string state)
        {
            stateText.text = state;
        }

        public void ChangeNetStateText(bool state)
        {
            netStateText.text = (state)?"Connect":"Disconnect";
        }

        public void ChangeComPortText(string port)
        {
            comPortText.text = port;
        }

        public void ChangeBaudrateText(string baudrate)
        {
            baudrateText.text = baudrate.ToString();
        }

        public void AddSerialMessage(string message)
        {
            serialMsgQueue.AddLine(message + ", 시간: " + DateTime.Now.ToString("HH:mm:ss"));
        }

        // 시리얼2 생기면 여기에다가
        public void AddTCPMessage(string message)
        {
            tcpMsgQueue.AddLine("센서2: " + message + ", 시간: " + DateTime.Now.ToString("HH:mm:ss"));
        }
    }
}

