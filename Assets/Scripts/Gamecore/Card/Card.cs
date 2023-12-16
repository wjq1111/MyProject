using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UseCardType
{
    MonsterCard,
    LGCard,
}

public class CardBase
{
    // 全局唯一的卡片id
    public int gid;
    public UseCardType useCardType;

    public string cardName;
    public string outlookCardName;

    public CardBase()
    {
        this.gid = AllocGid.Instance.Alloc();
    }

    public string PrintCard()
    {
        string result = "";
        result += "card: " + cardName + "\n";
        result += "gid: " + gid + "\n";
        result += "useCardType" + useCardType + "\n";
        return result;
    }
}

public class MonsterCard : CardBase
{
    public int attack;
    public int defense;
    public int hp;
    public int actionNum;
    public int minDamage;

    public MonsterCard() { }

    public MonsterCard(CardBase card)
    {
        this.gid = card.gid;
        this.useCardType = card.useCardType;
        this.cardName = card.cardName;
        this.outlookCardName = card.outlookCardName;
    }

    public void InitMonster()
    {
        // config, id -> cfg
        this.attack = 100;
        this.defense = 100;
        this.hp = 100;
        this.actionNum = 1;
        this.minDamage = 1;
    }
}

