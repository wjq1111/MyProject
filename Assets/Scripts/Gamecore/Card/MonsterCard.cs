using UnityEngine;

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
        if (!ConfigManager.Instance.monsterCardManualMap.ContainsKey(this.id))
        {
            Debug.LogError("card id not in monster card manual map");
            return;
        }
        var cfgCard = ConfigManager.Instance.monsterCardManualMap[this.id];
        this.attack = cfgCard.Attack;
        this.defense = cfgCard.Defense;
        this.hp = cfgCard.Hp;
        this.actionNum = cfgCard.ActionNum;
        this.minDamage = 1;
    }
}