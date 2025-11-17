
using Arc9.Unity.KioskToolkit;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerBase : MonoBehaviour
{
    public SerializableDictionary<string, PageTransitionBase> pageDic;
    protected int currentUIDepth;

    // Start is called before the first frame update
    void Start()
    {
        SetUIDepth(0);
    }

    public virtual void SetUIDepth(int selectDepth)
    {
        
    }
}

