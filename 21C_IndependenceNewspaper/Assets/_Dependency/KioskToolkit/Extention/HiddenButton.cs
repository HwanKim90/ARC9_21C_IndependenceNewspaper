using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HiddenButton : MonoBehaviour
{
    [System.Serializable]
    public class HiddenButtonEvent : UnityEvent{ }

    public int CheckClickCount = 10;
    public HiddenButtonEvent OnHiddenButtonEventHandler;

    private Button mButton;
    float mPrevHiddenButtonClickTime = 0;
    int mPrevHiddenButtonClickCount = 0;

    private void Awake()
    {
        mButton = GetComponent<Button>();
    }
    void Start()
    {
        if(mButton != null)
        {
            mButton.onClick.AddListener(OnButtonClick);
        }
        
    }

    void OnButtonClick()
    {
        if (Time.time - mPrevHiddenButtonClickTime > 1)
        {
            mPrevHiddenButtonClickCount = 0;
        }
        else
        {
            mPrevHiddenButtonClickCount++;
        }

        if (mPrevHiddenButtonClickCount > CheckClickCount)
        {
            //gameObject.SetActive(true);
            OnHiddenButtonEventHandler?.Invoke();
        }

        mPrevHiddenButtonClickTime = Time.time;
    }

}
