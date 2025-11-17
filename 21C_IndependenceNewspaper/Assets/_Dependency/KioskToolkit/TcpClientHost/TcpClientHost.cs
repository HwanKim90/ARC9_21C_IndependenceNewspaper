using Arc9.Unity.KioskToolkit.Service;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit
{
    public class TcpClientHost : MonoBehaviour
    {
        [System.Serializable]
        public class HostEvent : UnityEvent<HostEventEnum, string> { }

        public enum HostEventEnum : uint
        {
            Unknown = 0,
            Trace,
            ConnectionOk,
            Disconnected,
            Receive
        };

        public class HostEventData
        {
            public HostEventEnum eventEnum { get; set; }
            public string eventMessage { get; set; }
        }
        

        [System.Serializable]
        public delegate bool HostEventDelegate(int code, string message);

        public string TcpServerAddress = "127.0.0.1";
        public ushort TcpServerPortNumber = 9000;
        public bool Connected = false;
        public string PacketDelimiter = "\r\n";
        public HostEvent OnHostEventHandler;
        public GameObject ConnectionIndigator;
        //public HostEventDelegate OnHostEventHandler;

        private StandardClient mStandardClient;
        private Thread mClientThread;
        private NetworkStream mConnectedNetworkStream;
        private byte[] mPacketDelimiter;
        ConcurrentQueue<HostEventData> mEventQueue = new System.Collections.Concurrent.ConcurrentQueue<HostEventData>();


        private void Awake()
        {
            TcpServerAddress = AppData.Tcp.IpAddress; 
            TcpServerPortNumber = (ushort)AppData.Tcp.PortNumber;

            string tmpPacketDelimiter = PacketDelimiter;
            tmpPacketDelimiter = tmpPacketDelimiter.Replace("\\r", "\r").Replace("\\n", "\n");

            mPacketDelimiter = Encoding.Default.GetBytes(tmpPacketDelimiter);
        }
        // Start is called before the first frame update
        void Start()
        {
            mStandardClient = new StandardClient(this.OnMessage, this.ConnectionHandler, TcpServerAddress, TcpServerPortNumber); //Uses default host and port and timeouts
            mClientThread = new Thread(this.mStandardClient.Run);

            mClientThread.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (mEventQueue.Count > 0)
            {
                if (mEventQueue.TryDequeue(out HostEventData data))
                {
                    OnHostEventHandler?.Invoke(data.eventEnum, data.eventMessage);
                }
            }

            if (ConnectionIndigator != null)
            {
                if (Connected)
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

        private void OnDestroy()
        {
            //mStandardClient.
            if (mConnectedNetworkStream != null)
            {
                mConnectedNetworkStream.Close();
            }

            if (mStandardClient != null)
            {
                mStandardClient.ExitSignal = true;
            }

            if (mClientThread != null)
            {
                mClientThread.Join();
            }
        }

        protected virtual void ConnectionHandler(NetworkStream connectedAutoDisposedNetStream)
        {
            if (!connectedAutoDisposedNetStream.CanRead && !connectedAutoDisposedNetStream.CanWrite)
            {
                return; //We need to be able to read and write
            }

            mConnectedNetworkStream = connectedAutoDisposedNetStream;

            Connected = true;
            mEventQueue.Enqueue(new HostEventData()
            {
                eventEnum = HostEventEnum.ConnectionOk,
                eventMessage = "Connection Ok"
            });

            var writer = new StreamWriter(connectedAutoDisposedNetStream) { AutoFlush = true };
            var reader = new StreamReader(connectedAutoDisposedNetStream);

            ByteBuffer BB = new ByteBuffer();
            byte[] recvBuffer = new byte[4096];

            while (!this.mStandardClient.ExitSignal) //Tight network message-loop (optional)
            {
                try
                {
                    int length = connectedAutoDisposedNetStream.Read(recvBuffer, 0, recvBuffer.Length);

                    if (length <= 0)
                    {
                        break;
                    }

                    BB.PutByteBlock(recvBuffer, length);

                    int completedPacketCount = BB.PatternSearch(mPacketDelimiter, true);
                    while (completedPacketCount >= 0)
                    {
                        byte[] completedPacket = new byte[completedPacketCount];

                        BB.PopByteBlock(ref completedPacket, completedPacketCount);

                        //Debug.Log(Encoding.Default.GetString(completedPacket));
                        mEventQueue.Enqueue(new HostEventData()
                        {
                            eventEnum = HostEventEnum.Receive,
                            eventMessage = Encoding.Default.GetString(completedPacket)
                        });

                        completedPacketCount = BB.PatternSearch(mPacketDelimiter, true);
                    }
                }
                catch (IOException ex)
                {
                    _ = ex;
                    //this.OnHostMessages.Invoke(HostEvent.Trace, ex.ToString());
                    break;
                }
            }

            Connected = false;

            mConnectedNetworkStream = null;
          
            mEventQueue.Enqueue(new HostEventData()
            {
                eventEnum = HostEventEnum.Disconnected,
                eventMessage = "Disconnected"
            });
        }

        public bool WriteMessage(string message)
        {
            byte[] msg = Encoding.Default.GetBytes(message + Encoding.Default.GetString(mPacketDelimiter));
            if (mConnectedNetworkStream != null)
            {
                try
                {
                    mConnectedNetworkStream.Write(msg, 0, msg.Length);
                    mConnectedNetworkStream.Flush();
                }
                catch (Exception ex)
                {
                    _ = ex;

                    return false;
                }

                return true;
            }

            return false;
        }

        protected virtual void OnMessage(string message)
        {
            mEventQueue.Enqueue(new HostEventData()
            {
                eventEnum = HostEventEnum.Trace,
                eventMessage = message
            });
        }
    }
}
