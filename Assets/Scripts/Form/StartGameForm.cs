using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartGameForm : MonoBehaviour
{
    public string tempImagePath;
    public string playerImage;
    public string playerHp;
    public string monsterImage;
    public string monsterHp;


    public Button endRoundButton;
    public Button exitGameButton;

    public Button printPlayerButton;
    public Button printAIPlayerButton;

    private void Awake()
    {
        // TODO:make these strings to configs
        tempImagePath = "Images/temp";
        playerImage = "Player/Image";
        playerHp = "Player/Hp";
        monsterImage = "Monster/Image";
        monsterHp = "Monster/Hp";

        endRoundButton = GameObject.Find("EndRoundButton").gameObject.GetComponent<Button>();
        endRoundButton.GetComponent<UIEventScript>().onClickEventId = EventId.OnClickEndRoundButton;
        EventManager.Instance.AddEventListener(EventId.OnClickEndRoundButton, OnClickEndRoundButton);

        printPlayerButton = GameObject.Find("PrintPlayerButton").gameObject.GetComponent<Button>();
        printPlayerButton.GetComponent<UIEventScript>().onClickEventId = EventId.OnClickPrintPlayerButton;
        EventManager.Instance.AddEventListener(EventId.OnClickPrintPlayerButton, OnClickPrintPlayerButton);

        printAIPlayerButton = GameObject.Find("PrintAIPlayerButton").gameObject.GetComponent<Button>();
        printAIPlayerButton.GetComponent<UIEventScript>().onClickEventId = EventId.OnClickPrintAIPlayerButton;
        EventManager.Instance.AddEventListener(EventId.OnClickPrintAIPlayerButton, OnClickPrintAIPlayerButton);

        exitGameButton = GameObject.Find("ExitGameButton").gameObject.GetComponent<Button>();
        exitGameButton.GetComponent<UIEventScript>().onClickEventId = EventId.OnClickExitGameButton;
        EventManager.Instance.AddEventListener(EventId.OnClickExitGameButton, OnClickExitGameButton);

        Sprite sprite = Resources.Load<Sprite>(tempImagePath);
        GameObject.Find(playerImage).GetComponent<Image>().sprite = sprite;
        GameObject.Find(playerHp).GetComponent<Image>().sprite = sprite;
        GameObject.Find(monsterImage).GetComponent<Image>().sprite = sprite;
        GameObject.Find(monsterHp).GetComponent<Image>().sprite = sprite;
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(EventId.OnClickEndRoundButton, OnClickEndRoundButton);
        EventManager.Instance.RemoveEventListener(EventId.OnClickExitGameButton, OnClickExitGameButton);
        EventManager.Instance.RemoveEventListener(EventId.OnClickPrintPlayerButton, OnClickPrintPlayerButton);
        EventManager.Instance.RemoveEventListener(EventId.OnClickPrintAIPlayerButton, OnClickPrintAIPlayerButton);
    }

    private void Update()
    {
        Gamecore.Instance.Update();
    }

    public void OnClickEndRoundButton()
    {
        Gamecore.Instance.EndRound();
    }

    public void OnClickExitGameButton()
    {
        // temp use card
        Gamecore.Instance.player.DefaultUseCard();
    }

    public void OnClickPrintPlayerButton()
    {
        Debug.Log(Gamecore.Instance.player.ToString());
    }

    public void OnClickPrintAIPlayerButton()
    {
        Debug.Log(Gamecore.Instance.aiPlayer.ToString());
    }
}
