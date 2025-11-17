using Arc9.Unity.KioskToolkit.Service;
using Arc9.Unity.KioskToolkit.DebugView;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Arc9.Unity.KioskToolkit
{
    public class SerialComm : MonoBehaviour
    {
        public class EventObject
        {
            public string Key { get; set; }
            public string Value { get; set; }
        
        }

        [Serializable]
        public class RecvEventHandler : UnityEvent<string, string> { }

        public enum PATTERN_TYPE
        {
            HEX,
            ASCII,
        }

        public PATTERN_TYPE PatternType;
        public SerializableDictionary<string, string> Patterns;
        public RecvEventHandler OnEvent;

        public string PortNumber = "COM1";
        public int Baudrate = 9600;
        private string sensor1Hex = "";
        private string sensor2Hex = "";

        public byte[] terminatorBytes;
        public GameObject ConnectionIndigator;
        public DebugViewControl debugViewControl;

        private ByteBuffer mBuffer;

        private SerialDevice serialDevice;

        public bool bReturn { get; private set; } = false;

        public bool Opened = false;

        private bool forceStop = false;

        private Dictionary<string, byte[]> mPatternBytes = new Dictionary<string, byte[]>();

        protected ConcurrentQueue<EventObject> m_eventBuffer = new ConcurrentQueue<EventObject>();
        protected ConcurrentQueue<EventObject> m_debugBuffer = new ConcurrentQueue<EventObject>();

        private void Awake()
        {
            PortNumber = AppData.Serial.PortNumber;
            Baudrate = (ushort)AppData.Serial.Baudrate;

            sensor1Hex = ContentData.GetValue("Sensor1_Hex", "01");
            sensor2Hex = ContentData.GetValue("Sensor2_Hex", "02");

            //foreach (var p in Patterns)
            //{
            //    mPatternBytes[p.Key] = ToByteArray(PatternType, p.Value); // p.value 외부로 빼놓기...
            //}

            mPatternBytes["1"] = ToByteArray(PatternType, sensor1Hex);
            mPatternBytes["2"] = ToByteArray(PatternType, sensor2Hex);

        }
        private void Start()
        {
            mBuffer = new ByteBuffer();

            serialDevice = new SerialDevice(PortNumber, Baudrate);

            serialDevice.OnReceive += SerialDevice_OnReceive;

            Opened = serialDevice.Open();
        }

        private void SerialDevice_OnReceive(object sender, SerialDevice.StreamEventArguments e)
        {
            ProcessData(e.Stream, e.Stream.Length);
            
            ProcessDebugViewerData(e.Stream);
        }

        private void ProcessDebugViewerData(byte[] bytes)
        {
            m_debugBuffer.Enqueue(new EventObject()
            {
                Key = "1", // 키 필요없을지도...
                Value = Util.ByteHexToHexString(bytes)
            });
        }

        //int count = 0;
        private void ProcessData(byte[] bytes, int length)
        {
            mBuffer.PutByteBlock(bytes, length);

            bool bSearchOk = false;
            do
            {
                bSearchOk = false;

                foreach(var pattern in mPatternBytes)
                {
                    if(pattern.Value != null)
                    {
                        int pos = mBuffer.PatternSearch(pattern.Value, false);
                        if (pos >= 0)
                        {
                            //count++;
                            //debugString = string.Format("#{0} : Pos - {1}", count, pos);
                            byte[] packet = new byte[pattern.Value.Length];

                            mBuffer.PopByteBlock(ref packet, pos, pattern.Value.Length);

                            m_eventBuffer.Enqueue(new EventObject()
                            {
                                Key = pattern.Key,
                                Value = Util.ByteHexToHexString(packet)
                            });

                            bSearchOk = true;

                            continue;
                        }
                    }
                }


            }
            while (bSearchOk);
        }

        private void Update()
        {
            if (m_eventBuffer.Count > 0)
            {
                if (m_eventBuffer.TryDequeue(out EventObject msgobj))
                {
                    OnEvent?.Invoke(msgobj.Key, msgobj.Value); 
                }
            }

            if (m_debugBuffer.Count > 0)
            {
                if (m_debugBuffer.TryDequeue(out EventObject msgobj))
                {
                    if (debugViewControl.gameObject.activeSelf)
                        debugViewControl.AddSerialMessage(msgobj.Value);
                }
            }

            if (ConnectionIndigator != null)
            {
                if (Opened)
                {
                    if (ConnectionIndigator.activeSelf)
                    {
                        ConnectionIndigator.SetActive(false);
                    }
                }
                else
                {
                    if (!ConnectionIndigator.activeSelf)
                    {
                        ConnectionIndigator.SetActive(true);
                    }
                }
            }
        }

        private int Search(byte[] src, byte[] pattern)
        {
            int c = src.Length - pattern.Length + 1;
            int j;
            for (int i = 0; i < c; i++)
            {
                if (src[i] != pattern[0]) continue;
                for (j = pattern.Length - 1; j >= 1 && src[i + j] == pattern[j]; j--) ;
                if (j == 0) return i;
            }
            return -1;
        }

        public byte[] ToByteArray(PATTERN_TYPE type, string cmd)
        {
            byte[] bytes = null;
            switch (type)
            {
                case PATTERN_TYPE.HEX:
                    {
                        char[] separatorChar = { ' ' };
                        string[] tokens = cmd.Split(separatorChar, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens.Length > 0)
                        {
                            bytes = new byte[tokens.Length];

                            for (int i = 0; i < tokens.Length; i++)
                            {
                                tokens[i] = tokens[i].Trim();

                                bytes[i] = Convert.ToByte(tokens[i], 16);
                            }
                        }
                    }
                    break;
                case PATTERN_TYPE.ASCII:
                    {
                        string newCmd = cmd.Replace(@"\r", "\r").Replace(@"\n", "\n");

                        if (newCmd.Length > 0)
                        {
                            bytes = Encoding.UTF8.GetBytes(newCmd);
                        }
                    }
                    break;
            }
            return bytes;
        }

        public bool WriteByte(byte[] bytes)
        {
            if (Opened)
            {
                serialDevice.Write(bytes);

                return true;
            }

            return false;
        }

        private void OnDestroy()
        {
            if (serialDevice != null)
            {
                forceStop = true;
                serialDevice.Close();
                mBuffer.Clear();
                mBuffer = null;
                m_eventBuffer = null;
            }
        }

    }

}