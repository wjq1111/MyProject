using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 卡片基类和各种卡片的实现

// 卡片使用效果类型
public enum UseCardType
{
    MonsterCard,
    AttributeCard,
    LGCard,
}

public class CardBase
{
    // 全局唯一的卡片id
    public int gid;
    // 卡片使用效果类型
    public UseCardType useCardType;
    // 卡片名称
    public string cardName;
    // 卡片外显名称
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
    // 怪物卡攻击
    public int attack;
    // 怪物卡防御
    public int defense;
    // 怪物卡血量
    public int hp;
    // 怪物卡行动次数
    public int actionNum;
    // 怪物卡攻击时，如果被防御住，造成的最低伤害
    public int minDamage;

    public MonsterCard() { }

    public MonsterCard(CardBase card)
    {
        this.gid = card.gid;
        this.useCardType = card.useCardType;
        this.cardName = card.cardName;
        this.outlookCardName = card.outlookCardName;
    }

    // 初始化怪物
    public void Init()
    {
        // config, id -> cfg
        this.attack = 100;
        this.defense = 100;
        this.hp = 100;
        this.actionNum = 1;
        this.minDamage = 1;
    }
}

public class AttributeCard : CardBase
{
    // 增幅攻击力
    public int addAttack;
    // 增幅防御力
    public int addDefense;
    // 增幅血量
    public int addHp;
    // 增幅行动次数
    public int addActionNum;

    public AttributeCard() {}

    public AttributeCard(CardBase card)
    {
        this.gid = card.gid;
        this.useCardType = card.useCardType;
        this.cardName = card.cardName;
        this.outlookCardName = card.outlookCardName;
    }

    public void Init()
    {
        // config, id -> cfg
        this.addAttack = 1;
        this.addDefense = 1;
        this.addHp = 1;
        this.addActionNum = 1;
    }
}

