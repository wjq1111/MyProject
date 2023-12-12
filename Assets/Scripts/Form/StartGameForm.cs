using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartGameForm : MonoBehaviour
{
    public Button startGameButton;
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject obj = GameObject.Find("StartGameForm");
        startGameButton = obj.transform.Find("StartButton").gameObject.GetComponent<Button>();
        EventManager.Instance.AddEventListener(EventId.OnClickStartGameButton, OnClickStartGameButton);
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(EventId.OnClickStartGameButton, OnClickStartGameButton);
    }

    public void OnClickStartGameButton()
    {
        EventManager.Instance.DispatchEvent(EventId.StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
