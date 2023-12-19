using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyForm : MonoBehaviour
{
    public Button startGameButton;

    private void Awake()
    {
        startGameButton = GameObject.Find("StartButton").gameObject.GetComponent<Button>();
        startGameButton.GetComponent<UIEventScript>().onClickEventId = EventId.OnClickStartGameButton;
        EventManager.Instance.AddEventListener(EventId.OnClickStartGameButton, OnClickStartGameButton);
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(EventId.OnClickStartGameButton, OnClickStartGameButton);
    }

    public void OnClickStartGameButton()
    {
        Debug.Log("OnClickStartGameButton");
        GameFsm.Instance.SetGameStatus(GameStatus.OnGoing);
    }
}
