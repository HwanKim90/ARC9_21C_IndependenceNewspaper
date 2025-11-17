using Arc9.Unity.KioskToolkit;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIDepthEnum
{
    UNKNOWN = -1,
    IDLE_DEPTH,
    ERROR_DEPTH,
}

public class UIManager_BasicKiosk : MonoBehaviour
{
    public SerializableDictionary<string, PageTransitionBase> pageDic;
    protected UIDepthEnum currentUIDepth = UIDepthEnum.UNKNOWN;

    // Start is called before the first frame update
    void Start()
    {
        SetUIDepth(UIDepthEnum.IDLE_DEPTH);
    }

    public void SetUIDepth(int selectDepth)
    {
        SetUIDepth((UIDepthEnum)selectDepth);
    }

    public void SetUIDepth(UIDepthEnum selectDepth)
    {
        // 트랜지션 중에 입력을 막는 방식은 여러가지 자유롭게..
        if(pageDic["idle"].IsPageTransitionProgress() || pageDic["error"].IsPageTransitionProgress())
        {
            return;
        }
        switch(selectDepth)
        {
            case UIDepthEnum.IDLE_DEPTH:
                pageDic["idle"].ExecuteShowTransition();
                pageDic["error"].ExecuteHideTransition();
                break;

            case UIDepthEnum.ERROR_DEPTH:
                pageDic["idle"].ExecuteHideTransition();
                pageDic["error"].ExecuteShowTransition();
                break;
        }
        currentUIDepth = UIDepthEnum.ERROR_DEPTH;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
