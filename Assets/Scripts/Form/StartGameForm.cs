using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartGameForm : MonoBehaviour
{
    public string tempImagePath;
    public string playerImage;
    public string playerHp;
    public string monsterImage;
    public string monsterHp;
    public string buttonPath;


    public Button endRoundButton;
    public Button exitGameButton;

    public Button printPlayerButton;
    public Button printAIPlayerButton;

    public Text statusText;

    // 手牌区
    public GameObject playerCardList;
    public GameObject aiCardList;

    // 怪物区
    public GameObject playerMonsterList;
    public GameObject aiMonsterList;

    // 记录点击状态
    private GameObject firstlyClickedObject;
    private List<GameObject> secondlyClickedObject;

    private void Awake()
    {
        // TODO:make these strings to configs
        tempImagePath = "Images/temp";
        playerImage = "Player/Image";
        playerHp = "Player/Hp";
        monsterImage = "Monster/Image";
        monsterHp = "Monster/Hp";
        buttonPath = "Prefabs/Card";

        // 主体按钮
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

        // 状态展示
        statusText = GameObject.Find("DebugStatus/Viewport/Content").gameObject.GetComponent<Text>();
        EventManager.Instance.AddEventListener(EventId.FlushDebugStatus, OnFlushDebugStatus);

        // 手牌区
        playerCardList = GameObject.Find("PlayerCardList");
        aiCardList = GameObject.Find("AICardList");
        EventManager.Instance.AddEventListener(EventId.OnClickPlayerCardButton, OnClickPlayerCardButton);
        EventManager.Instance.AddEventListener(EventId.OnEnterPlayerCardButton, OnEnterPlayerCardButton);

        // 怪物展示区
        playerMonsterList = GameObject.Find("PlayerMonsterList");
        aiMonsterList = GameObject.Find("AIMonsterList");
        EventManager.Instance.AddEventListener(EventId.OnClickMonsterButton, OnClickMonsterButton);


        // 记录点击状态
        secondlyClickedObject = new List<GameObject>();
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(EventId.OnClickEndRoundButton, OnClickEndRoundButton);
        EventManager.Instance.RemoveEventListener(EventId.OnClickExitGameButton, OnClickExitGameButton);
        EventManager.Instance.RemoveEventListener(EventId.OnClickPrintPlayerButton, OnClickPrintPlayerButton);
        EventManager.Instance.RemoveEventListener(EventId.OnClickPrintAIPlayerButton, OnClickPrintAIPlayerButton);
        EventManager.Instance.RemoveEventListener(EventId.FlushDebugStatus, OnFlushDebugStatus);
        EventManager.Instance.RemoveEventListener(EventId.OnClickPlayerCardButton, OnClickPlayerCardButton);
        EventManager.Instance.RemoveEventListener(EventId.OnEnterPlayerCardButton, OnEnterPlayerCardButton);
    }

    private void Update()
    {
        Gamecore.Instance.Update();
    }

    // 刷新debug状态
    private void OnFlushDebugStatus()
    {
        // 刷新debug信息
        statusText.color = Color.red;
        statusText.text = Gamecore.Instance.ToString();

        // 刷新手牌
        FlushPlayerCard(playerCardList, CampId.Myself);
        FlushPlayerCard(aiCardList, CampId.AI);


        // 刷新场上怪物区
        FlushPlayerMonster(playerMonsterList, CampId.Myself);
        FlushPlayerMonster(aiMonsterList, CampId.AI);
    }

    // 刷新玩家手牌区
    private void FlushPlayerCard(GameObject playerCardList, CampId campId)
    {
        // 遍历playerCardList下所有子object，全部回收
        int allCardCount = playerCardList.transform.childCount;
        for (int i = allCardCount - 1; i >= 0; i--)
        {
            PoolManager.Instance.PushObject(ResourceType.PlayerCardButton, playerCardList.transform.GetChild(i).gameObject);
        }

        // 加入新的object
        var handCardList = Gamecore.Instance.GetPlayer(campId).handCardList;
        for (int i = 0; i < handCardList.Count; i++)
        {
            CardBase handCard = handCardList[i];
            GameObject playerCardButton = PoolManager.Instance.GetObject(ResourceType.PlayerCardButton);

            float posX = (i - handCardList.Count / 2) * playerCardButton.transform.GetComponent<RectTransform>().sizeDelta.x;
            playerCardButton.transform.SetParent(playerCardList.transform);
            playerCardButton.transform.SetLocalPositionAndRotation(new Vector3(posX, 0, 0), new Quaternion());
            playerCardButton.transform.Find("CardText").GetComponent<Text>().text = handCard.ToString();
            playerCardButton.GetComponent<Image>().color = Color.white;

            // 绑定按钮参数
            var eventScript = playerCardButton.GetComponent<UIEventScript>();
            eventScript.eventParams.campId = campId;
            eventScript.eventParams.buttonIndex = i;
            eventScript.eventParams.cardId = handCard.gid;
            eventScript.onClickEventId = EventId.OnClickPlayerCardButton;
            eventScript.onEnterEventId = EventId.OnEnterPlayerCardButton;
        }
    }

    // 刷新怪物区
    private void FlushPlayerMonster(GameObject playerMonsterList, CampId campId)
    {
        // 遍历playerMonsterList下所有子object，全部回收
        int allMonsterCount = playerMonsterList.transform.childCount;
        for (int i = allMonsterCount - 1; i >= 0; i--)
        {
            PoolManager.Instance.PushObject(ResourceType.MonsterButton, playerMonsterList.transform.GetChild(i).gameObject);
        }

        // 加入新的object
        Player player = Gamecore.Instance.GetPlayer(campId);
        var monsterList = player.monsterList;
        for (int i = 0; i < player.maxMonsterNum; i++)
        {
            GameObject monsterButton = PoolManager.Instance.GetObject(ResourceType.MonsterButton);
            float posX = (i - player.maxMonsterNum / 2) * monsterButton.transform.GetComponent<RectTransform>().sizeDelta.x;
            monsterButton.transform.SetParent(playerMonsterList.transform);
            monsterButton.transform.SetLocalPositionAndRotation(new Vector3(posX, 0, 0), new Quaternion());
            monsterButton.transform.Find("MonsterButton").GetComponent<Image>().color = Color.white;

            Text statusText = monsterButton.transform.Find("StatusText").GetComponent<Text>();
            statusText.color = Color.red;

            // 如果这个位置有怪的话，把怪物属性设置上
            if (i < monsterList.Count)
            {
                MonsterBase monster = monsterList[i];
                statusText.text = monster.ToString();
            }
            else
            {
                MonsterBase monster = new MonsterBase();
                statusText.text = monster.ToString();
            }

            // 绑定按钮参数
            var eventScript = monsterButton.transform.Find("MonsterButton").GetComponent<Button>().GetComponent<UIEventScript>();
            eventScript.eventParams.campId = campId;
            eventScript.eventParams.buttonIndex = i;
            eventScript.onClickEventId = EventId.OnClickMonsterButton;
        }
    }

    // 点击玩家手牌回调
    public void OnClickPlayerCardButton(UIEventParams eventParams)
    {
        CampId campId = eventParams.campId;
        int buttonIndex = eventParams.buttonIndex;

        GameObject player = campId == CampId.Myself ? playerCardList : aiCardList;
        // 给button涂个颜色表示自己被点击了，把不是这个button的颜色弄回来
        for (int i = 0; i < player.transform.childCount; i++)
        {
            GameObject card = player.transform.GetChild(i).gameObject;
            if (card.GetComponent<UIEventScript>().eventParams.buttonIndex == buttonIndex)
            {
                card.GetComponent<Image>().color = Color.red;
                // 点了自己的手牌区，那必须是直接改变首先选中的东西 ？
                firstlyClickedObject = card;
                secondlyClickedObject.Clear();
            }
            else
            {
                card.GetComponent<Image>().color = Color.white;
            }
        }
    }

    // 点击怪物区按钮
    public void OnClickMonsterButton(UIEventParams eventParams)
    {
        CampId campId = eventParams.campId;
        int buttonIndex = eventParams.buttonIndex;

        GameObject player = campId == CampId.Myself ? playerMonsterList : aiMonsterList;
        // 给button涂个颜色表示自己被点击了，把不是这个button的颜色弄回来
        for (int i = 0; i < player.transform.childCount; i++)
        {
            GameObject card = player.transform.GetChild(i).gameObject;
            Transform monsterButton = card.transform.Find("MonsterButton");
            var monsterScript = monsterButton.GetComponent<UIEventScript>();
            if (monsterScript.eventParams.buttonIndex == buttonIndex)
            {
                monsterButton.GetComponent<Image>().color = Color.red;
                // 如果存在第一次点击的物体，且第二次点了怪物区
                if (firstlyClickedObject != null)
                {
                    var firstlyClickedScript = firstlyClickedObject.GetComponent<UIEventScript>();
                    Gamecore.Instance.UseCard(firstlyClickedScript.eventParams.campId, firstlyClickedScript.eventParams.cardId);
                }
            }
            else
            {
                monsterButton.GetComponent<Image>().color = Color.white;
            }
        }
    }

    // 光标移入玩家手牌区回调
    public void OnEnterPlayerCardButton()
    {
        
    }

    public void OnClickEndRoundButton()
    {
        Gamecore.Instance.StartRound();
    }

    public void OnClickExitGameButton()
    {
        // temp use card
        Gamecore.Instance.UseCard(CampId.Myself, Gamecore.Instance.GetPlayer(CampId.Myself).handCardList[0].gid);
    }

    public void OnClickPrintPlayerButton()
    {
        Debug.Log(Gamecore.Instance.GetPlayer(CampId.Myself).ToString());
    }

    public void OnClickPrintAIPlayerButton()
    {
        Debug.Log(Gamecore.Instance.GetPlayer(CampId.AI).ToString());
    }
}
