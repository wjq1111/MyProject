using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ƭ����͸��ֿ�Ƭ��ʵ��

// ��Ƭʹ��Ч������
public enum UseCardType
{
    MonsterCard,
    AttributeCard,
    LGCard,
}

public class CardBase
{
    // ȫ��Ψһ�Ŀ�Ƭid
    public int gid;
    // ��Ƭʹ��Ч������
    public UseCardType useCardType;
    // ��Ƭ����
    public string cardName;
    // ��Ƭ��������
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
    // ���￨����
    public int attack;
    // ���￨����
    public int defense;
    // ���￨Ѫ��
    public int hp;
    // ���￨�ж�����
    public int actionNum;
    // ���￨����ʱ�����������ס����ɵ�����˺�
    public int minDamage;

    public MonsterCard() { }

    public MonsterCard(CardBase card)
    {
        this.gid = card.gid;
        this.useCardType = card.useCardType;
        this.cardName = card.cardName;
        this.outlookCardName = card.outlookCardName;
    }

    // ��ʼ������
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
    // ����������
    public int addAttack;
    // ����������
    public int addDefense;
    // ����Ѫ��
    public int addHp;
    // �����ж�����
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

