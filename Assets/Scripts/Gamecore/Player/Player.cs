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
                card.outlookCardName = "¹ÖÎïlg";
            }
            else
            {
                card.useCardType = UseCardType.MonsterCard;
                card.cardName = "monster gwc";
                card.outlookCardName = "¹ÖÎïgwc";
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

    public void InitGameStartMonsters()
    {
        // TODO use config to init monsters, now directly write them
        for (int i = 0; i < maxMonsterNum; i++)
        {
            MonsterCard monsterCard = new MonsterCard();
            monsterCard.InitMonster();
            MonsterBase monster = new MonsterBase(monsterCard, campId);
            monsterList.Add(monster);
        }
    }

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
        UseCard(handCardList[0]);
    }

    public void UseCard(CardBase card)
    {
        if (card.useCardType == UseCardType.MonsterCard)
        {
            MonsterCard monsterCard = new(card);
            SummonMonster(monsterCard);
        }
        else if (card.useCardType == UseCardType.LGCard)
        {
            // ..
        }

        handCardList.Remove(card);
    }

    private void SummonMonster(MonsterCard monsterCard)
    {
        if (monsterList.Count == maxMonsterNum)
        {
            Debug.LogError("monsterList.Count == maxMonsterNum");
            return;
        }
        monsterCard.InitMonster();
        MonsterBase monster = new MonsterBase(monsterCard, this.campId);
        monsterList.Add(monster);
    }

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
