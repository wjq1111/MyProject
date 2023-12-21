// ��Ƭ����͸��ֿ�Ƭ��ʵ��

// ��Ƭʹ��Ч������
using System.Collections.Generic;

public enum UseCardType
{
    MonsterCard,
    AttributeCard,
    LGCard,
}

// ��Ƭ����
public class CardAbility
{
    public Ability ability;
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
    }
}

// ��Ƭ�˺���������
public class CardAbilityHurtMonster : CardAbility
{
    public int hurtCount;
    public AttributeCard attributeCard;

    public CardAbilityHurtMonster()
    {
        ability = Ability.Invalid;
    }

    public CardAbilityHurtMonster(CardAbility ability)
    {
        this.ability = ability.ability;
    }
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
    // ��Ƭ����
    public List<CardAbility> cardAbility;

    public CardBase()
    {
        this.gid = AllocGid.Instance.Alloc();
        cardAbility = new List<CardAbility>();
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

