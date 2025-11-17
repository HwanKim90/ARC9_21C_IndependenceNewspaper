using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arc9.Unity.KioskToolkit;


public class GameManager_BasicKiosk : MonoBehaviourSingleton<GameManager_BasicKiosk>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTcpEventHandler(TcpClientHost.HostEventEnum hostEventEnum, string message )
    {
        Debug.Log(string.Format("Host Event : {0}, Message : {1}", hostEventEnum.ToString(), message));
    }
}
