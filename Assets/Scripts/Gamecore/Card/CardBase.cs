// 卡片基类和各种卡片的实现

// 卡片使用效果类型
using System.Collections.Generic;

public enum UseCardType
{
    MonsterCard,
    AttributeCard,
    LGCard,
}

// 卡片能力
public class CardAbility
{
    public Ability ability;
}

// 卡片召唤怪物能力
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

// 卡片伤害怪物能力
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
    // 全局唯一的卡片id
    public int gid;
    // 卡片使用效果类型
    public UseCardType useCardType;
    // 卡片名称
    public string cardName;
    // 卡片外显名称
    public string outlookCardName;
    // 卡片能力
    public List<CardAbility> cardAbility;

    public CardBase()
    {
        this.gid = AllocGid.Instance.Alloc();
        cardAbility = new List<CardAbility>();
    }

    public override string ToString()
    {
        string str = "";
        str += "卡片名称: " + cardName + "\n";
        str += "gid: " + gid + "\n";
        str += "卡片使用类型: " + useCardType + "\n";
        str += "卡片能力:\n";
        foreach (var ability in cardAbility)
        {
            str += ability + " ";
        }
        return str;
    }
}

