using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadGameForm : MonoBehaviour
{
    public Button button1;
    public Button button2;

    private void Awake()
    {
        button1 = GameObject.Find("Button1").gameObject.GetComponent<Button>();
        button1.GetComponent<UIEventScript>().onClickEventId = EventId.OnClickButton1;
        EventManager.Instance.AddEventListener(EventId.OnClickButton1, OnClickButton1);

        button2 = GameObject.Find("Button2").gameObject.GetComponent<Button>();
        button2.GetComponent<UIEventScript>().onClickEventId = EventId.OnClickButton2;
        EventManager.Instance.AddEventListener(EventId.OnClickButton2, OnClickButton2);
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(EventId.OnClickButton1, OnClickButton1);
        EventManager.Instance.RemoveEventListener(EventId.OnClickButton2, OnClickButton2);
    }

    public void OnClickButton1()
    {
        Debug.Log("OnClickButton1");
    }

    public void OnClickButton2()
    {
        Debug.Log("OnClickButton2");
    }
}
