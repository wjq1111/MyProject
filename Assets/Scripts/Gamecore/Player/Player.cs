using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    // player camp
    public CampId campId;

    // player card bag
    public List<CardBase> cardBag;
    // game card deck
    public List<CardBase> handCardList;

    // max num of monsters
    public int maxMonsterNum;
    // max num of card
    public int maxCardNum;
    // face
    public MonsterBase face;
    // in-game monsters
    public List<MonsterBase> monsterList;
    // each round draw card number
    public int eachRoundDrawCardNum;

    // is action first
    public bool isActionFirst;

    public void Init()
    {
        handCardList = new List<CardBase>();
        // TODO config
        maxMonsterNum = 4;
        eachRoundDrawCardNum = 2;
        maxCardNum = 10;

        if (campId == CampId.Myself)
        {
            isActionFirst = true;
        }
        if (campId == CampId.AI)
        {
            isActionFirst = false;
        }
        face = new MonsterBase();
        monsterList = new List<MonsterBase>(maxMonsterNum);

        InitCardBag();
    }

    // 初始化卡片背包
    private void InitCardBag()
    {
        cardBag = new List<CardBase>(maxCardNum);
        for (int i = 0; i < maxCardNum; i++)
        {
            CardBase card = new CardBase();
            if (i % 2 == 0)
            {
                card.useCardType = UseCardType.MonsterCard;
                card.cardName = "monster lg";
                card.outlookCardName = "怪物lg";
            }
            else
            {
                card.useCardType = UseCardType.MonsterCard;
                card.cardName = "monster gwc";
                card.outlookCardName = "怪物gwc";
            }

            cardBag.Add(card);
        }
    }

    public bool IsDead()
    {
        return face.IsDead();
    }

    // 获取怪物
    public MonsterBase GetMonster(int index)
    {
        if (index >= monsterList.Count)
        {
            return null;
        }
        return monsterList[index];
    }

    // 获取卡片
    public CardBase GetCard(int cardId)
    {
        foreach (var card in handCardList)
        {
            if (cardId == card.gid)
            {
                return card;
            }
        }
        return null;
    }

    // 计算怪物状态
    public void CalcStatus()
    {
        List<MonsterBase> removeList = new List<MonsterBase>();
        foreach (var monster in monsterList)
        {
            if (monster.IsDead())
            {
                removeList.Add(monster);
            }
        }
        foreach (var monster in removeList)
        {
            monsterList.Remove(monster);
        }
    }

    // 初始化游戏启动时怪物
    public void InitGameStartMonsters()
    {
        // TODO use config to init monsters, now directly write them
        for (int i = 0; i < maxMonsterNum; i++)
        {
            MonsterCard monsterCard = new MonsterCard();
            monsterCard.Init();
            MonsterBase monster = new MonsterBase(monsterCard, campId);
            monsterList.Add(monster);
        }
    }

    // 使用怪物卡
    public void UseMonsterCard(MonsterCard monsterCard)
    {
        if (monsterList.Count == maxMonsterNum)
        {
            Debug.LogError("monsterList.Count == maxMonsterNum");
            return;
        }
        MonsterBase monster = new MonsterBase(monsterCard, campId);
        monsterList.Add(monster);
    }

    // 使用属性卡        
    public void UseAttributeCard(AttributeCard attributeCard, List<int> indexs)
    {
        attributeCard.Init();
        if (attributeCard.attributeCardUseTargetType != AttributeCardUseTargetType.All)
        {
            foreach (int index in indexs)
            {
                if (index > maxMonsterNum)
                {
                    Debug.LogError("index > maxMonsterNum:" + index + " " + maxMonsterNum);
                    return;
                }
                MonsterBase monster = monsterList[index];
                monster.AddAttribute(attributeCard);
            }
        }
        else
        {
            foreach (var monster in monsterList)
            {
                monster.AddAttribute(attributeCard);
            }
        }
    }

    public void AfterUseCard(int cardId)
    {
        CardBase card = GetCard(cardId);
        if (card == null)
        {
            return;
        }
        handCardList.Remove(card);
    }

    // 每回合抽卡
    public void EachRoundDrawCard()
    {
        List<CardBase> canDrawCard = CalcCanDrawCard();

        if (canDrawCard.Count < eachRoundDrawCardNum)
        {
            Debug.LogError("init game start card fail" + canDrawCard.Count);
            return;
        }

        for (int i = 0; i < eachRoundDrawCardNum; i++)
        {
            handCardList.Add(canDrawCard[i]);
            Debug.Log("player: " + this.campId + " each round draw card:" + canDrawCard[i].cardName);
        }
    }

    // 计算可以抽卡的集合
    private List<CardBase> CalcCanDrawCard()
    {
        List<CardBase> result = new List<CardBase>();
        foreach (var card in cardBag)
        {
            bool found = false;
            foreach (var monster in monsterList)
            {
                if (card.gid == monster.id)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                continue;
            }

            foreach (var handCard in handCardList)
            {
                if (card.gid == handCard.gid)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                continue;
            }

            result.Add(card);
        }

        if (result.Count == 0)
        {
            Debug.Log("no more card to draw");
            return null;
        }

        // shuffle result
        result = Shuffle(result);
        return result;
    }

    private List<T> Shuffle<T>(List<T> original)
    {
        System.Random random = new System.Random();
        T temp;
        for (int i = 0; i < original.Count; i++)
        {
            int index = random.Next(0, original.Count - 1);
            if (index != i)
            {
                temp = original[i];
                original[i] = original[index];
                original[index] = temp;
            }
        }
        return original;
    }

    public override string ToString()
    {
        string str = "";
        str += "camp: " + this.campId + "\n";
        str += "cardBag: " + this.cardBag.Count + "\n";
        str += "cardBag:";
        foreach (CardBase card in this.cardBag)
        {
            str += card.ToString();
        }
        str += "\n";
        str += "handCardList:";
        foreach (CardBase card in this.handCardList)
        {
            str += card.ToString();
        }
        str += "\n";
        str += "monsterList:";
        foreach (MonsterBase monster in this.monsterList)
        {
            str += monster.ToString();
        }
        return str;
    }
}
