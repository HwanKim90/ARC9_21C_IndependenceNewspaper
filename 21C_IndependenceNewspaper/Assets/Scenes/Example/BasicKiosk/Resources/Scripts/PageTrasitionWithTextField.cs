using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arc9.Unity.KioskToolkit;
using UnityEngine.UI;

public class PageTrasitionWithTextField : PageTransitionBase
{
    [SerializeField] Text info_text;
    // Start is called before the first frame update
    void Start()
    {
        //info_text = GameObject.Find($"info_text ({})").GetComponent<Text>();
    }


    protected override void OnPreHideTransition()
    {
        base.OnPreHideTransition();

        if(info_text != null)
        {
            info_text.text = $"{gameObject.name}: OnPreHideTransition !";
        }
    }

    protected override void OnPreShowTransition()
    {
        base.OnPreShowTransition();
        if (info_text != null)
        {
            info_text.text = $"{gameObject.name}: OnPreShowTransition !";
        }
    }

    protected override void OnPostHideTransition()
    {
        base.OnPostHideTransition();
        if (info_text != null)
        {
            info_text.text = $"{gameObject.name}: OnPostHideTransition !";
        }
    }

    protected override void OnPostShowTransition()
    {
        base.OnPostShowTransition();
        if (info_text != null)
        {
            info_text.text = $"{gameObject.name}: OnPostShowTransition !";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
