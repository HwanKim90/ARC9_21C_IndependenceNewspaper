using Arc9.Unity.KioskToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Manager_4 : MonoBehaviour
{
    [SerializeField] ShowHideControl showHideControl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RelayMotion()
    {
        showHideControl.ShowElementWS(0).OnComplete(()=> {
            showHideControl.ShowElementWS(1).OnComplete(() => {
                showHideControl.ShowElementWS(2).OnComplete(() => {
                    showHideControl.ShowElementWS(3).OnComplete(() => {
                        showHideControl.ShowElementWS(4).OnComplete(() => {
                            showHideControl.HideElement();
                        });
                    });
                });
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
