using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogHandler
{
    static bool Initialized = false;
    static public bool Initialize()
    {

        if(Initialized)
        {
            return Initialized;
        }

#if UNITY_EDITOR
        string nLogConfig = Application.streamingAssetsPath + "/nlog.editor.config";
#else
	    string nLogConfig = Application.streamingAssetsPath + "/nlog.config";
#endif
        try
        {
            NLog.LogManager.LoadConfiguration(nLogConfig);

            Initialized = true;
        }
        catch(Exception e)
        {
            Debug.Log("NLog LoadConfiguration Failed");
            Debug.Log(e.ToString());

            Initialized = false;
        }

        
        return Initialized;
    }

    public static NLog.Logger GetCurrentClassLogger()
    {
        if(!Initialized)
        {
            if(!Initialize())
            {
                return null;
            }
        }

        return NLog.LogManager.GetCurrentClassLogger();
    }
}
