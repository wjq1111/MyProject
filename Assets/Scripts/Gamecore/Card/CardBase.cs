// ��Ƭ����͸��ֿ�Ƭ��ʵ��

// ��Ƭʹ��Ч������
using System.Collections.Generic;
using UnityEngine;

public enum UseCardType
{
    Invalid,
    MonsterCard,
    AttributeCard,
    LGCard,
}

// ��Ƭ����
public class CardAbility
{
    public Ability ability;
    public int abilityCardId;
}

// ��Ƭ�ٻ���������
public class CardAbilitySummonMonster : CardAbility
{
    public int summonCount;
    public MonsterCard monsterCard;

    public CardAbilitySummonMonster()
    {
        ability = Ability.Invalid;
    }

    public CardAbilitySummonMonster(CardAbility ability)
    {
        this.ability = ability.ability;
        // ��ʼ�����Ź��￨
        this.monsterCard.id = this.abilityCardId;
        this.monsterCard.Init();
    }
}

// ��Ƭ�޸Ĺ�����������
public class CardAbilityModifyMonsterAttribute : CardAbility
{
    public int hurtCount;
    public AttributeCard attributeCard;

    public CardAbilityModifyMonsterAttribute()
    {
        ability = Ability.Invalid;
    }

    public CardAbilityModifyMonsterAttribute(CardAbility ability)
    {
        this.ability = ability.ability;
        // ��ʼ���������Կ�
        this.attributeCard.id = this.abilityCardId;
        this.attributeCard.Init();
    }
}

public class CardBase
{
    // ���ñ��Ӧ��id
    public int id;
    // ȫ��Ψһ�Ŀ�Ƭid
    public int gid;
    // ��Ƭʹ��Ч������
    public UseCardType useCardType;
    // ��Ƭ����
    public string cardName;
    // ��Ƭ��������
    public string outlookCardName;
    // ��Ƭ����
    public List<CardAbility> cardAbility;

    public CardBase()
    {
        this.gid = AllocGid.Instance.Alloc();
        cardAbility = new List<CardAbility>();
    }

    public void Init(int cardId)
    {
        this.id = cardId;
        if (!ConfigManager.Instance.cardManualMap.ContainsKey(cardId))
        {
            Debug.LogError("card id not in card manual map" + cardId.ToString());
            return;
        }
        var cfgCard = ConfigManager.Instance.cardManualMap[cardId];
        this.cardName = cfgCard.CardName;
        this.useCardType = (UseCardType)cfgCard.UseCardType;

        // ��ʼ����������
        string abilityStr = cfgCard.Ability;
        if (abilityStr.Length != 0)
        {
            string[] subAbilityArray = abilityStr.Split(";");
            foreach (string subAbilityStr in subAbilityArray)
            {
                string[] ability = subAbilityStr.Split(",");
                if (ability.Length != 2)
                {
                    Debug.LogError("ability length not 2");
                    return;
                }
                Ability abilityType = (Ability)int.Parse(ability[0]);
                int abilityCardId = int.Parse(ability[1]);
                CardAbility newCardAbility = new CardAbility();
                newCardAbility.ability = abilityType;
                newCardAbility.abilityCardId = abilityCardId;
            }
        }
    }

    public override string ToString()
    {
        string str = "";
        str += "��Ƭ����: " + cardName + "\n";
        str += "gid: " + gid + "\n";
        str += "��Ƭʹ������: " + useCardType + "\n";
        str += "��Ƭ����:\n";
        foreach (var ability in cardAbility)
        {
            str += ability + " ";
        }
        return str;
    }
}

