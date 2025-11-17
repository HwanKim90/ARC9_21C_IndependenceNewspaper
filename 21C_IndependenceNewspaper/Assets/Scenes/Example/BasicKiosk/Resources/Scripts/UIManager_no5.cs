using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_no5 : UIManagerBase
{
    // Start is called before the first frame update
    void Start()
    {
        currentUIDepth = -1;
        foreach(var element in pageDic)
        {
            element.Value.MoveHidePositionWithFade();
        }
        SetUIDepth(0);
    }

    public override void SetUIDepth(int selectDepth)
    {
        //base.SetUIDepth(selectDepth);

        switch(selectDepth)
        {
            case 0:
                {
                    pageDic["0"].ExecuteShowTransition();
                    if(currentUIDepth != -1)
                        pageDic[currentUIDepth.ToString()].ExecuteHideTransition();
                    break;
                }
            case 1:
                {                    
                    pageDic["1"].ExecuteShowTransition();
                    pageDic[currentUIDepth.ToString()].ExecuteHideTransition();
                    break;
                }
            case 2:
                {                    
                    pageDic["2"].ExecuteShowTransition();
                    pageDic[currentUIDepth.ToString()].ExecuteHideTransition();
                    break;
                }
            case 3:
                {                    
                    pageDic["3"].ExecuteShowTransition();
                    pageDic[currentUIDepth.ToString()].ExecuteHideTransition();
                    break;
                }
            case 4:
                {                    
                    pageDic["4"].ExecuteShowTransition();
                    pageDic[currentUIDepth.ToString()].ExecuteHideTransition();
                    break;
                }
        }
        currentUIDepth = selectDepth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
