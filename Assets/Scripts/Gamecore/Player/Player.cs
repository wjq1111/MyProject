using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CampId
{
    Invalid,
    Mine,
    AI,
}

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

        if (campId == CampId.Mine)
        {
            isActionFirst = true;
        }
        if (campId == CampId.AI)
        {
            isActionFirst = false;
        }

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

    public void PrintPlayer()
    {
        string str = "";
        str += "camp: " + this.campId + "\n";
        str += "cardBag: " + this.cardBag.Count + "\n";
        str += "cardBag:";
        foreach (CardBase card in this.cardBag) {
            str += card.PrintCard();
        }
        str += "\n";
        str += "handCardList:";
        foreach (CardBase card in this.handCardList)
        {
            str += card.PrintCard();
        }
        str += "\n";
        str += "monsterList:";
        foreach (MonsterBase monster in this.monsterList)
        {
            str += monster.PrintMonster();
        }
        Debug.Log(str);
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

    // 操作怪物发起攻击
    public void Fight(int index, int oppoIndex)
    {
        MonsterBase myMonster = monsterList[index];
        MonsterBase oppoMonster = Gamecore.Instance.aiPlayer.monsterList[oppoIndex];
        myMonster.Fight(ref oppoMonster);

        if (myMonster.IsDead())
        {
            monsterList.RemoveAt(index);
        }
        if (oppoMonster.IsDead())
        {
            Gamecore.Instance.aiPlayer.monsterList.RemoveAt(oppoIndex);
        }
    }

    // 每回合抽卡
    public void EachRoundDrawCard(int number)
    {
        List<CardBase> canDrawCard = CalcCanDrawCard();

        if (canDrawCard.Count < eachRoundDrawCardNum)
        {
            Debug.LogError("init game start card fail" + canDrawCard.Count);
            return;
        }

        for (int i = 0; i < number; i++)
        {
            handCardList.Add(canDrawCard[i]);
            Debug.Log("player: " + this.campId + " each round draw card:" + canDrawCard[i].cardName);
        }
    }

    public void DefaultUseCard()
    {
        UseCard(0, 0);
    }

    // 使用卡片
    public void UseCard(int cardIndex, int monsterIndex)
    {
        if (cardIndex > handCardList.Count)
        {
            Debug.LogError("can not use card" + cardIndex + " " + handCardList.Count);
            return;
        }
        CardBase card = handCardList[cardIndex];
        if (card.useCardType == UseCardType.MonsterCard)
        {
            MonsterCard monsterCard = new(card);
            OnUseMonsterCard(monsterCard, 0);
        }
        else if (card.useCardType == UseCardType.AttributeCard)
        {
            AttributeCard attributeCard = new(card);
            OnUseAttributeCard(attributeCard, monsterIndex);
        }
        else if (card.useCardType == UseCardType.LGCard)
        {
            // ..
        }

        handCardList.Remove(card);
    }

    // 使用召唤怪物卡
    private void OnUseMonsterCard(MonsterCard monsterCard, int index)
    {
        if (monsterList.Count == maxMonsterNum)
        {
            Debug.LogError("monsterList.Count == maxMonsterNum");
            return;
        }
        if (monsterList[index] != null)
        {
            Debug.LogError("already have a monster");
            return;
        }
        monsterCard.Init();
        MonsterBase monster = new MonsterBase(monsterCard, this.campId);
        monsterList[index] = monster;
    }

    // 使用属性卡        
    private void OnUseAttributeCard(AttributeCard attributeCard, int index)
    {
        attributeCard.Init();
        if (index > maxMonsterNum)
        {
            Debug.LogError("index > maxMonsterNum:" + index + " " + maxMonsterNum);
            return;
        }
        MonsterBase monster = monsterList[index];
        monster.AddAttribute(attributeCard);
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
}
